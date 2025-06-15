import React, { useState, useEffect } from 'react';
import { DashboardPageWrapper } from '../components/layout/DashboardPageWrapper';
import { GlassCard } from '../components/ui/GlassCard';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';
import { Table } from '../components/ui/Table';
import { Modal } from '../components/ui/Modal';
import {
  UserCheck, Plus, Search, Filter, Edit, Trash2,
  Phone, Mail, Stethoscope, Calendar, Award, AlertCircle,
  Key, UserPlus, CheckCircle
} from 'lucide-react';
import { format } from 'date-fns';
import { es } from 'date-fns/locale';
import { optometristService, Optometrist, CreateOptometristRequest, CreateCredentialsRequest } from '../services/optometristService';
import { useAuth } from '../contexts/AuthContext';

export function OptometristsPage() {
  const [optometrists, setOptometrists] = useState<Optometrist[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [searchQuery, setSearchQuery] = useState('');
  const [searchType, setSearchType] = useState<'all' | 'name' | 'specialty'>('all');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isCredentialsModalOpen, setIsCredentialsModalOpen] = useState(false);
  const [editingOptometrist, setEditingOptometrist] = useState<Optometrist | null>(null);
  const [selectedOptometrist, setSelectedOptometrist] = useState<Optometrist | null>(null);
  const [formData, setFormData] = useState({
    nombre: '',
    apellido: '',
    cedulaProfesional: '',
    especialidad: '',
    correo: '',
    celular: '',
    numeroEmergencia: '',
    telefono: '',
    direccion: '',
    username: '',
    password: '',
    confirmPassword: ''
  });
  const [credentialsData, setCredentialsData] = useState({
    username: '',
    password: '',
    confirmPassword: ''
  });
  const [formErrors, setFormErrors] = useState<Record<string, string>>({});
  const [credentialsErrors, setCredentialsErrors] = useState<Record<string, string>>({});
  const [createCredentials, setCreateCredentials] = useState(false);

  const { user } = useAuth();

  // Cargar optometristas al montar el componente
  useEffect(() => {
    loadOptometrists();
  }, []);

  const loadOptometrists = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await optometristService.getAll();
      setOptometrists(data);
    } catch (err) {
      setError('Error al cargar los optometristas');
      console.error('Error loading optometrists:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = async () => {
    if (!searchQuery.trim()) {
      loadOptometrists();
      return;
    }

    try {
      setLoading(true);
      let results: Optometrist[] = [];

      switch (searchType) {
        case 'name':
          results = await optometristService.searchByName(searchQuery);
          break;
        case 'specialty':
          results = await optometristService.searchBySpecialty(searchQuery);
          break;
        default:
          results = await optometristService.getAll();
          results = results.filter(optometrist =>
            optometrist.nombre.toLowerCase().includes(searchQuery.toLowerCase()) ||
            optometrist.apellido.toLowerCase().includes(searchQuery.toLowerCase()) ||
            optometrist.correo.toLowerCase().includes(searchQuery.toLowerCase()) ||
            optometrist.especialidad.toLowerCase().includes(searchQuery.toLowerCase())
          );
      }

      setOptometrists(results);
    } catch (err) {
      setError('Error al buscar optometristas');
      console.error('Error searching optometrists:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setEditingOptometrist(null);
    setFormErrors({});
  };

  const handleCorreoChange = (value: string) => {
    setFormData(prev => ({ 
      ...prev, 
      correo: value,
      username: value // Sincronizar el correo con el username
    }));
  };

  const handleOpenModal = (optometrist?: Optometrist) => {
    if (optometrist) {
      setEditingOptometrist(optometrist);
      setFormData({
        nombre: optometrist.nombre,
        apellido: optometrist.apellido,
        cedulaProfesional: optometrist.cedulaProfesional,
        especialidad: optometrist.especialidad,
        correo: optometrist.correo,
        celular: optometrist.celular,
        numeroEmergencia: optometrist.numeroEmergencia,
        telefono: optometrist.telefono || '',
        direccion: optometrist.direccion || '',
        username: optometrist.username || optometrist.correo,
        password: '',
        confirmPassword: ''
      });
    } else {
      setEditingOptometrist(null);
      setFormData({
        nombre: '',
        apellido: '',
        cedulaProfesional: '',
        especialidad: '',
        correo: '',
        celular: '',
        numeroEmergencia: '',
        telefono: '',
        direccion: '',
        username: '',
        password: '',
        confirmPassword: ''
      });
    }
    setFormErrors({});
    setIsModalOpen(true);
  };

  const validateForm = (): boolean => {
    const errors: Record<string, string> = {};

    if (!formData.nombre.trim()) errors.nombre = 'El nombre es requerido';
    if (!formData.apellido.trim()) errors.apellido = 'El apellido es requerido';
    if (!formData.cedulaProfesional.trim()) errors.cedulaProfesional = 'La cédula profesional es requerida';
    if (!formData.especialidad.trim()) errors.especialidad = 'La especialidad es requerida';
    if (!formData.correo.trim()) errors.correo = 'El correo es requerido';
    if (!formData.celular.trim()) errors.celular = 'El celular es requerido';
    if (!formData.numeroEmergencia.trim()) errors.numeroEmergencia = 'El número de emergencia es requerido';

    // Validar formato de correo
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (formData.correo && !emailRegex.test(formData.correo)) {
      errors.correo = 'Formato de correo inválido';
    }

    // Validar credenciales si se están creando
    if (!editingOptometrist) {
      if (!formData.password) {
        errors.password = 'La contraseña es requerida';
      } else if (formData.password.length < 6) {
        errors.password = 'La contraseña debe tener al menos 6 caracteres';
      }

      if (formData.password !== formData.confirmPassword) {
        errors.confirmPassword = 'Las contraseñas no coinciden';
      }
    }

    setFormErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }

    try {
      if (editingOptometrist) {
        await optometristService.update(editingOptometrist.id, formData);
      } else {
        const createData: CreateOptometristRequest = {
          nombre: formData.nombre,
          apellido: formData.apellido,
          cedulaProfesional: formData.cedulaProfesional,
          especialidad: formData.especialidad,
          correo: formData.correo,
          celular: formData.celular,
          numeroEmergencia: formData.numeroEmergencia,
          telefono: formData.telefono || undefined,
          direccion: formData.direccion || undefined
        };
        
        // Crear el optometrista
        const newOptometrist = await optometristService.create(createData);
        
        // Crear credenciales automáticamente
        if (formData.password) {
          const credentials: CreateCredentialsRequest = {
            username: formData.username || formData.correo,
            password: formData.password
          };
          
          try {
            await optometristService.createCredentials(newOptometrist.id, credentials);
          } catch (credError) {
            console.error('Error creating credentials:', credError);
            // No fallar si las credenciales no se pueden crear
          }
        }
      }

      handleCloseModal();
      loadOptometrists(); // Recargar la lista
    } catch (err) {
      console.error('Error saving optometrist:', err);
      setError('Error al guardar el optometrista');
    }
  };

  const handleDelete = async (optometristId: string) => {
    if (!confirm('¿Estás seguro de que quieres eliminar este optometrista?')) {
      return;
    }

    try {
      await optometristService.delete(optometristId);
      loadOptometrists(); // Recargar la lista
    } catch (err) {
      console.error('Error deleting optometrist:', err);
      setError('Error al eliminar el optometrista');
    }
  };

  const handleOpenCredentialsModal = (optometrist: Optometrist) => {
    setSelectedOptometrist(optometrist);
    setCredentialsData({
      username: '',
      password: '',
      confirmPassword: ''
    });
    setCredentialsErrors({});
    setIsCredentialsModalOpen(true);
  };

  const handleCloseCredentialsModal = () => {
    setIsCredentialsModalOpen(false);
    setSelectedOptometrist(null);
    setCredentialsErrors({});
  };

  const validateCredentials = (): boolean => {
    const errors: Record<string, string> = {};

    if (!credentialsData.username.trim()) {
      errors.username = 'El nombre de usuario es requerido';
    } else if (credentialsData.username.length < 3) {
      errors.username = 'El nombre de usuario debe tener al menos 3 caracteres';
    }

    if (!credentialsData.password) {
      errors.password = 'La contraseña es requerida';
    } else if (credentialsData.password.length < 6) {
      errors.password = 'La contraseña debe tener al menos 6 caracteres';
    }

    if (credentialsData.password !== credentialsData.confirmPassword) {
      errors.confirmPassword = 'Las contraseñas no coinciden';
    }

    setCredentialsErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleCreateCredentials = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateCredentials() || !selectedOptometrist) {
      return;
    }

    try {
      const credentials: CreateCredentialsRequest = {
        username: credentialsData.username,
        password: credentialsData.password
      };

      await optometristService.createCredentials(selectedOptometrist.id, credentials);
      handleCloseCredentialsModal();
      loadOptometrists(); // Recargar para actualizar el estado de credenciales
    } catch (err) {
      console.error('Error creating credentials:', err);
      setError('Error al crear las credenciales');
    }
  };

  const columns = [
    {
      key: 'name',
      label: 'Optometrista',
      render: (_: any, optometrist: Optometrist) => (
        <div className="flex items-center space-x-3">
          <div className="w-10 h-10 bg-secondary-100 dark:bg-secondary-900 rounded-full flex items-center justify-center">
            <span className="text-secondary-600 dark:text-secondary-400 font-medium">
              {optometrist.nombre.charAt(0)}{optometrist.apellido.charAt(0)}
            </span>
          </div>
          <div>
            <p className="font-medium text-neutral-900 dark:text-neutral-100">
              {optometrist.nombre} {optometrist.apellido}
            </p>
            <p className="text-sm text-neutral-500">{optometrist.correo}</p>
            <p className="text-xs text-neutral-400">Cédula: {optometrist.cedulaProfesional}</p>
          </div>
        </div>
      )
    },
    {
      key: 'specialty',
      label: 'Especialidad',
      render: (_: any, optometrist: Optometrist) => (
        <div className="flex items-center space-x-2">
          <Award className="w-4 h-4 text-secondary-500" />
          <span>{optometrist.especialidad}</span>
        </div>
      )
    },
    {
      key: 'contact',
      label: 'Contacto',
      render: (_: any, optometrist: Optometrist) => (
        <div className="space-y-1">
          <div className="flex items-center space-x-2">
            <Phone className="w-4 h-4 text-neutral-400" />
            <span className="text-sm">{optometrist.celular}</span>
          </div>
          {optometrist.telefono && (
            <div className="flex items-center space-x-2">
              <Phone className="w-4 h-4 text-neutral-400" />
              <span className="text-sm text-neutral-500">{optometrist.telefono}</span>
            </div>
          )}
        </div>
      )
    },
    {
      key: 'status',
      label: 'Estado',
      render: (_: any, optometrist: Optometrist) => (
        <span className={`px-2 py-1 rounded-full text-xs font-medium ${
          optometrist.activo 
            ? 'bg-green-100 text-green-800 dark:bg-green-900/20 dark:text-green-400'
            : 'bg-red-100 text-red-800 dark:bg-red-900/20 dark:text-red-400'
        }`}>
          {optometrist.activo ? 'Activo' : 'Inactivo'}
        </span>
      )
    },
    {
      key: 'hireDate',
      label: 'Fecha Contratación',
      render: (_: any, optometrist: Optometrist) => (
        <span className="text-sm text-neutral-600 dark:text-neutral-400">
          {format(new Date(optometrist.fechaContratacion), 'dd/MM/yyyy', { locale: es })}
        </span>
      )
    },
    {
      key: 'actions',
      label: 'Acciones',
      render: (_: any, optometrist: Optometrist) => (
        <div className="flex items-center space-x-2">
          <Button
            variant="ghost"
            size="sm"
            icon={Edit}
            onClick={() => handleOpenModal(optometrist)}
            className="p-1 hover:bg-green-50"
          >
            <span className="sr-only">Editar</span>
          </Button>
          
          {!optometrist.hasLoginCredentials ? (
            <Button
              variant="ghost"
              size="sm"
              icon={Key}
              onClick={() => handleOpenCredentialsModal(optometrist)}
              className="p-1 hover:bg-blue-50 text-blue-600"
            >
              <span className="sr-only">Crear credenciales</span>
            </Button>
          ) : (
            <div className="flex items-center space-x-1 text-green-600">
              <CheckCircle className="w-4 h-4" />
              <span className="text-xs">Acceso</span>
            </div>
          )}
          
          <Button
            variant="ghost"
            size="sm"
            icon={Trash2}
            onClick={() => handleDelete(optometrist.id)}
            className="p-1 hover:bg-red-50 text-red-600"
          >
            <span className="sr-only">Eliminar</span>
          </Button>
        </div>
      )
    }
  ];

  const activeOptometrists = optometrists.filter(o => o.activo);
  const specialties = [...new Set(optometrists.map(o => o.especialidad))];

  if (loading && optometrists.length === 0) {
    return (
      <DashboardPageWrapper>
        <div className="flex items-center justify-center h-64">
          <div className="text-center">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-secondary-500 mx-auto mb-4"></div>
            <p className="text-neutral-600 dark:text-neutral-400">Cargando optometristas...</p>
          </div>
        </div>
      </DashboardPageWrapper>
    );
  }

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

        {/* Error Message */}
        {error && (
          <GlassCard className="p-4 border-red-200 dark:border-red-800">
            <div className="flex items-center space-x-2 text-red-600 dark:text-red-400">
              <AlertCircle className="w-5 h-5" />
              <span>{error}</span>
              <Button
                variant="ghost"
                size="sm"
                onClick={() => setError(null)}
                className="ml-auto"
              >
                ×
              </Button>
            </div>
          </GlassCard>
        )}

        {/* Stats Cards */}
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-green-100 dark:bg-green-900 rounded-lg">
                <UserCheck className="w-5 h-5 text-green-600 dark:text-green-400" />
              </div>
              <div>
                <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                  {activeOptometrists.length}
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
                  {specialties.length}
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
                  {optometrists.filter(o => {
                    const hireDate = new Date(o.fechaContratacion);
                    const currentDate = new Date();
                    const diffTime = Math.abs(currentDate.getTime() - hireDate.getTime());
                    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
                    return diffDays <= 30;
                  }).length}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Nuevos (30 días)
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
                  {optometrists.length - activeOptometrists.length}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Inactivos
                </p>
              </div>
            </div>
          </GlassCard>
        </div>

        {/* Filters */}
        <GlassCard className="p-6">
          <div className="flex flex-col sm:flex-row gap-4">
            <div className="flex-1 flex gap-2">
              <Input
                placeholder="Buscar por nombre, correo o especialidad..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                icon={Search}
                className="flex-1"
              />
              <select
                value={searchType}
                onChange={(e) => setSearchType(e.target.value as 'all' | 'name' | 'specialty')}
                className="px-3 py-2 border border-neutral-300 dark:border-neutral-600 rounded-lg bg-white dark:bg-neutral-800 text-neutral-900 dark:text-neutral-100"
              >
                <option value="all">Todos</option>
                <option value="name">Por nombre</option>
                <option value="specialty">Por especialidad</option>
              </select>
            </div>
            <Button variant="ghost" icon={Filter} onClick={handleSearch}>
              Buscar
            </Button>
            <Button variant="ghost" onClick={loadOptometrists}>
              Limpiar
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
                  Mostrando {optometrists.length} optometristas
                </p>
              </div>
            </div>
          </div>
          <Table
            data={optometrists}
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
                label="Nombre"
                value={formData.nombre}
                onChange={(e) => setFormData(prev => ({ ...prev, nombre: e.target.value }))}
                error={formErrors.nombre}
                required
              />
              <Input
                label="Apellido"
                value={formData.apellido}
                onChange={(e) => setFormData(prev => ({ ...prev, apellido: e.target.value }))}
                error={formErrors.apellido}
                required
              />
              <Input
                label="Cédula Profesional"
                value={formData.cedulaProfesional}
                onChange={(e) => setFormData(prev => ({ ...prev, cedulaProfesional: e.target.value }))}
                error={formErrors.cedulaProfesional}
                required
              />
              <Input
                label="Especialidad"
                value={formData.especialidad}
                onChange={(e) => setFormData(prev => ({ ...prev, especialidad: e.target.value }))}
                error={formErrors.especialidad}
                required
              />
              <Input
                label="Correo Electrónico"
                type="email"
                value={formData.correo}
                onChange={(e) => handleCorreoChange(e.target.value)}
                icon={Mail}
                error={formErrors.correo}
                required
              />
              <Input
                label="Celular"
                value={formData.celular}
                onChange={(e) => setFormData(prev => ({ ...prev, celular: e.target.value }))}
                icon={Phone}
                error={formErrors.celular}
                required
              />
              <Input
                label="Número de Emergencia"
                value={formData.numeroEmergencia}
                onChange={(e) => setFormData(prev => ({ ...prev, numeroEmergencia: e.target.value }))}
                icon={Phone}
                error={formErrors.numeroEmergencia}
                required
              />
              <Input
                label="Teléfono (opcional)"
                value={formData.telefono}
                onChange={(e) => setFormData(prev => ({ ...prev, telefono: e.target.value }))}
                icon={Phone}
              />
            </div>
            
            <Input
              label="Dirección (opcional)"
              value={formData.direccion}
              onChange={(e) => setFormData(prev => ({ ...prev, direccion: e.target.value }))}
              placeholder="Dirección completa"
            />
            
            {/* Campos de credenciales solo para nuevos optometristas */}
            {!editingOptometrist && (
              <>
                <div className="border-t border-neutral-200 dark:border-neutral-700 pt-6">
                  <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100 mb-4 flex items-center">
                    <Key className="w-5 h-5 mr-2 text-blue-600" />
                    Credenciales de Acceso
                  </h3>
                  <p className="text-sm text-neutral-600 dark:text-neutral-400 mb-4">
                    Configura las credenciales para que el optometrista pueda acceder al sistema.
                  </p>
                </div>
                
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <Input
                    label="Usuario (correo electrónico)"
                    value={formData.username}
                    onChange={(e) => setFormData(prev => ({ ...prev, username: e.target.value }))}
                    placeholder="Se llena automáticamente con el correo"
                    disabled
                  />
                  
                  <Input
                    label="Contraseña"
                    type="password"
                    value={formData.password}
                    onChange={(e) => setFormData(prev => ({ ...prev, password: e.target.value }))}
                    error={formErrors.password}
                    placeholder="Mínimo 6 caracteres"
                    required
                  />
                  
                  <Input
                    label="Confirmar Contraseña"
                    type="password"
                    value={formData.confirmPassword}
                    onChange={(e) => setFormData(prev => ({ ...prev, confirmPassword: e.target.value }))}
                    error={formErrors.confirmPassword}
                    placeholder="Repite la contraseña"
                    required
                  />
                </div>
              </>
            )}
            
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

        {/* Credentials Modal */}
        <Modal
          isOpen={isCredentialsModalOpen}
          onClose={handleCloseCredentialsModal}
          title={`Crear Credenciales - ${selectedOptometrist?.nombre} ${selectedOptometrist?.apellido}`}
          size="md"
        >
          <form onSubmit={handleCreateCredentials} className="space-y-6">
            <div className="bg-blue-50 dark:bg-blue-900/20 p-4 rounded-lg">
              <div className="flex items-center space-x-2 text-blue-700 dark:text-blue-300">
                <Key className="w-5 h-5" />
                <span className="font-medium">Configurar acceso al sistema</span>
              </div>
              <p className="text-sm text-blue-600 dark:text-blue-400 mt-2">
                Crea credenciales para que el optometrista pueda acceder al sistema con su propio usuario y contraseña.
              </p>
            </div>

            <div className="space-y-4">
              <Input
                label="Nombre de Usuario"
                value={credentialsData.username}
                onChange={(e) => setCredentialsData(prev => ({ ...prev, username: e.target.value }))}
                error={credentialsErrors.username}
                placeholder="Ej: dr.garcia"
                required
              />
              
              <Input
                label="Contraseña"
                type="password"
                value={credentialsData.password}
                onChange={(e) => setCredentialsData(prev => ({ ...prev, password: e.target.value }))}
                error={credentialsErrors.password}
                placeholder="Mínimo 6 caracteres"
                required
              />
              
              <Input
                label="Confirmar Contraseña"
                type="password"
                value={credentialsData.confirmPassword}
                onChange={(e) => setCredentialsData(prev => ({ ...prev, confirmPassword: e.target.value }))}
                error={credentialsErrors.confirmPassword}
                placeholder="Repite la contraseña"
                required
              />
            </div>
            
            <div className="flex justify-end space-x-3 pt-4">
              <Button variant="ghost" onClick={handleCloseCredentialsModal}>
                Cancelar
              </Button>
              <Button variant="primary" type="submit" icon={UserPlus}>
                Crear Credenciales
              </Button>
            </div>
          </form>
        </Modal>
      </div>
    </DashboardPageWrapper>
  );
}