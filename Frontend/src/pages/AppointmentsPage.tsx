import React, { useState, useEffect } from 'react';
import { DashboardPageWrapper } from '../components/layout/DashboardPageWrapper';
import { GlassCard } from '../components/ui/GlassCard';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';
import { Table } from '../components/ui/Table';
import { Modal } from '../components/ui/Modal';
import {
  Calendar, Plus, Search, Filter, Clock, User, 
  CheckCircle, XCircle, AlertCircle, Edit, Trash2
} from 'lucide-react';
import { format, addDays } from 'date-fns';
import { es } from 'date-fns/locale';
import { Appointment, Patient } from '../types';
import { useAuth } from '../contexts/AuthContext';
import { appointmentService } from '../services/appointmentService';
import { patientService } from '../services/patientService';

// Helper para saber si una cita es de hoy
const isAppointmentToday = (appointment: Appointment): boolean => {
  const appointmentDate = (appointment.date || '').slice(0, 10);
  const today = new Date().toISOString().slice(0, 10);
  return appointmentDate === today;
};

export function AppointmentsPage() {
  const [activeSection, setActiveSection] = useState('appointments');
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [patients, setPatients] = useState<Patient[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState('');
  const [statusFilter, setStatusFilter] = useState('all');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingAppointment, setEditingAppointment] = useState<Appointment | null>(null);
  const [formData, setFormData] = useState({
    patientId: '',
    date: '',
    time: '',
    type: 'consultation',
    notes: ''
  });

  const { user } = useAuth();

  // Cargar datos al montar el componente
  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      console.log('Cargando datos...');
      
      // Cargar pacientes
      const patientsData = await patientService.getAll();
      setPatients(patientsData);
      console.log('Pacientes cargados:', patientsData);
      
      // Cargar citas
      const appointmentsData = await appointmentService.getAll();
      console.log('Citas cargadas:', appointmentsData);
      setAppointments(appointmentsData);
    } catch (error) {
      console.error('Error loading data:', error);
      // Fallback con datos mock si hay error
      setAppointments([
        {
          id: '1',
          patientId: '1',
          patientName: 'María García López',
          optometristId: '2',
          optometristName: 'Dr. Carlos Ruiz',
          date: format(new Date(), 'yyyy-MM-dd'),
          time: '09:00',
          type: 'consultation',
          status: 'scheduled',
          notes: 'Revisión anual completa'
        },
        {
          id: '2',
          patientId: '2',
          patientName: 'Carlos López Martín',
          optometristId: '2',
          optometristName: 'Dr. Carlos Ruiz',
          date: format(new Date(), 'yyyy-MM-dd'),
          time: '10:30',
          type: 'follow-up',
          status: 'in-progress',
          notes: 'Seguimiento post-cirugía'
        },
        {
          id: '3',
          patientId: '3',
          patientName: 'Ana Martínez Silva',
          optometristId: '2',
          optometristName: 'Dr. Carlos Ruiz',
          date: format(addDays(new Date(), 1), 'yyyy-MM-dd'),
          time: '11:00',
          type: 'emergency',
          status: 'scheduled',
          notes: 'Molestias en ojo derecho'
        }
      ]);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenModal = (appointment?: Appointment) => {
    if (appointment) {
      setEditingAppointment(appointment);
      setFormData({
        patientId: appointment.patientId,
        date: appointment.date,
        time: appointment.time,
        type: appointment.type,
        notes: appointment.notes || ''
      });
    } else {
      setEditingAppointment(null);
      setFormData({
        patientId: '',
        date: format(new Date(), 'yyyy-MM-dd'),
        time: '09:00',
        type: 'consultation',
        notes: ''
      });
    }
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setEditingAppointment(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    try {
      console.log('Creando/actualizando cita...');
      console.log('Form data:', formData);
      console.log('User:', user);
      
      const selectedPatient = patients.find(p => p.id === formData.patientId);
      if (!selectedPatient) {
        alert('Por favor selecciona un paciente válido');
        return;
      }

      if (editingAppointment) {
        // Actualizar cita existente
        console.log('Actualizando cita existente...');
        await appointmentService.update(editingAppointment.id, {
          date: formData.date,
          time: formData.time,
          type: formData.type as any,
          notes: formData.notes
        });
        console.log('Cita actualizada exitosamente');
      } else {
        // Crear nueva cita
        console.log('Creando nueva cita...');
        await appointmentService.create({
          patientId: formData.patientId,
          optometristId: user?.id || '2', // Current user
          date: formData.date,
          time: formData.time,
          type: formData.type as any,
          notes: formData.notes
        });
        console.log('Cita creada exitosamente');
      }

      console.log('Recargando datos...');
      await loadData(); // Recargar datos
      handleCloseModal();
      console.log('Modal cerrado, operación completada');
    } catch (error) {
      console.error('Error saving appointment:', error);
      console.error('Error details:', {
        message: error instanceof Error ? error.message : 'Unknown error',
        stack: error instanceof Error ? error.stack : undefined
      });
      alert('Error al guardar la cita. Por favor, intenta de nuevo.');
    }
  };

  const filteredAppointments = appointments.filter(appointment => {
    const matchesSearch = appointment.patientName.toLowerCase().includes(searchQuery.toLowerCase()) ||
                         appointment.optometristName.toLowerCase().includes(searchQuery.toLowerCase());
    const matchesStatus = statusFilter === 'all' || appointment.status === statusFilter;
    
    return matchesSearch && matchesStatus;
  });

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'scheduled':
        return <Clock className="w-4 h-4" />;
      case 'in-progress':
        return <AlertCircle className="w-4 h-4" />;
      case 'completed':
        return <CheckCircle className="w-4 h-4" />;
      case 'cancelled':
        return <XCircle className="w-4 h-4" />;
      default:
        return <Clock className="w-4 h-4" />;
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'scheduled':
        return 'bg-blue-100 text-blue-800 dark:bg-blue-900/20 dark:text-blue-400';
      case 'in-progress':
        return 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900/20 dark:text-yellow-400';
      case 'completed':
        return 'bg-green-100 text-green-800 dark:bg-green-900/20 dark:text-green-400';
      case 'cancelled':
        return 'bg-red-100 text-red-800 dark:bg-red-900/20 dark:text-red-400';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'scheduled':
        return 'Programada';
      case 'in-progress':
        return 'En Curso';
      case 'completed':
        return 'Completada';
      case 'cancelled':
        return 'Cancelada';
      default:
        return status;
    }
  };

  const getTypeText = (type: string) => {
    switch (type) {
      case 'consultation':
        return 'Consulta';
      case 'follow-up':
        return 'Seguimiento';
      case 'emergency':
        return 'Urgencia';
      default:
        return type;
    }
  };

  const handleStatusChange = async (appointmentId: string, newStatus: string) => {
    try {
      console.log('Actualizando estado de cita:', appointmentId, 'a:', newStatus);
      await appointmentService.updateStatus(appointmentId, newStatus);
      await loadData(); // Recargar datos
      console.log('Estado de cita actualizado exitosamente');
    } catch (error) {
      console.error('Error updating appointment status:', error);
      alert('Error al actualizar el estado de la cita. Por favor, intenta de nuevo.');
    }
  };

  const handleDelete = async (appointmentId: string) => {
    if (confirm('¿Estás seguro de que quieres eliminar esta cita?')) {
      try {
        console.log('Eliminando cita:', appointmentId);
        await appointmentService.delete(appointmentId);
        await loadData(); // Recargar datos
        console.log('Cita eliminada exitosamente');
      } catch (error) {
        console.error('Error deleting appointment:', error);
        alert('Error al eliminar la cita. Por favor, intenta de nuevo.');
      }
    }
  };

  const columns = [
    {
      key: 'date',
      label: 'Fecha',
      render: (value: string) => (
        <div>
          <p className="font-medium text-neutral-900 dark:text-neutral-100">
            {format(new Date(value), 'dd/MM/yyyy', { locale: es })}
          </p>
          <p className="text-sm text-neutral-500">
            {format(new Date(value), 'EEEE', { locale: es })}
          </p>
        </div>
      )
    },
    {
      key: 'time',
      label: 'Hora',
      render: (value: string) => (
        <div className="flex items-center space-x-2">
          <Clock className="w-4 h-4 text-neutral-400" />
          <span className="font-mono">{value}</span>
        </div>
      )
    },
    {
      key: 'patientName',
      label: 'Paciente',
      render: (value: string) => (
        <div className="flex items-center space-x-2">
          <User className="w-4 h-4 text-neutral-400" />
          <span className="font-medium">{value}</span>
        </div>
      )
    },
    {
      key: 'type',
      label: 'Tipo',
      render: (value: string) => (
        <span className={`px-2 py-1 rounded-full text-xs font-medium ${
          value === 'emergency' ? 'bg-red-100 text-red-800' :
          value === 'follow-up' ? 'bg-orange-100 text-orange-800' :
          'bg-blue-100 text-blue-800'
        }`}>
          {getTypeText(value)}
        </span>
      )
    },
    {
      key: 'status',
      label: 'Estado',
      render: (value: string, appointment: Appointment) => (
        <div className="flex items-center space-x-2">
          <div className={`flex items-center space-x-1 px-2 py-1 rounded-full text-xs font-medium ${getStatusColor(value)}`}>
            {getStatusIcon(value)}
            <span>{getStatusText(value)}</span>
          </div>
        </div>
      )
    },
    {
      key: 'actions',
      label: 'Acciones',
      render: (_: any, appointment: Appointment) => (
        <div className="flex items-center space-x-2">
          {appointment.status === 'scheduled' && (
            <Button
              variant="ghost"
              size="sm"
              onClick={() => handleStatusChange(appointment.id, 'in-progress')}
              className="p-1 hover:bg-yellow-50 text-yellow-600"
            >
              Iniciar
            </Button>
          )}
          {appointment.status === 'in-progress' && (
            <Button
              variant="ghost"
              size="sm"
              onClick={() => handleStatusChange(appointment.id, 'completed')}
              className="p-1 hover:bg-green-50 text-green-600"
            >
              Completar
            </Button>
          )}
          <Button
            variant="ghost"
            size="sm"
            icon={Edit}
            onClick={() => handleOpenModal(appointment)}
            className="p-1 hover:bg-blue-50"
          >
            Editar
          </Button>
          <Button
            variant="ghost"
            size="sm"
            icon={Trash2}
            onClick={() => handleDelete(appointment.id)}
            className="p-1 hover:bg-red-50 text-red-600"
          >
            Eliminar
          </Button>
        </div>
      )
    }
  ];

  return (
    <DashboardPageWrapper>
      <div className="space-y-6">
        {/* Header */}
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div className="flex items-center space-x-3">
            <div className="p-3 bg-secondary-100 dark:bg-secondary-900 rounded-xl">
              <Calendar className="w-6 h-6 text-secondary-600 dark:text-secondary-400" />
            </div>
            <div>
              <h1 className="text-3xl font-bold font-montserrat text-neutral-900 dark:text-neutral-100">
                Gestión de Citas
              </h1>
              <p className="text-neutral-600 dark:text-neutral-400">
                {appointments.length} citas programadas
              </p>
            </div>
          </div>
          <Button variant="primary" icon={Plus} onClick={() => handleOpenModal()}>
            Nueva Cita
          </Button>
        </div>

        {/* Stats Cards */}
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          {[
            { label: 'Hoy', count: appointments.filter(isAppointmentToday).length, color: 'bg-blue-500' },
            { label: 'Programadas', count: appointments.filter(a => a.status === 'scheduled').length, color: 'bg-yellow-500' },
            { label: 'En Curso', count: appointments.filter(a => a.status === 'in-progress').length, color: 'bg-green-500' },
            { label: 'Completadas', count: appointments.filter(a => a.status === 'completed').length, color: 'bg-gray-500' }
          ].map((stat, index) => (
            <GlassCard key={index} className="p-4">
              <div className="flex items-center space-x-3">
                <div className={`w-3 h-3 rounded-full ${stat.color}`} />
                <div>
                  <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                    {stat.count}
                  </p>
                  <p className="text-sm text-neutral-600 dark:text-neutral-400">
                    {stat.label}
                  </p>
                </div>
              </div>
            </GlassCard>
          ))}
        </div>

        {/* Filters */}
        <GlassCard className="p-6">
          <div className="flex flex-col sm:flex-row gap-4">
            <Input
              placeholder="Buscar por paciente o doctor..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              icon={Search}
              className="flex-1"
            />
            <select
              value={statusFilter}
              onChange={(e) => setStatusFilter(e.target.value)}
              className="px-4 py-2 border border-neutral-300 dark:border-neutral-600 rounded-lg bg-white/50 dark:bg-neutral-800/50 backdrop-blur-sm text-neutral-900 dark:text-neutral-100"
            >
              <option value="all">Todos los estados</option>
              <option value="scheduled">Programadas</option>
              <option value="in-progress">En curso</option>
              <option value="completed">Completadas</option>
              <option value="cancelled">Canceladas</option>
            </select>
            <Button variant="ghost" icon={Filter}>
              Más Filtros
            </Button>
          </div>
        </GlassCard>

        {/* Appointments Table */}
        <GlassCard className="overflow-hidden">
          <div className="p-6 border-b border-neutral-200 dark:border-neutral-700">
            <div className="flex items-center justify-between">
              <div>
                <h2 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100">
                  Lista de Citas
                </h2>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Mostrando {filteredAppointments.length} de {appointments.length} citas
                </p>
              </div>
            </div>
          </div>
          <Table
            data={filteredAppointments}
            columns={columns}
            searchable={false}
          />
        </GlassCard>
      </div>

      {/* Modal */}
      <Modal
        isOpen={isModalOpen}
        onClose={handleCloseModal}
        title={editingAppointment ? 'Editar Cita' : 'Nueva Cita'}
      >
        <form onSubmit={handleSubmit}>
          <div className="space-y-4">
            <div>
              <label htmlFor="patientId" className="block text-sm font-medium text-neutral-700 dark:text-neutral-300">
                Paciente
              </label>
              <select
                id="patientId"
                value={formData.patientId}
                onChange={(e) => setFormData({ ...formData, patientId: e.target.value })}
                className="mt-1 block w-full rounded-md border-neutral-300 dark:border-neutral-700 bg-white/50 dark:bg-neutral-800/50 text-neutral-900 dark:text-neutral-100"
              >
                <option value="">Selecciona un paciente</option>
                {patients.map((patient) => (
                  <option key={patient.id} value={patient.id}>
                    {patient.nombre} {patient.apellido}
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label htmlFor="date" className="block text-sm font-medium text-neutral-700 dark:text-neutral-300">
                Fecha
              </label>
              <input
                type="date"
                id="date"
                value={formData.date}
                onChange={(e) => setFormData({ ...formData, date: e.target.value })}
                className="mt-1 block w-full rounded-md border-neutral-300 dark:border-neutral-700 bg-white/50 dark:bg-neutral-800/50 text-neutral-900 dark:text-neutral-100"
              />
            </div>
            <div>
              <label htmlFor="time" className="block text-sm font-medium text-neutral-700 dark:text-neutral-300">
                Hora
              </label>
              <input
                type="time"
                id="time"
                value={formData.time}
                onChange={(e) => setFormData({ ...formData, time: e.target.value })}
                className="mt-1 block w-full rounded-md border-neutral-300 dark:border-neutral-700 bg-white/50 dark:bg-neutral-800/50 text-neutral-900 dark:text-neutral-100"
              />
            </div>
            <div>
              <label htmlFor="type" className="block text-sm font-medium text-neutral-700 dark:text-neutral-300">
                Tipo
              </label>
              <select
                id="type"
                value={formData.type}
                onChange={(e) => setFormData({ ...formData, type: e.target.value as any })}
                className="mt-1 block w-full rounded-md border-neutral-300 dark:border-neutral-700 bg-white/50 dark:bg-neutral-800/50 text-neutral-900 dark:text-neutral-100"
              >
                <option value="consultation">Consulta</option>
                <option value="follow-up">Seguimiento</option>
                <option value="emergency">Urgencia</option>
              </select>
            </div>
            <div>
              <label htmlFor="notes" className="block text-sm font-medium text-neutral-700 dark:text-neutral-300">
                Notas
              </label>
              <textarea
                id="notes"
                value={formData.notes}
                onChange={(e) => setFormData({ ...formData, notes: e.target.value })}
                className="mt-1 block w-full rounded-md border-neutral-300 dark:border-neutral-700 bg-white/50 dark:bg-neutral-800/50 text-neutral-900 dark:text-neutral-100"
              />
            </div>
          </div>
          <div className="mt-4 space-x-2">
            <Button type="submit">Guardar</Button>
            <Button type="reset" variant="ghost">Cancelar</Button>
          </div>
        </form>
      </Modal>
    </DashboardPageWrapper>
  );
}