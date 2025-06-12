import React, { useState, useEffect } from 'react';
import { DashboardPageWrapper } from '../components/layout/DashboardPageWrapper';
import { GlassCard } from '../components/ui/GlassCard';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';
import { Table } from '../components/ui/Table';
import { Modal } from '../components/ui/Modal';
import { Phone, Calendar, MapPin, User } from 'lucide-react';
import { format } from 'date-fns';
import { es } from 'date-fns/locale';
import { Patient } from '../types';
import { useAuth } from '../contexts/AuthContext';
import { patientService } from '../services/patientService';

export function PatientsPage() {
  const [patients, setPatients] = useState<Patient[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingPatient, setEditingPatient] = useState<Patient | null>(null);
  const [formData, setFormData] = useState({
    nombre: '',
    apellido: '',
    fechaNacimiento: '',
    genero: '',
    direccion: '',
    telefono: '',
    correo: '',
    dni: '',
    ocupacion: '',
    seguroMedico: ''
  });

  const { user } = useAuth();

  useEffect(() => {
    loadPatients();
  }, []);

  const loadPatients = async () => {
    try {
      setLoading(true);
      const data = await patientService.getAll();
      setPatients(data);
    } catch (error) {
      setPatients([]);
    } finally {
      setLoading(false);
    }
  };

  const filteredPatients = patients.filter(patient =>
    `${patient.nombre} ${patient.apellido}`.toLowerCase().includes(searchQuery.toLowerCase()) ||
    patient.correo.toLowerCase().includes(searchQuery.toLowerCase()) ||
    patient.telefono.includes(searchQuery)
  );

  const handleOpenModal = (patient?: Patient) => {
    if (patient) {
      setEditingPatient(patient);
      setFormData({
        nombre: patient.nombre,
        apellido: patient.apellido,
        fechaNacimiento: patient.fechaNacimiento,
        genero: patient.genero,
        direccion: patient.direccion,
        telefono: patient.telefono,
        correo: patient.correo,
        dni: patient.dni || '',
        ocupacion: patient.ocupacion || '',
        seguroMedico: patient.seguroMedico || ''
      });
    } else {
      setEditingPatient(null);
      setFormData({
        nombre: '',
        apellido: '',
        fechaNacimiento: '',
        genero: '',
        direccion: '',
        telefono: '',
        correo: '',
        dni: '',
        ocupacion: '',
        seguroMedico: ''
      });
    }
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setEditingPatient(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const patientData = {
        nombre: formData.nombre,
        apellido: formData.apellido,
        fechaNacimiento: formData.fechaNacimiento,
        genero: formData.genero,
        direccion: formData.direccion,
        telefono: formData.telefono,
        correo: formData.correo,
        dni: formData.dni,
        ocupacion: formData.ocupacion,
        seguroMedico: formData.seguroMedico
      };
      if (editingPatient) {
        await patientService.update(editingPatient.id, patientData);
      } else {
        await patientService.create(patientData);
      }
      await loadPatients();
      handleCloseModal();
    } catch (error) {
      alert('Error al guardar el paciente. Por favor, intenta de nuevo.');
    }
  };

  const handleDelete = async (patientId: string) => {
    if (confirm('¿Estás seguro de que quieres eliminar este paciente?')) {
      try {
        await patientService.delete(patientId);
        await loadPatients();
      } catch (error) {
        alert('Error al eliminar el paciente. Por favor, intenta de nuevo.');
      }
    }
  };

  const columns = [
    {
      key: 'nombre',
      label: 'Nombre',
      render: (value: string, patient: Patient) => (
        <div>
          <p className="font-medium text-neutral-900 dark:text-neutral-100">{value} {patient.apellido}</p>
          <p className="text-sm text-neutral-500">{patient.correo}</p>
        </div>
      )
    },
    {
      key: 'telefono',
      label: 'Teléfono',
      render: (value: string) => (
        <div className="flex items-center space-x-2">
          <Phone className="w-4 h-4 text-neutral-400" />
          <span>{value}</span>
        </div>
      )
    },
    {
      key: 'fechaNacimiento',
      label: 'Fecha de Nacimiento',
      render: (value: string) => format(new Date(value), 'dd/MM/yyyy', { locale: es })
    },
    {
      key: 'genero',
      label: 'Género',
      render: (value: string) => value
    },
    {
      key: 'direccion',
      label: 'Dirección',
      render: (value: string) => value
    },
    {
      key: 'ocupacion',
      label: 'Ocupación',
      render: (value: string) => value || '-'
    },
    {
      key: 'seguroMedico',
      label: 'Seguro Médico',
      render: (value: string) => value || '-'
    },
    {
      key: 'actions',
      label: 'Acciones',
      render: (_: any, patient: Patient) => (
        <div className="flex items-center space-x-2">
          <Button variant="ghost" size="sm" onClick={() => handleOpenModal(patient)}>Editar</Button>
          <Button variant="ghost" size="sm" onClick={() => handleDelete(patient.id)} className="text-red-600">Eliminar</Button>
        </div>
      )
    }
  ];

  return (
    <DashboardPageWrapper>
      <div className="space-y-6">
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div className="flex items-center space-x-3">
            <div className="p-3 bg-secondary-100 dark:bg-secondary-900 rounded-xl">
              <User className="w-6 h-6 text-secondary-600 dark:text-secondary-400" />
            </div>
            <div>
              <h1 className="text-3xl font-bold font-montserrat text-neutral-900 dark:text-neutral-100">
                Gestión de Pacientes
              </h1>
              <p className="text-neutral-600 dark:text-neutral-400">
                {patients.length} pacientes registrados
              </p>
            </div>
          </div>
          <Button variant="primary" onClick={() => handleOpenModal()}>Nuevo Paciente</Button>
        </div>
        <GlassCard className="p-6">
          <div className="flex flex-col sm:flex-row gap-4">
            <Input
              placeholder="Buscar por nombre, correo o teléfono..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              icon={User}
              className="flex-1"
            />
          </div>
        </GlassCard>
        <GlassCard className="overflow-hidden">
          <div className="p-6 border-b border-neutral-200 dark:border-neutral-700">
            <div className="flex items-center justify-between">
              <div>
                <h2 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100">
                  Lista de Pacientes
                </h2>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Mostrando {filteredPatients.length} de {patients.length} pacientes
                </p>
              </div>
            </div>
          </div>
          <Table data={filteredPatients} columns={columns} searchable={false} />
        </GlassCard>
        <Modal isOpen={isModalOpen} onClose={handleCloseModal} title={editingPatient ? 'Editar Paciente' : 'Nuevo Paciente'} size="lg">
          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <Input label="Nombre" value={formData.nombre} onChange={e => setFormData(prev => ({ ...prev, nombre: e.target.value }))} required />
              <Input label="Apellido" value={formData.apellido} onChange={e => setFormData(prev => ({ ...prev, apellido: e.target.value }))} required />
              <Input label="Correo" value={formData.correo} onChange={e => setFormData(prev => ({ ...prev, correo: e.target.value }))} required />
              <Input label="Teléfono" value={formData.telefono} onChange={e => setFormData(prev => ({ ...prev, telefono: e.target.value }))} icon={Phone} required />
              <Input label="Fecha de Nacimiento" type="date" value={formData.fechaNacimiento} onChange={e => setFormData(prev => ({ ...prev, fechaNacimiento: e.target.value }))} icon={Calendar} required />
              <Input label="Género" value={formData.genero} onChange={e => setFormData(prev => ({ ...prev, genero: e.target.value }))} required />
              <Input label="Dirección" value={formData.direccion} onChange={e => setFormData(prev => ({ ...prev, direccion: e.target.value }))} icon={MapPin} required />
              <Input label="DNI" value={formData.dni} onChange={e => setFormData(prev => ({ ...prev, dni: e.target.value }))} />
              <Input label="Ocupación" value={formData.ocupacion} onChange={e => setFormData(prev => ({ ...prev, ocupacion: e.target.value }))} />
              <Input label="Seguro Médico" value={formData.seguroMedico} onChange={e => setFormData(prev => ({ ...prev, seguroMedico: e.target.value }))} />
            </div>
            <div className="flex justify-end space-x-3 pt-4">
              <Button type="button" variant="secondary" onClick={handleCloseModal}>Cancelar</Button>
              <Button type="submit" variant="primary">{editingPatient ? 'Actualizar' : 'Crear'} Paciente</Button>
            </div>
          </form>
        </Modal>
      </div>
    </DashboardPageWrapper>
  );
}