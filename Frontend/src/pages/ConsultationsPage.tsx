import React, { useState, useEffect } from 'react';
import { DashboardPageWrapper } from '../components/layout/DashboardPageWrapper';
import { GlassCard } from '../components/ui/GlassCard';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';
import { Table } from '../components/ui/Table';
import { Modal } from '../components/ui/Modal';
import {
  Eye, Plus, Search, FileText, Calendar, User,
  Edit, Trash2, Download, Stethoscope
} from 'lucide-react';
import { format } from 'date-fns';
import { es } from 'date-fns/locale';
import { Consultation, Patient } from '../types';
import { useAuth } from '../contexts/AuthContext';
import { patientService } from '../services/patientService';
import { consultationService, CreateConsultationRequest, UpdateConsultationRequest } from '../services/consultationService';
import { appointmentService } from '../services/appointmentService';

export function ConsultationsPage() {
  const [activeSection, setActiveSection] = useState('consultations');
  const [consultations, setConsultations] = useState<Consultation[]>([]);
  const [patients, setPatients] = useState<Patient[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingConsultation, setEditingConsultation] = useState<Consultation | null>(null);
  const [formData, setFormData] = useState({
    patientId: '',
    rightEyeVA: '',
    leftEyeVA: '',
    rightEyeSphere: '',
    rightEyeCylinder: '',
    rightEyeAxis: '',
    leftEyeSphere: '',
    leftEyeCylinder: '',
    leftEyeAxis: '',
    diagnosis: '',
    treatment: '',
    recommendations: '',
    followUpDate: '',
    symptoms: '',
    observations: ''
  });

  const { user } = useAuth();

  // Cargar datos al montar el componente
  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      // Cargar pacientes
      const patientsData = await patientService.getAll();
      setPatients(patientsData);
      
      // Cargar consultas
      const consultationsData = await consultationService.getAll();
      console.log('Consultas cargadas:', consultationsData); // Debug
      setConsultations(consultationsData);
    } catch (error) {
      console.error('Error loading data:', error);
      // Fallback con datos mock si hay error
      setConsultations([
        {
          id: '1',
          pacienteId: '1',
          optometristaId: '2',
          pacientecorreo: 'paciente@example.com',
          optometristacorreo: 'optometrista@example.com',
          fecha: '2024-01-15T10:00:00Z',
          motivo: 'Consulta de rutina',
          sintomas: 'Dolor de cabeza, visión borrosa',
          diagnostico: 'Miopía leve bilateral con astigmatismo',
          tratamiento: 'Prescripción de lentes correctivos',
          recomendaciones: 'Revisión anual, descansos visuales frecuentes',
          observaciones: 'Paciente cooperativo',
          agudezaVisual: {
            ojoDerecho: '20/25',
            ojoIzquierdo: '20/30'
          },
          refraccion: {
            ojoDerecho: { esfera: -1.5, cilindro: -0.5, eje: 90 },
            ojoIzquierdo: { esfera: -1.75, cilindro: -0.25, eje: 85 }
          },
          fechaSeguimiento: '2025-01-15T10:00:00Z',
          nombrePaciente: 'Juan',
          apellidoPaciente: 'Pérez',
          nombreOptometrista: 'Dr. María',
          apellidoOptometrista: 'García'
        }
      ]);
    } finally {
      setLoading(false);
    }
  };

  // Helper functions para manejar tipos
  const getVisualAcuityValue = (consultation: Consultation, eye: 'right' | 'left'): string => {
    if (consultation.agudezaVisual) {
      return eye === 'right' ? consultation.agudezaVisual.ojoDerecho : consultation.agudezaVisual.ojoIzquierdo;
    }
    if (consultation.visualAcuity) {
      return eye === 'right' ? consultation.visualAcuity.rightEye : consultation.visualAcuity.leftEye;
    }
    return '';
  };

  const getRefractionValue = (consultation: Consultation, eye: 'right' | 'left', field: 'sphere' | 'cylinder' | 'axis'): number => {
    if (consultation.refraccion) {
      const eyeData = eye === 'right' ? consultation.refraccion.ojoDerecho : consultation.refraccion.ojoIzquierdo;
      if (field === 'sphere') return eyeData.esfera;
      if (field === 'cylinder') return eyeData.cilindro;
      if (field === 'axis') return eyeData.eje;
    }
    if (consultation.refraction) {
      const eyeData = eye === 'right' ? consultation.refraction.rightEye : consultation.refraction.leftEye;
      return eyeData[field];
    }
    return 0;
  };

  // Obtener nombre del paciente
  const getPatientName = (consultation: Consultation) => {
    // Usar los campos nuevos si están disponibles, sino usar el método legacy
    if (consultation.nombrePaciente && consultation.apellidoPaciente) {
      return `${consultation.nombrePaciente} ${consultation.apellidoPaciente}`;
    }
    
    const patientId = consultation.pacienteId || consultation.patientId;
    if (patientId) {
      const patient = patients.find(p => p.id === patientId);
      return patient ? `${patient.nombre} ${patient.apellido}` : 'Paciente Desconocido';
    }
    
    return 'Paciente Desconocido';
  };

  const filteredConsultations = consultations.filter(consultation => {
    const patientName = getPatientName(consultation);
    const diagnosis = consultation.diagnostico || consultation.diagnosis || '';
    
    return patientName.toLowerCase().includes(searchQuery.toLowerCase()) ||
           diagnosis.toLowerCase().includes(searchQuery.toLowerCase());
  });

  const handleOpenModal = (consultation?: Consultation) => {
    if (consultation) {
      setEditingConsultation(consultation);
      
      setFormData({
        patientId: consultation.pacienteId || consultation.patientId || '',
        rightEyeVA: getVisualAcuityValue(consultation, 'right'),
        leftEyeVA: getVisualAcuityValue(consultation, 'left'),
        rightEyeSphere: getRefractionValue(consultation, 'right', 'sphere').toString(),
        rightEyeCylinder: getRefractionValue(consultation, 'right', 'cylinder').toString(),
        rightEyeAxis: getRefractionValue(consultation, 'right', 'axis').toString(),
        leftEyeSphere: getRefractionValue(consultation, 'left', 'sphere').toString(),
        leftEyeCylinder: getRefractionValue(consultation, 'left', 'cylinder').toString(),
        leftEyeAxis: getRefractionValue(consultation, 'left', 'axis').toString(),
        diagnosis: consultation.diagnostico || consultation.diagnosis || '',
        treatment: consultation.tratamiento || consultation.treatment || '',
        recommendations: consultation.recomendaciones || consultation.recommendations || '',
        followUpDate: consultation.fechaSeguimiento || consultation.followUpDate || '',
        symptoms: consultation.sintomas || '',
        observations: consultation.observaciones || ''
      });
    } else {
      setEditingConsultation(null);
      setFormData({
        patientId: '',
        rightEyeVA: '',
        leftEyeVA: '',
        rightEyeSphere: '',
        rightEyeCylinder: '',
        rightEyeAxis: '',
        leftEyeSphere: '',
        leftEyeCylinder: '',
        leftEyeAxis: '',
        diagnosis: '',
        treatment: '',
        recommendations: '',
        followUpDate: '',
        symptoms: '',
        observations: ''
      });
    }
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setEditingConsultation(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    try {
      console.log('Iniciando creación de consulta...');
      console.log('Form data:', formData);
      console.log('User:', user);
      
      const selectedPatient = patients.find(p => p.id === formData.patientId);
      console.log('Selected patient:', selectedPatient);
      
      if (!selectedPatient) {
        alert('Por favor selecciona un paciente válido');
        return;
      }

      if (editingConsultation) {
        // Actualizar consulta existente
        console.log('Actualizando consulta existente...');
        const updateData: UpdateConsultationRequest = {
          sintomas: formData.symptoms,
          diagnostico: formData.diagnosis,
          tratamiento: formData.treatment,
          recomendaciones: formData.recommendations,
          observaciones: formData.observations,
          agudezaVisual: {
            ojoDerecho: formData.rightEyeVA,
            ojoIzquierdo: formData.leftEyeVA
          },
          refraccion: {
            ojoDerecho: {
              esfera: parseFloat(formData.rightEyeSphere) || 0,
              cilindro: parseFloat(formData.rightEyeCylinder) || 0,
              eje: parseInt(formData.rightEyeAxis) || 0
            },
            ojoIzquierdo: {
              esfera: parseFloat(formData.leftEyeSphere) || 0,
              cilindro: parseFloat(formData.leftEyeCylinder) || 0,
              eje: parseInt(formData.leftEyeAxis) || 0
            }
          },
          fechaSeguimiento: formData.followUpDate ? new Date(formData.followUpDate).toISOString() : undefined
        };

        console.log('Update data:', updateData);
        await consultationService.update(editingConsultation.id, updateData);
        console.log('Consulta actualizada exitosamente');
      } else {
        // Crear nueva consulta
        console.log('Creando nueva consulta...');
        // Guardar solo la fecha YYYY-MM-DD
        const currentDate = new Date().toISOString().slice(0, 10);
        
        const createData: CreateConsultationRequest = {
          pacienteId: formData.patientId,
          optometristaId: user?.id || '2', // Current user
          pacientecorreo: selectedPatient.correo,
          optometristacorreo: user?.correo || 'optometrista@example.com',
          fecha: currentDate,
          motivo: 'Consulta de rutina',
          sintomas: formData.symptoms,
          diagnostico: formData.diagnosis,
          tratamiento: formData.treatment,
          recomendaciones: formData.recommendations,
          observaciones: formData.observations,
          agudezaVisual: {
            ojoDerecho: formData.rightEyeVA,
            ojoIzquierdo: formData.leftEyeVA
          },
          refraccion: {
            ojoDerecho: {
              esfera: parseFloat(formData.rightEyeSphere) || 0,
              cilindro: parseFloat(formData.rightEyeCylinder) || 0,
              eje: parseInt(formData.rightEyeAxis) || 0
            },
            ojoIzquierdo: {
              esfera: parseFloat(formData.leftEyeSphere) || 0,
              cilindro: parseFloat(formData.leftEyeCylinder) || 0,
              eje: parseInt(formData.leftEyeAxis) || 0
            }
          },
          fechaSeguimiento: formData.followUpDate ? new Date(formData.followUpDate).toISOString() : undefined
        };

        console.log('Create data:', createData);
        const result = await consultationService.create(createData);
        console.log('Consulta creada exitosamente:', result);

        // Si se estableció una fecha de seguimiento, crear automáticamente una cita
        if (formData.followUpDate) {
          try {
            console.log('Creando cita de seguimiento automáticamente...');
            await appointmentService.create({
              patientId: formData.patientId,
              optometristId: user?.id || '2',
              date: formData.followUpDate,
              time: '09:00', // Hora por defecto
              type: 'follow-up',
              notes: `Seguimiento programado desde consulta del ${currentDate}. Diagnóstico: ${formData.diagnosis}`
            });
            console.log('Cita de seguimiento creada exitosamente');
          } catch (appointmentError) {
            console.error('Error creating follow-up appointment:', appointmentError);
            // No mostrar error al usuario, solo log
          }
        }
      }

      console.log('Recargando datos...');
      await loadData(); // Recargar datos
      handleCloseModal();
      console.log('Modal cerrado, operación completada');
    } catch (error) {
      console.error('Error saving consultation:', error);
      console.error('Error details:', {
        message: error instanceof Error ? error.message : 'Unknown error',
        stack: error instanceof Error ? error.stack : undefined
      });
      alert('Error al guardar la consulta. Por favor, intenta de nuevo.');
    }
  };

  const handleDelete = async (consultationId: string) => {
    if (confirm('¿Estás seguro de que quieres eliminar esta consulta?')) {
      try {
        await consultationService.delete(consultationId);
        await loadData(); // Recargar datos
      } catch (error) {
        console.error('Error deleting consultation:', error);
        alert('Error al eliminar la consulta. Por favor, intenta de nuevo.');
      }
    }
  };

  const handleViewConsultation = (consultation: Consultation) => {
    // TODO: Implementar vista detallada de consulta
    alert(`Vista de consulta: ${consultation.diagnosis}`);
  };

  // Helper function para verificar si una consulta es de hoy
  const isConsultationToday = (consultation: Consultation): boolean => {
    const consultationDate = (consultation.fecha || consultation.date || '').slice(0, 10);
    const today = new Date().toISOString().slice(0, 10);
    return consultationDate === today;
  };

  const columns = [
    {
      key: 'fecha',
      label: 'Fecha',
      render: (value: string, consultation: Consultation) => {
        const date = consultation.fecha || consultation.date || value;
        return (
          <div>
            <p className="font-medium text-neutral-900 dark:text-neutral-100">
              {format(new Date(date), 'dd/MM/yyyy', { locale: es })}
            </p>
            <p className="text-sm text-neutral-500">
              {format(new Date(date), 'HH:mm')}
            </p>
          </div>
        );
      }
    },
    {
      key: 'pacienteId',
      label: 'Paciente',
      render: (value: string, consultation: Consultation) => (
        <div className="flex items-center space-x-2">
          <User className="w-4 h-4 text-neutral-400" />
          <span className="font-medium">{getPatientName(consultation)}</span>
        </div>
      )
    },
    {
      key: 'agudezaVisual',
      label: 'Agudeza Visual',
      render: (value: any, consultation: Consultation) => {
        const rightEye = getVisualAcuityValue(consultation, 'right');
        const leftEye = getVisualAcuityValue(consultation, 'left');
        return (
          <div className="text-sm">
            <div>OD: {rightEye}</div>
            <div>OI: {leftEye}</div>
          </div>
        );
      }
    },
    {
      key: 'diagnostico',
      label: 'Diagnóstico',
      render: (value: any, consultation: Consultation) => {
        const diagnosis = consultation.diagnostico || consultation.diagnosis || '';
        return (
          <div className="max-w-xs">
            <p className="text-sm truncate" title={diagnosis}>
              {diagnosis}
            </p>
          </div>
        );
      }
    },
    {
      key: 'fechaSeguimiento',
      label: 'Próxima Revisión',
      render: (value: any, consultation: Consultation) => {
        const followUpDate = consultation.fechaSeguimiento || consultation.followUpDate;
        return followUpDate 
          ? format(new Date(followUpDate), 'dd/MM/yyyy', { locale: es })
          : 'No programada';
      }
    },
    {
      key: 'actions',
      label: 'Acciones',
      render: (_: any, consultation: Consultation) => (
        <div className="flex items-center space-x-2">
          <Button
            variant="ghost"
            size="sm"
            icon={Download}
            onClick={() => {/* Generate report */}}
            className="p-1 hover:bg-blue-50"
          >
            Reporte
          </Button>
          <Button
            variant="ghost"
            size="sm"
            icon={Eye}
            onClick={() => handleViewConsultation(consultation)}
            className="text-blue-600 hover:text-blue-700"
          >
            Ver
          </Button>
          <Button
            variant="ghost"
            size="sm"
            icon={Edit}
            onClick={() => handleOpenModal(consultation)}
            className="text-green-600 hover:text-green-700"
          >
            Editar
          </Button>
          <Button
            variant="ghost"
            size="sm"
            icon={Trash2}
            onClick={() => handleDelete(consultation.id)}
            className="text-red-600 hover:text-red-700"
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
              <Eye className="w-6 h-6 text-secondary-600 dark:text-secondary-400" />
            </div>
            <div>
              <h1 className="text-3xl font-bold font-montserrat text-neutral-900 dark:text-neutral-100">
                Consultas Médicas
              </h1>
              <p className="text-neutral-600 dark:text-neutral-400">
                {consultations.length} consultas registradas
              </p>
            </div>
          </div>
          <Button 
            variant="primary" 
            icon={Plus}
            onClick={() => handleOpenModal()}
          >
            Nueva Consulta
          </Button>
        </div>

        {/* Quick Stats */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-green-100 dark:bg-green-900 rounded-lg">
                <Stethoscope className="w-5 h-5 text-green-600 dark:text-green-400" />
              </div>
              <div>
                <p className="text-xl font-bold text-neutral-900 dark:text-neutral-100">
                  {consultations.length}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Total Consultas
                </p>
              </div>
            </div>
          </GlassCard>
          
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-blue-100 dark:bg-blue-900 rounded-lg">
                <Calendar className="w-5 h-5 text-blue-600 dark:text-blue-400" />
              </div>
              <div>
                <p className="text-xl font-bold text-neutral-900 dark:text-neutral-100">
                  {consultations.filter(c => c.fechaSeguimiento || c.followUpDate).length}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Seguimientos Programados
                </p>
              </div>
            </div>
          </GlassCard>
          
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-orange-100 dark:bg-orange-900 rounded-lg">
                <FileText className="w-5 h-5 text-orange-600 dark:text-orange-400" />
              </div>
              <div>
                <p className="text-xl font-bold text-neutral-900 dark:text-neutral-100">
                  {consultations.filter(c => isConsultationToday(c)).length}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Consultas Hoy
                </p>
              </div>
            </div>
          </GlassCard>
        </div>

        {/* Filters */}
        <GlassCard className="p-6">
          <div className="flex flex-col sm:flex-row gap-4">
            <Input
              placeholder="Buscar por paciente o diagnóstico..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              icon={Search}
              className="flex-1"
            />
          </div>
        </GlassCard>

        {/* Consultations Table */}
        <GlassCard className="overflow-hidden">
          <div className="p-6 border-b border-neutral-200 dark:border-neutral-700">
            <div className="flex items-center justify-between">
              <div>
                <h2 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100">
                  Historial de Consultas
                </h2>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Mostrando {filteredConsultations.length} de {consultations.length} consultas
                </p>
              </div>
            </div>
          </div>
          <Table
            data={filteredConsultations}
            columns={columns}
            searchable={false}
          />
        </GlassCard>

        {/* Consultation Form Modal */}
        <Modal
          isOpen={isModalOpen}
          onClose={handleCloseModal}
          title={editingConsultation ? 'Editar Consulta' : 'Nueva Consulta'}
          size="xl"
        >
          <form onSubmit={handleSubmit} className="space-y-6">
            {/* Patient Selection */}
            <div>
              <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-300 mb-2">
                Paciente *
              </label>
              <select
                value={formData.patientId}
                onChange={(e) => setFormData(prev => ({ ...prev, patientId: e.target.value }))}
                required
                className="w-full px-4 py-2.5 border border-neutral-300 dark:border-neutral-600 rounded-lg bg-white/50 dark:bg-neutral-800/50 backdrop-blur-sm text-neutral-900 dark:text-neutral-100"
              >
                <option value="">Seleccionar paciente</option>
                {patients.map(patient => (
                  <option key={patient.id} value={patient.id}>{patient.nombre} {patient.apellido}</option>
                ))}
              </select>
            </div>

            {/* Symptoms */}
            <div>
              <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-300 mb-2">
                Síntomas *
              </label>
              <textarea
                value={formData.symptoms}
                onChange={(e) => setFormData(prev => ({ ...prev, symptoms: e.target.value }))}
                required
                rows={2}
                className="w-full px-4 py-2.5 border border-neutral-300 dark:border-neutral-600 rounded-lg bg-white/50 dark:bg-neutral-800/50 backdrop-blur-sm text-neutral-900 dark:text-neutral-100 placeholder-neutral-500 dark:placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                placeholder="Describir los síntomas del paciente..."
              />
            </div>

            {/* Visual Acuity */}
            <div>
              <h3 className="text-lg font-medium text-neutral-900 dark:text-neutral-100 mb-3">
                Agudeza Visual
              </h3>
              <div className="grid grid-cols-2 gap-4">
                <Input
                  label="Ojo Derecho (OD)"
                  placeholder="20/20"
                  value={formData.rightEyeVA}
                  onChange={(e) => setFormData(prev => ({ ...prev, rightEyeVA: e.target.value }))}
                  required
                />
                <Input
                  label="Ojo Izquierdo (OI)"
                  placeholder="20/20"
                  value={formData.leftEyeVA}
                  onChange={(e) => setFormData(prev => ({ ...prev, leftEyeVA: e.target.value }))}
                  required
                />
              </div>
            </div>

            {/* Refraction */}
            <div>
              <h3 className="text-lg font-medium text-neutral-900 dark:text-neutral-100 mb-3">
                Refracción
              </h3>
              <div className="space-y-4">
                <div>
                  <h4 className="text-sm font-medium text-neutral-700 dark:text-neutral-300 mb-2">
                    Ojo Derecho (OD)
                  </h4>
                  <div className="grid grid-cols-3 gap-4">
                    <div className="space-y-2">
                      <label className="block text-sm font-medium text-neutral-700">
                        Esfera
                      </label>
                      <input
                        type="number"
                        step="0.25"
                        value={formData.rightEyeSphere}
                        onChange={(e) => setFormData(prev => ({ ...prev, rightEyeSphere: e.target.value }))}
                        className="w-full px-3 py-2 border border-neutral-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-primary-500"
                        placeholder="0.00"
                      />
                    </div>
                    <div className="space-y-2">
                      <label className="block text-sm font-medium text-neutral-700">
                        Cilindro
                      </label>
                      <input
                        type="number"
                        step="0.25"
                        value={formData.rightEyeCylinder}
                        onChange={(e) => setFormData(prev => ({ ...prev, rightEyeCylinder: e.target.value }))}
                        className="w-full px-3 py-2 border border-neutral-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-primary-500"
                        placeholder="0.00"
                      />
                    </div>
                    <div className="space-y-2">
                      <label className="block text-sm font-medium text-neutral-700">
                        Eje
                      </label>
                      <input
                        type="number"
                        min="0"
                        max="180"
                        value={formData.rightEyeAxis}
                        onChange={(e) => setFormData(prev => ({ ...prev, rightEyeAxis: e.target.value }))}
                        className="w-full px-3 py-2 border border-neutral-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-primary-500"
                        placeholder="0"
                      />
                    </div>
                  </div>
                </div>
                <div>
                  <h4 className="text-sm font-medium text-neutral-700 dark:text-neutral-300 mb-2">
                    Ojo Izquierdo (OI)
                  </h4>
                  <div className="grid grid-cols-3 gap-4">
                    <div className="space-y-2">
                      <label className="block text-sm font-medium text-neutral-700">
                        Esfera
                      </label>
                      <input
                        type="number"
                        step="0.25"
                        value={formData.leftEyeSphere}
                        onChange={(e) => setFormData(prev => ({ ...prev, leftEyeSphere: e.target.value }))}
                        className="w-full px-3 py-2 border border-neutral-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-primary-500"
                        placeholder="0.00"
                      />
                    </div>
                    <div className="space-y-2">
                      <label className="block text-sm font-medium text-neutral-700">
                        Cilindro
                      </label>
                      <input
                        type="number"
                        step="0.25"
                        value={formData.leftEyeCylinder}
                        onChange={(e) => setFormData(prev => ({ ...prev, leftEyeCylinder: e.target.value }))}
                        className="w-full px-3 py-2 border border-neutral-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-primary-500"
                        placeholder="0.00"
                      />
                    </div>
                    <div className="space-y-2">
                      <label className="block text-sm font-medium text-neutral-700">
                        Eje
                      </label>
                      <input
                        type="number"
                        min="0"
                        max="180"
                        value={formData.leftEyeAxis}
                        onChange={(e) => setFormData(prev => ({ ...prev, leftEyeAxis: e.target.value }))}
                        className="w-full px-3 py-2 border border-neutral-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-primary-500"
                        placeholder="0"
                      />
                    </div>
                  </div>
                </div>
              </div>
            </div>

            {/* Clinical Information */}
            <div className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-300 mb-2">
                  Diagnóstico *
                </label>
                <textarea
                  value={formData.diagnosis}
                  onChange={(e) => setFormData(prev => ({ ...prev, diagnosis: e.target.value }))}
                  required
                  rows={3}
                  className="w-full px-4 py-2.5 border border-neutral-300 dark:border-neutral-600 rounded-lg bg-white/50 dark:bg-neutral-800/50 backdrop-blur-sm text-neutral-900 dark:text-neutral-100 placeholder-neutral-500 dark:placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                  placeholder="Describir el diagnóstico..."
                />
              </div>
              
              <div>
                <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-300 mb-2">
                  Tratamiento *
                </label>
                <textarea
                  value={formData.treatment}
                  onChange={(e) => setFormData(prev => ({ ...prev, treatment: e.target.value }))}
                  required
                  rows={3}
                  className="w-full px-4 py-2.5 border border-neutral-300 dark:border-neutral-600 rounded-lg bg-white/50 dark:bg-neutral-800/50 backdrop-blur-sm text-neutral-900 dark:text-neutral-100 placeholder-neutral-500 dark:placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                  placeholder="Describir el tratamiento..."
                />
              </div>
              
              <div>
                <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-300 mb-2">
                  Recomendaciones
                </label>
                <textarea
                  value={formData.recommendations}
                  onChange={(e) => setFormData(prev => ({ ...prev, recommendations: e.target.value }))}
                  rows={2}
                  className="w-full px-4 py-2.5 border border-neutral-300 dark:border-neutral-600 rounded-lg bg-white/50 dark:bg-neutral-800/50 backdrop-blur-sm text-neutral-900 dark:text-neutral-100 placeholder-neutral-500 dark:placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                  placeholder="Recomendaciones para el paciente..."
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-300 mb-2">
                  Observaciones
                </label>
                <textarea
                  value={formData.observations}
                  onChange={(e) => setFormData(prev => ({ ...prev, observations: e.target.value }))}
                  rows={2}
                  className="w-full px-4 py-2.5 border border-neutral-300 dark:border-neutral-600 rounded-lg bg-white/50 dark:bg-neutral-800/50 backdrop-blur-sm text-neutral-900 dark:text-neutral-100 placeholder-neutral-500 dark:placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                  placeholder="Observaciones adicionales..."
                />
              </div>
              
              <Input
                label="Fecha de Seguimiento"
                type="date"
                value={formData.followUpDate}
                onChange={(e) => setFormData(prev => ({ ...prev, followUpDate: e.target.value }))}
                icon={Calendar}
              />
            </div>
            
            <div className="flex justify-end space-x-3 pt-4">
              <Button variant="ghost" onClick={handleCloseModal}>
                Cancelar
              </Button>
              <Button variant="primary" type="submit">
                {editingConsultation ? 'Actualizar' : 'Crear'} Consulta
              </Button>
            </div>
          </form>
        </Modal>
      </div>
    </DashboardPageWrapper>
  );
}