import React, { useState, useEffect } from 'react';
import { DashboardPageWrapper } from '../components/layout/DashboardPageWrapper';
import { GlassCard } from '../components/ui/GlassCard';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';
import { Table } from '../components/ui/Table';
import { Modal } from '../components/ui/Modal';
import {
  Settings, Shield, Bell, Database, Activity, 
  Save, Download, Upload, Trash2, Eye, 
  AlertCircle, CheckCircle, Clock, Users,
  Mail, Server, HardDrive, Cpu, Monitor
} from 'lucide-react';
import { format } from 'date-fns';
import { es } from 'date-fns/locale';
import { systemConfigService, SystemConfig, BackupInfo, EmailTestRequest } from '../services/systemConfigService';
import { systemMonitoringService, SystemStats, SystemHealth, ActivityLog } from '../services/systemMonitoringService';
import { useAuth } from '../contexts/AuthContext';

export function AdminSystemPage() {
  const [activeTab, setActiveTab] = useState('config');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);
  
  // Configuración del sistema
  const [systemConfig, setSystemConfig] = useState<SystemConfig | null>(null);
  const [backups, setBackups] = useState<BackupInfo[]>([]);
  const [isConfigModalOpen, setIsConfigModalOpen] = useState(false);
  const [isEmailTestModalOpen, setIsEmailTestModalOpen] = useState(false);
  
  // Monitoreo del sistema
  const [systemStats, setSystemStats] = useState<SystemStats | null>(null);
  const [systemHealth, setSystemHealth] = useState<SystemHealth | null>(null);
  const [activities, setActivities] = useState<ActivityLog[]>([]);
  const [activityFilters, setActivityFilters] = useState({
    user: '',
    action: '',
    startDate: '',
    endDate: ''
  });

  // Email test
  const [emailTest, setEmailTest] = useState<EmailTestRequest>({
    to: '',
    subject: 'Prueba de correo - OptiCare',
    body: 'Este es un correo de prueba del sistema OptiCare.'
  });

  const { user } = useAuth();

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    if (activeTab === 'monitoring') {
      loadMonitoringData();
    }
  }, [activeTab]);

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);
      
      const [config, backupsList] = await Promise.all([
        systemConfigService.getSystemConfig(),
        systemConfigService.getBackups()
      ]);
      
      setSystemConfig(config);
      setBackups(backupsList);
    } catch (err) {
      setError('Error al cargar los datos del sistema');
      console.error('Error loading system data:', err);
    } finally {
      setLoading(false);
    }
  };

  const loadMonitoringData = async () => {
    try {
      const [stats, health, activitiesList] = await Promise.all([
        systemMonitoringService.getSystemStats(),
        systemMonitoringService.getSystemHealth(),
        systemMonitoringService.getActivities()
      ]);
      
      setSystemStats(stats);
      setSystemHealth(health);
      setActivities(activitiesList);
    } catch (err) {
      setError('Error al cargar datos de monitoreo');
      console.error('Error loading monitoring data:', err);
    }
  };

  const handleSaveConfig = async () => {
    if (!systemConfig) return;
    
    try {
      await systemConfigService.updateSystemConfig(systemConfig);
      setSuccess('Configuración guardada exitosamente');
      setIsConfigModalOpen(false);
    } catch (err) {
      setError('Error al guardar la configuración');
      console.error('Error saving config:', err);
    }
  };

  const handleCreateBackup = async () => {
    try {
      await systemConfigService.createBackup();
      setSuccess('Respaldo creado exitosamente');
      loadData(); // Recargar lista de respaldos
    } catch (err) {
      setError('Error al crear el respaldo');
      console.error('Error creating backup:', err);
    }
  };

  const handleTestEmail = async () => {
    try {
      await systemConfigService.testEmail(emailTest);
      setSuccess('Correo de prueba enviado exitosamente');
      setIsEmailTestModalOpen(false);
    } catch (err) {
      setError('Error al enviar correo de prueba');
      console.error('Error testing email:', err);
    }
  };

  const handleClearLogs = async () => {
    if (!confirm('¿Estás seguro de que quieres limpiar todos los logs?')) {
      return;
    }
    
    try {
      await systemMonitoringService.clearLogs();
      setSuccess('Logs limpiados exitosamente');
      loadMonitoringData();
    } catch (err) {
      setError('Error al limpiar los logs');
      console.error('Error clearing logs:', err);
    }
  };

  const formatBytes = (bytes: number) => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  };

  const formatUptime = (uptime: string) => {
    const seconds = parseInt(uptime);
    const days = Math.floor(seconds / 86400);
    const hours = Math.floor((seconds % 86400) / 3600);
    const minutes = Math.floor((seconds % 3600) / 60);
    
    if (days > 0) return `${days}d ${hours}h ${minutes}m`;
    if (hours > 0) return `${hours}h ${minutes}m`;
    return `${minutes}m`;
  };

  const tabs = [
    { id: 'config', label: 'Configuración', icon: Settings },
    { id: 'monitoring', label: 'Monitoreo', icon: Activity },
    { id: 'backup', label: 'Respaldo', icon: Database }
  ];

  const activityColumns = [
    {
      key: 'timestamp',
      label: 'Fecha/Hora',
      render: (_: any, activity: ActivityLog) => (
        <span className="text-sm text-neutral-600 dark:text-neutral-400">
          {format(new Date(activity.timestamp), 'dd/MM/yyyy HH:mm', { locale: es })}
        </span>
      )
    },
    {
      key: 'user',
      label: 'Usuario',
      render: (_: any, activity: ActivityLog) => (
        <div>
          <p className="font-medium text-neutral-900 dark:text-neutral-100">
            {activity.userEmail}
          </p>
          <p className="text-xs text-neutral-500">{activity.userRole}</p>
        </div>
      )
    },
    {
      key: 'action',
      label: 'Acción',
      render: (_: any, activity: ActivityLog) => (
        <span className="px-2 py-1 bg-blue-100 text-blue-800 dark:bg-blue-900/20 dark:text-blue-400 rounded-full text-xs font-medium">
          {activity.action}
        </span>
      )
    },
    {
      key: 'details',
      label: 'Detalles',
      render: (_: any, activity: ActivityLog) => (
        <span className="text-sm text-neutral-600 dark:text-neutral-400">
          {activity.details}
        </span>
      )
    },
    {
      key: 'status',
      label: 'Estado',
      render: (_: any, activity: ActivityLog) => (
        <span className={`px-2 py-1 rounded-full text-xs font-medium ${
          activity.success 
            ? 'bg-green-100 text-green-800 dark:bg-green-900/20 dark:text-green-400'
            : 'bg-red-100 text-red-800 dark:bg-red-900/20 dark:text-red-400'
        }`}>
          {activity.success ? 'Exitoso' : 'Error'}
        </span>
      )
    }
  ];

  if (loading && !systemConfig) {
    return (
      <DashboardPageWrapper>
        <div className="flex items-center justify-center h-64">
          <div className="text-center">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-secondary-500 mx-auto mb-4"></div>
            <p className="text-neutral-600 dark:text-neutral-400">Cargando configuración del sistema...</p>
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
              <Settings className="w-6 h-6 text-secondary-600 dark:text-secondary-400" />
            </div>
            <div>
              <h1 className="text-3xl font-bold font-montserrat text-neutral-900 dark:text-neutral-100">
                Administración del Sistema
              </h1>
              <p className="text-neutral-600 dark:text-neutral-400">
                Configuración, monitoreo y respaldo del sistema
              </p>
            </div>
          </div>
        </div>

        {/* Messages */}
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

        {success && (
          <GlassCard className="p-4 border-green-200 dark:border-green-800">
            <div className="flex items-center space-x-2 text-green-600 dark:text-green-400">
              <CheckCircle className="w-5 h-5" />
              <span>{success}</span>
              <Button
                variant="ghost"
                size="sm"
                onClick={() => setSuccess(null)}
                className="ml-auto"
              >
                ×
              </Button>
            </div>
          </GlassCard>
        )}

        {/* Tabs */}
        <GlassCard className="p-6">
          <div className="flex space-x-1">
            {tabs.map((tab) => {
              const Icon = tab.icon;
              return (
                <Button
                  key={tab.id}
                  variant={activeTab === tab.id ? 'primary' : 'ghost'}
                  onClick={() => setActiveTab(tab.id)}
                  icon={Icon}
                  className="flex-1"
                >
                  {tab.label}
                </Button>
              );
            })}
          </div>
        </GlassCard>

        {/* Tab Content */}
        {activeTab === 'config' && systemConfig && (
          <div className="space-y-6">
            {/* Roles y Permisos */}
            <GlassCard className="p-6">
              <div className="flex items-center justify-between mb-4">
                <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100">
                  Roles y Permisos
                </h3>
                <Button variant="primary" icon={Save} onClick={() => setIsConfigModalOpen(true)}>
                  Editar Configuración
                </Button>
              </div>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {systemConfig.roles.map((role) => (
                  <div key={role.name} className="p-4 border border-neutral-200 dark:border-neutral-700 rounded-lg">
                    <h4 className="font-medium text-neutral-900 dark:text-neutral-100 mb-2">
                      {role.name}
                    </h4>
                    <div className="flex flex-wrap gap-1">
                      {role.permissions.map((permission) => (
                        <span
                          key={permission}
                          className="px-2 py-1 bg-blue-100 text-blue-800 dark:bg-blue-900/20 dark:text-blue-400 rounded-full text-xs"
                        >
                          {permission}
                        </span>
                      ))}
                    </div>
                  </div>
                ))}
              </div>
            </GlassCard>

            {/* Configuración de Notificaciones */}
            <GlassCard className="p-6">
              <div className="flex items-center justify-between mb-4">
                <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100">
                  Configuración de Notificaciones
                </h3>
                <Button variant="secondary" icon={Mail} onClick={() => setIsEmailTestModalOpen(true)}>
                  Probar Correo
                </Button>
              </div>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <h4 className="font-medium text-neutral-900 dark:text-neutral-100 mb-2">
                    Configuración SMTP
                  </h4>
                  <div className="space-y-2 text-sm text-neutral-600 dark:text-neutral-400">
                    <p>Servidor: {systemConfig.notifications.emailSettings.smtpServer}</p>
                    <p>Puerto: {systemConfig.notifications.emailSettings.smtpPort}</p>
                    <p>Remitente: {systemConfig.notifications.emailSettings.senderEmail}</p>
                  </div>
                </div>
                <div>
                  <h4 className="font-medium text-neutral-900 dark:text-neutral-100 mb-2">
                    Plantillas Disponibles
                  </h4>
                  <div className="space-y-1">
                    {systemConfig.notifications.templates.map((template) => (
                      <div key={template.name} className="text-sm text-neutral-600 dark:text-neutral-400">
                        {template.name}
                      </div>
                    ))}
                  </div>
                </div>
              </div>
            </GlassCard>
          </div>
        )}

        {activeTab === 'monitoring' && (
          <div className="space-y-6">
            {/* Estadísticas del Sistema */}
            {systemStats && (
              <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
                <GlassCard className="p-4">
                  <div className="flex items-center space-x-3">
                    <div className="p-2 bg-blue-100 dark:bg-blue-900 rounded-lg">
                      <Activity className="w-5 h-5 text-blue-600 dark:text-blue-400" />
                    </div>
                    <div>
                      <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                        {systemStats.totalActivities}
                      </p>
                      <p className="text-sm text-neutral-600 dark:text-neutral-400">
                        Total Actividades
                      </p>
                    </div>
                  </div>
                </GlassCard>
                
                <GlassCard className="p-4">
                  <div className="flex items-center space-x-3">
                    <div className="p-2 bg-green-100 dark:bg-green-900 rounded-lg">
                      <Clock className="w-5 h-5 text-green-600 dark:text-green-400" />
                    </div>
                    <div>
                      <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                        {systemStats.activitiesLast24Hours}
                      </p>
                      <p className="text-sm text-neutral-600 dark:text-neutral-400">
                        Últimas 24h
                      </p>
                    </div>
                  </div>
                </GlassCard>
                
                <GlassCard className="p-4">
                  <div className="flex items-center space-x-3">
                    <div className="p-2 bg-yellow-100 dark:bg-yellow-900 rounded-lg">
                      <Users className="w-5 h-5 text-yellow-600 dark:text-yellow-400" />
                    </div>
                    <div>
                      <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                        {systemStats.topUsers.length}
                      </p>
                      <p className="text-sm text-neutral-600 dark:text-neutral-400">
                        Usuarios Activos
                      </p>
                    </div>
                  </div>
                </GlassCard>
                
                <GlassCard className="p-4">
                  <div className="flex items-center space-x-3">
                    <div className="p-2 bg-purple-100 dark:bg-purple-900 rounded-lg">
                      <Server className="w-5 h-5 text-purple-600 dark:text-purple-400" />
                    </div>
                    <div>
                      <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                        {systemStats.topActions.length}
                      </p>
                      <p className="text-sm text-neutral-600 dark:text-neutral-400">
                        Tipos de Acción
                      </p>
                    </div>
                  </div>
                </GlassCard>
              </div>
            )}

            {/* Estado del Sistema */}
            {systemHealth && (
              <GlassCard className="p-6">
                <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100 mb-4">
                  Estado del Sistema
                </h3>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                  <div className="p-4 border border-neutral-200 dark:border-neutral-700 rounded-lg">
                    <div className="flex items-center space-x-2 mb-2">
                      <Cpu className="w-4 h-4 text-neutral-400" />
                      <span className="text-sm font-medium">CPU</span>
                    </div>
                    <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                      {systemHealth.cpuUsage.toFixed(1)}%
                    </p>
                  </div>
                  
                  <div className="p-4 border border-neutral-200 dark:border-neutral-700 rounded-lg">
                    <div className="flex items-center space-x-2 mb-2">
                      <Monitor className="w-4 h-4 text-neutral-400" />
                      <span className="text-sm font-medium">Memoria</span>
                    </div>
                    <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                      {formatBytes(systemHealth.memoryUsage)}
                    </p>
                  </div>
                  
                  <div className="p-4 border border-neutral-200 dark:border-neutral-700 rounded-lg">
                    <div className="flex items-center space-x-2 mb-2">
                      <HardDrive className="w-4 h-4 text-neutral-400" />
                      <span className="text-sm font-medium">Disco</span>
                    </div>
                    <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                      {formatBytes(systemHealth.diskSpace.usedSpace)}
                    </p>
                    <p className="text-xs text-neutral-500">
                      de {formatBytes(systemHealth.diskSpace.totalSpace)}
                    </p>
                  </div>
                </div>
              </GlassCard>
            )}

            {/* Log de Actividades */}
            <GlassCard className="p-6">
              <div className="flex items-center justify-between mb-4">
                <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100">
                  Log de Actividades
                </h3>
                <Button variant="danger" icon={Trash2} onClick={handleClearLogs}>
                  Limpiar Logs
                </Button>
              </div>
              
              {/* Filtros */}
              <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-4">
                <Input
                  placeholder="Filtrar por usuario..."
                  value={activityFilters.user}
                  onChange={(e) => setActivityFilters(prev => ({ ...prev, user: e.target.value }))}
                />
                <Input
                  placeholder="Filtrar por acción..."
                  value={activityFilters.action}
                  onChange={(e) => setActivityFilters(prev => ({ ...prev, action: e.target.value }))}
                />
                <Input
                  type="date"
                  value={activityFilters.startDate}
                  onChange={(e) => setActivityFilters(prev => ({ ...prev, startDate: e.target.value }))}
                />
                <Input
                  type="date"
                  value={activityFilters.endDate}
                  onChange={(e) => setActivityFilters(prev => ({ ...prev, endDate: e.target.value }))}
                />
              </div>

              <Table
                data={activities}
                columns={activityColumns}
                searchable={false}
              />
            </GlassCard>
          </div>
        )}

        {activeTab === 'backup' && (
          <div className="space-y-6">
            {/* Configuración de Respaldo */}
            {systemConfig && (
              <GlassCard className="p-6">
                <div className="flex items-center justify-between mb-4">
                  <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100">
                    Configuración de Respaldo
                  </h3>
                  <Button variant="primary" icon={Download} onClick={handleCreateBackup}>
                    Crear Respaldo
                  </Button>
                </div>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <h4 className="font-medium text-neutral-900 dark:text-neutral-100 mb-2">
                      Configuración Actual
                    </h4>
                    <div className="space-y-2 text-sm text-neutral-600 dark:text-neutral-400">
                      <p>Respaldo Automático: {systemConfig.backup.autoBackup ? 'Activado' : 'Desactivado'}</p>
                      <p>Frecuencia: {systemConfig.backup.backupFrequency}</p>
                      <p>Retención: {systemConfig.backup.retentionDays} días</p>
                      <p>Ruta: {systemConfig.backup.backupPath}</p>
                    </div>
                  </div>
                </div>
              </GlassCard>
            )}

            {/* Lista de Respaldos */}
            <GlassCard className="p-6">
              <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100 mb-4">
                Respaldos Disponibles
              </h3>
              <div className="space-y-2">
                {backups.length === 0 ? (
                  <p className="text-neutral-600 dark:text-neutral-400">No hay respaldos disponibles</p>
                ) : (
                  backups.map((backup) => (
                    <div
                      key={backup.name}
                      className="flex items-center justify-between p-4 border border-neutral-200 dark:border-neutral-700 rounded-lg"
                    >
                      <div>
                        <p className="font-medium text-neutral-900 dark:text-neutral-100">
                          {backup.name}
                        </p>
                        <p className="text-sm text-neutral-600 dark:text-neutral-400">
                          Creado: {format(new Date(backup.created), 'dd/MM/yyyy HH:mm', { locale: es })}
                        </p>
                        <p className="text-xs text-neutral-500">
                          Tamaño: {formatBytes(backup.size)}
                        </p>
                      </div>
                      <div className="flex space-x-2">
                        <Button variant="ghost" size="sm" icon={Download}>
                          Descargar
                        </Button>
                        <Button variant="ghost" size="sm" icon={Trash2}>
                          Eliminar
                        </Button>
                      </div>
                    </div>
                  ))
                )}
              </div>
            </GlassCard>
          </div>
        )}

        {/* Modal de Configuración */}
        <Modal
          isOpen={isConfigModalOpen}
          onClose={() => setIsConfigModalOpen(false)}
          title="Editar Configuración del Sistema"
          size="xl"
        >
          {systemConfig && (
            <div className="space-y-6">
              <div>
                <h4 className="font-medium text-neutral-900 dark:text-neutral-100 mb-3">
                  Configuración de Correo
                </h4>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <Input
                    label="Servidor SMTP"
                    value={systemConfig.notifications.emailSettings.smtpServer}
                    onChange={(e) => setSystemConfig(prev => prev ? {
                      ...prev,
                      notifications: {
                        ...prev.notifications,
                        emailSettings: {
                          ...prev.notifications.emailSettings,
                          smtpServer: e.target.value
                        }
                      }
                    } : null)}
                  />
                  <Input
                    label="Puerto SMTP"
                    type="number"
                    value={systemConfig.notifications.emailSettings.smtpPort.toString()}
                    onChange={(e) => setSystemConfig(prev => prev ? {
                      ...prev,
                      notifications: {
                        ...prev.notifications,
                        emailSettings: {
                          ...prev.notifications.emailSettings,
                          smtpPort: parseInt(e.target.value) || 587
                        }
                      }
                    } : null)}
                  />
                  <Input
                    label="Correo Remitente"
                    value={systemConfig.notifications.emailSettings.senderEmail}
                    onChange={(e) => setSystemConfig(prev => prev ? {
                      ...prev,
                      notifications: {
                        ...prev.notifications,
                        emailSettings: {
                          ...prev.notifications.emailSettings,
                          senderEmail: e.target.value
                        }
                      }
                    } : null)}
                  />
                  <Input
                    label="Nombre Remitente"
                    value={systemConfig.notifications.emailSettings.senderName}
                    onChange={(e) => setSystemConfig(prev => prev ? {
                      ...prev,
                      notifications: {
                        ...prev.notifications,
                        emailSettings: {
                          ...prev.notifications.emailSettings,
                          senderName: e.target.value
                        }
                      }
                    } : null)}
                  />
                </div>
              </div>

              <div>
                <h4 className="font-medium text-neutral-900 dark:text-neutral-100 mb-3">
                  Configuración de Respaldo
                </h4>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="flex items-center space-x-2">
                      <input
                        type="checkbox"
                        checked={systemConfig.backup.autoBackup}
                        onChange={(e) => setSystemConfig(prev => prev ? {
                          ...prev,
                          backup: {
                            ...prev.backup,
                            autoBackup: e.target.checked
                          }
                        } : null)}
                        className="rounded border-neutral-300"
                      />
                      <span className="text-sm text-neutral-700 dark:text-neutral-300">
                        Respaldo Automático
                      </span>
                    </label>
                  </div>
                  <Input
                    label="Frecuencia"
                    value={systemConfig.backup.backupFrequency}
                    onChange={(e) => setSystemConfig(prev => prev ? {
                      ...prev,
                      backup: {
                        ...prev.backup,
                        backupFrequency: e.target.value
                      }
                    } : null)}
                  />
                  <Input
                    label="Días de Retención"
                    type="number"
                    value={systemConfig.backup.retentionDays.toString()}
                    onChange={(e) => setSystemConfig(prev => prev ? {
                      ...prev,
                      backup: {
                        ...prev.backup,
                        retentionDays: parseInt(e.target.value) || 30
                      }
                    } : null)}
                  />
                  <Input
                    label="Ruta de Respaldo"
                    value={systemConfig.backup.backupPath}
                    onChange={(e) => setSystemConfig(prev => prev ? {
                      ...prev,
                      backup: {
                        ...prev.backup,
                        backupPath: e.target.value
                      }
                    } : null)}
                  />
                </div>
              </div>

              <div className="flex justify-end space-x-3 pt-4">
                <Button variant="ghost" onClick={() => setIsConfigModalOpen(false)}>
                  Cancelar
                </Button>
                <Button variant="primary" onClick={handleSaveConfig}>
                  Guardar Configuración
                </Button>
              </div>
            </div>
          )}
        </Modal>

        {/* Modal de Prueba de Correo */}
        <Modal
          isOpen={isEmailTestModalOpen}
          onClose={() => setIsEmailTestModalOpen(false)}
          title="Probar Configuración de Correo"
          size="lg"
        >
          <div className="space-y-4">
            <Input
              label="Correo Destinatario"
              type="email"
              value={emailTest.to}
              onChange={(e) => setEmailTest(prev => ({ ...prev, to: e.target.value }))}
              required
            />
            <Input
              label="Asunto"
              value={emailTest.subject}
              onChange={(e) => setEmailTest(prev => ({ ...prev, subject: e.target.value }))}
              required
            />
            <div>
              <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-300 mb-2">
                Contenido
              </label>
              <textarea
                value={emailTest.body}
                onChange={(e) => setEmailTest(prev => ({ ...prev, body: e.target.value }))}
                className="w-full px-3 py-2 border border-neutral-300 dark:border-neutral-600 rounded-lg bg-white dark:bg-neutral-800 text-neutral-900 dark:text-neutral-100"
                rows={4}
                required
              />
            </div>
            
            <div className="flex justify-end space-x-3 pt-4">
              <Button variant="ghost" onClick={() => setIsEmailTestModalOpen(false)}>
                Cancelar
              </Button>
              <Button variant="primary" onClick={handleTestEmail}>
                Enviar Correo de Prueba
              </Button>
            </div>
          </div>
        </Modal>
      </div>
    </DashboardPageWrapper>
  );
} 