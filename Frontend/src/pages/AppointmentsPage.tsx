import React, { useState } from 'react';
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
import { Appointment } from '../types';
import { useAuth } from '../contexts/AuthContext';

const mockAppointments: Appointment[] = [
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
];

// Helper para saber si una cita es de hoy
const isAppointmentToday = (appointment: Appointment): boolean => {
  const appointmentDate = (appointment.date || '').slice(0, 10);
  const today = new Date().toISOString().slice(0, 10);
  return appointmentDate === today;
};

export function AppointmentsPage() {
  const [activeSection, setActiveSection] = useState('appointments');
  const [appointments, setAppointments] = useState(mockAppointments);
  const [searchQuery, setSearchQuery] = useState('');
  const [statusFilter, setStatusFilter] = useState('all');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingAppointment, setEditingAppointment] = useState<Appointment | null>(null);

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

  const handleStatusChange = (appointmentId: string, newStatus: string) => {
    setAppointments(prev => prev.map(app => 
      app.id === appointmentId ? { ...app, status: newStatus as any } : app
    ));
  };

  const handleDelete = (appointmentId: string) => {
    if (confirm('¿Estás seguro de que quieres eliminar esta cita?')) {
      setAppointments(prev => prev.filter(app => app.id !== appointmentId));
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
            onClick={() => {/* Edit appointment */}}
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
          <Button variant="primary" icon={Plus}>
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
    </DashboardPageWrapper>
  );
}