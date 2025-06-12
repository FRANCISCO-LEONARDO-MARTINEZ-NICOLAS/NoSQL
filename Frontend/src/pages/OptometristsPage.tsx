import React, { useState } from 'react';
import { DashboardPageWrapper } from '../components/layout/DashboardPageWrapper';
import { GlassCard } from '../components/ui/GlassCard';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';
import { Table } from '../components/ui/Table';
import { Modal } from '../components/ui/Modal';
import {
  UserCheck, Plus, Search, Filter, Edit, Trash2,
  Phone, Mail, Stethoscope, Calendar, Award
} from 'lucide-react';
import { format } from 'date-fns';
import { es } from 'date-fns/locale';
import { User } from '../types';
import { useAuth } from '../contexts/AuthContext';

const mockOptometrists: User[] = [
  {
    id: '2',
    name: 'Dr. Carlos Ruiz',
    correo: 'carlos.ruiz@opticare.com',
    role: 'optometrist',
    specialty: 'Optometría Clínica',
    avatar: 'https://images.pexels.com/photos/6749778/pexels-photo-6749778.jpeg?auto=compress&cs=tinysrgb&w=100&h=100&dpr=2',
    phone: '+34 987 654 321',
  },
  {
    id: '3',
    name: 'Dra. Elena Martín',
    correo: 'elena.martin@opticare.com',
    role: 'optometrist',
    specialty: 'Lentes de Contacto',
    avatar: 'https://images.pexels.com/photos/5215024/pexels-photo-5215024.jpeg?auto=compress&cs=tinysrgb&w=100&h=100&dpr=2',
    phone: '+34 876 543 210',
  },
  {
    id: '4',
    name: 'Dr. Miguel Fernández',
    correo: 'miguel.fernandez@opticare.com',
    role: 'optometrist',
    specialty: 'Baja Visión',
    avatar: 'https://images.pexels.com/photos/5452201/pexels-photo-5452201.jpeg?auto=compress&cs=tinysrgb&w=100&h=100&dpr=2',
    phone: '+34 765 432 109',
  }
];

export function OptometristsPage() {
  const [activeSection, setActiveSection] = useState('optometrists');
  const [optometrists, setOptometrists] = useState(mockOptometrists);
  const [searchQuery, setSearchQuery] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingOptometrist, setEditingOptometrist] = useState<User | null>(null);
  const [formData, setFormData] = useState({
    name: '',
    correo: '',
    phone: '',
    specialty: '',
    avatar: ''
  });

  const filteredOptometrists = optometrists.filter(optometrist =>
    optometrist.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
    optometrist.correo.toLowerCase().includes(searchQuery.toLowerCase()) ||
    optometrist.specialty?.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const handleOpenModal = (optometrist?: User) => {
    if (optometrist) {
      setEditingOptometrist(optometrist);
      setFormData({
        name: optometrist.name,
        correo: optometrist.correo,
        phone: optometrist.phone || '',
        specialty: optometrist.specialty || '',
        avatar: optometrist.avatar || ''
      });
    } else {
      setEditingOptometrist(null);
      setFormData({
        name: '',
        correo: '',
        phone: '',
        specialty: '',
        avatar: ''
      });
    }
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setEditingOptometrist(null);
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    const optometristData: User = {
      id: editingOptometrist?.id || Date.now().toString(),
      name: formData.name,
      correo: formData.correo,
      role: 'optometrist',
      phone: formData.phone,
      specialty: formData.specialty,
      avatar: formData.avatar || `https://ui-avatars.com/api/?name=${formData.name}&background=2A5CAA&color=fff`
    };

    if (editingOptometrist) {
      setOptometrists(prev => prev.map(o => o.id === editingOptometrist.id ? optometristData : o));
    } else {
      setOptometrists(prev => [...prev, optometristData]);
    }

    handleCloseModal();
  };

  const handleDelete = (optometristId: string) => {
    if (confirm('¿Estás seguro de que quieres eliminar este optometrista?')) {
      setOptometrists(prev => prev.filter(o => o.id !== optometristId));
    }
  };

  const columns = [
    {
      key: 'name',
      label: 'Optometrista',
      render: (value: string, optometrist: User) => (
        <div className="flex items-center space-x-3">
          <img
            src={optometrist.avatar || `https://ui-avatars.com/api/?name=${value}&background=2A5CAA&color=fff`}
            alt={value}
            className="w-10 h-10 rounded-full"
          />
          <div>
            <p className="font-medium text-neutral-900 dark:text-neutral-100">{value}</p>
            <p className="text-sm text-neutral-500">{optometrist.correo}</p>
          </div>
        </div>
      )
    },
    {
      key: 'specialty',
      label: 'Especialidad',
      render: (value?: string) => (
        <div className="flex items-center space-x-2">
          <Award className="w-4 h-4 text-secondary-500" />
          <span>{value || 'General'}</span>
        </div>
      )
    },
    {
      key: 'phone',
      label: 'Teléfono',
      render: (value?: string) => value ? (
        <div className="flex items-center space-x-2">
          <Phone className="w-4 h-4 text-neutral-400" />
          <span>{value}</span>
        </div>
      ) : (
        <span className="text-neutral-400">No disponible</span>
      )
    },
    {
      key: 'status',
      label: 'Estado',
      render: () => (
        <span className="px-2 py-1 bg-green-100 text-green-800 dark:bg-green-900/20 dark:text-green-400 rounded-full text-xs font-medium">
          Activo
        </span>
      )
    },
    {
      key: 'actions',
      label: 'Acciones',
      render: (_: any, optometrist: User) => (
        <div className="flex items-center space-x-2">
          <Button
            variant="ghost"
            size="sm"
            icon={Edit}
            onClick={() => handleOpenModal(optometrist)}
            className="p-1 hover:bg-green-50"
          />
          <Button
            variant="ghost"
            size="sm"
            icon={Trash2}
            onClick={() => handleDelete(optometrist.id)}
            className="p-1 hover:bg-red-50 text-red-600"
          />
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
              <UserCheck className="w-6 h-6 text-secondary-600 dark:text-secondary-400" />
            </div>
            <div>
              <h1 className="text-3xl font-bold font-montserrat text-neutral-900 dark:text-neutral-100">
                Gestión de Optometristas
              </h1>
              <p className="text-neutral-600 dark:text-neutral-400">
                {optometrists.length} optometristas registrados
              </p>
            </div>
          </div>
          <Button 
            variant="primary" 
            icon={Plus}
            onClick={() => handleOpenModal()}
          >
            Nuevo Optometrista
          </Button>
        </div>

        {/* Stats Cards */}
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-green-100 dark:bg-green-900 rounded-lg">
                <UserCheck className="w-5 h-5 text-green-600 dark:text-green-400" />
              </div>
              <div>
                <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                  {optometrists.length}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Total Activos
                </p>
              </div>
            </div>
          </GlassCard>
          
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-blue-100 dark:bg-blue-900 rounded-lg">
                <Stethoscope className="w-5 h-5 text-blue-600 dark:text-blue-400" />
              </div>
              <div>
                <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                  {new Set(optometrists.map(o => o.specialty).filter(Boolean)).size}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Especialidades
                </p>
              </div>
            </div>
          </GlassCard>
          
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-yellow-100 dark:bg-yellow-900 rounded-lg">
                <Calendar className="w-5 h-5 text-yellow-600 dark:text-yellow-400" />
              </div>
              <div>
                <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                  24
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Citas Hoy
                </p>
              </div>
            </div>
          </GlassCard>
          
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-purple-100 dark:bg-purple-900 rounded-lg">
                <Award className="w-5 h-5 text-purple-600 dark:text-purple-400" />
              </div>
              <div>
                <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                  156
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Consultas Mes
                </p>
              </div>
            </div>
          </GlassCard>
        </div>

        {/* Filters */}
        <GlassCard className="p-6">
          <div className="flex flex-col sm:flex-row gap-4">
            <Input
              placeholder="Buscar por nombre, correo o especialidad..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              icon={Search}
              className="flex-1"
            />
            <Button variant="ghost" icon={Filter}>
              Filtros Avanzados
            </Button>
          </div>
        </GlassCard>

        {/* Optometrists Table */}
        <GlassCard className="overflow-hidden">
          <div className="p-6 border-b border-neutral-200 dark:border-neutral-700">
            <div className="flex items-center justify-between">
              <div>
                <h2 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100">
                  Lista de Optometristas
                </h2>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Mostrando {filteredOptometrists.length} de {optometrists.length} optometristas
                </p>
              </div>
            </div>
          </div>
          <Table
            data={filteredOptometrists}
            columns={columns}
            searchable={false}
          />
        </GlassCard>

        {/* Optometrist Form Modal */}
        <Modal
          isOpen={isModalOpen}
          onClose={handleCloseModal}
          title={editingOptometrist ? 'Editar Optometrista' : 'Nuevo Optometrista'}
          size="lg"
        >
          <form onSubmit={handleSubmit} className="space-y-6">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <Input
                label="Nombre Completo"
                value={formData.name}
                onChange={(e) => setFormData(prev => ({ ...prev, name: e.target.value }))}
                required
              />
              <Input
                label="correo"
                type="correo"
                value={formData.correo}
                onChange={(e) => setFormData(prev => ({ ...prev, correo: e.target.value }))}
                icon={Mail}
                required
              />
              <Input
                label="Teléfono"
                value={formData.phone}
                onChange={(e) => setFormData(prev => ({ ...prev, phone: e.target.value }))}
                icon={Phone}
              />
              <Input
                label="Especialidad"
                value={formData.specialty}
                onChange={(e) => setFormData(prev => ({ ...prev, specialty: e.target.value }))}
                icon={Award}
                placeholder="Ej: Optometría Clínica"
              />
            </div>
            
            <Input
              label="URL del Avatar (opcional)"
              value={formData.avatar}
              onChange={(e) => setFormData(prev => ({ ...prev, avatar: e.target.value }))}
              placeholder="https://ejemplo.com/avatar.jpg"
            />
            
            <div className="flex justify-end space-x-3 pt-4">
              <Button variant="ghost" onClick={handleCloseModal}>
                Cancelar
              </Button>
              <Button variant="primary" type="submit">
                {editingOptometrist ? 'Actualizar' : 'Crear'} Optometrista
              </Button>
            </div>
          </form>
        </Modal>
      </div>
    </DashboardPageWrapper>
  );
}