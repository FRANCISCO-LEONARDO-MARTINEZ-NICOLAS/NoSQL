import React, { useState } from 'react';
import { DashboardPageWrapper } from '../components/layout/DashboardPageWrapper';
import { GlassCard } from '../components/ui/GlassCard';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';
import {
  Settings, User, Bell, Shield, Palette, 
  Globe, Database, Mail, Phone, MapPin,
  Save, Eye, EyeOff
} from 'lucide-react';
import { useAuth } from '../contexts/AuthContext';
import { useTheme } from '../hooks/useTheme';

export function SettingsPage() {
  const [activeSection, setActiveSection] = useState('settings');
  const [activeTab, setActiveTab] = useState('profile');
  const { user } = useAuth();
  const { isDark, toggleTheme } = useTheme();
  
  const [profileData, setProfileData] = useState({
    name: user?.name || '',
    correo: user?.correo || '',
    phone: user?.phone || '',
    specialty: user?.specialty || '',
    avatar: user?.avatar || ''
  });

  const [passwordData, setPasswordData] = useState({
    current: '',
    new: '',
    confirm: ''
  });

  const [showPasswords, setShowPasswords] = useState({
    current: false,
    new: false,
    confirm: false
  });

  const [clinicData, setClinicData] = useState({
    name: 'OptiCare Centro Médico',
    address: 'Calle Mayor 123, 28001 Madrid',
    phone: '+34 91 123 45 67',
    correo: 'info@opticare.com',
    website: 'www.opticare.com',
    taxId: 'B12345678'
  });

  const [notificationSettings, setNotificationSettings] = useState({
    correoNotifications: true,
    smsNotifications: false,
    appointmentReminders: true,
    inventoryAlerts: true,
    reportNotifications: true
  });

  const tabs = [
    { id: 'profile', label: 'Perfil Personal', icon: User },
    { id: 'clinic', label: 'Información Clínica', icon: MapPin },
    { id: 'notifications', label: 'Notificaciones', icon: Bell },
    { id: 'security', label: 'Seguridad', icon: Shield },
    { id: 'appearance', label: 'Apariencia', icon: Palette },
    { id: 'system', label: 'Sistema', icon: Database }
  ];

  const handleSaveProfile = () => {
    // Save profile logic
    alert('Perfil actualizado correctamente');
  };

  const handleChangePassword = () => {
    if (passwordData.new !== passwordData.confirm) {
      alert('Las contraseñas no coinciden');
      return;
    }
    // Change password logic
    alert('Contraseña cambiada correctamente');
    setPasswordData({ current: '', new: '', confirm: '' });
  };

  const handleSaveClinic = () => {
    // Save clinic data logic
    alert('Información de la clínica actualizada');
  };

  const handleSaveNotifications = () => {
    // Save notification settings logic
    alert('Configuración de notificaciones guardada');
  };

  const renderTabContent = () => {
    switch (activeTab) {
      case 'profile':
        return (
          <div className="space-y-6">
            <div>
              <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100 mb-4">
                Información Personal
              </h3>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <Input
                  label="Nombre Completo"
                  value={profileData.name}
                  onChange={(e) => setProfileData(prev => ({ ...prev, name: e.target.value }))}
                />
                <Input
                  label="correo"
                  type="correo"
                  value={profileData.correo}
                  onChange={(e) => setProfileData(prev => ({ ...prev, correo: e.target.value }))}
                  icon={Mail}
                />
                <Input
                  label="Teléfono"
                  value={profileData.phone}
                  onChange={(e) => setProfileData(prev => ({ ...prev, phone: e.target.value }))}
                  icon={Phone}
                />
                <Input
                  label="Especialidad"
                  value={profileData.specialty}
                  onChange={(e) => setProfileData(prev => ({ ...prev, specialty: e.target.value }))}
                />
              </div>
              <div className="mt-4">
                <Input
                  label="URL del Avatar"
                  value={profileData.avatar}
                  onChange={(e) => setProfileData(prev => ({ ...prev, avatar: e.target.value }))}
                  placeholder="https://ejemplo.com/avatar.jpg"
                />
              </div>
              <div className="mt-6">
                <Button variant="primary" onClick={handleSaveProfile} icon={Save}>
                  Guardar Cambios
                </Button>
              </div>
            </div>

            <hr className="border-neutral-200 dark:border-neutral-700" />

            <div>
              <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100 mb-4">
                Cambiar Contraseña
              </h3>
              <div className="space-y-4 max-w-md">
                <div className="relative">
                  <Input
                    label="Contraseña Actual"
                    type={showPasswords.current ? 'text' : 'password'}
                    value={passwordData.current}
                    onChange={(e) => setPasswordData(prev => ({ ...prev, current: e.target.value }))}
                    icon={Shield}
                  />
                  <button
                    type="button"
                    onClick={() => setShowPasswords(prev => ({ ...prev, current: !prev.current }))}
                    className="absolute right-3 top-8 text-neutral-400 hover:text-neutral-600"
                  >
                    {showPasswords.current ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                  </button>
                </div>
                <div className="relative">
                  <Input
                    label="Nueva Contraseña"
                    type={showPasswords.new ? 'text' : 'password'}
                    value={passwordData.new}
                    onChange={(e) => setPasswordData(prev => ({ ...prev, new: e.target.value }))}
                    icon={Shield}
                  />
                  <button
                    type="button"
                    onClick={() => setShowPasswords(prev => ({ ...prev, new: !prev.new }))}
                    className="absolute right-3 top-8 text-neutral-400 hover:text-neutral-600"
                  >
                    {showPasswords.new ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                  </button>
                </div>
                <div className="relative">
                  <Input
                    label="Confirmar Nueva Contraseña"
                    type={showPasswords.confirm ? 'text' : 'password'}
                    value={passwordData.confirm}
                    onChange={(e) => setPasswordData(prev => ({ ...prev, confirm: e.target.value }))}
                    icon={Shield}
                  />
                  <button
                    type="button"
                    onClick={() => setShowPasswords(prev => ({ ...prev, confirm: !prev.confirm }))}
                    className="absolute right-3 top-8 text-neutral-400 hover:text-neutral-600"
                  >
                    {showPasswords.confirm ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                  </button>
                </div>
                <Button variant="secondary" onClick={handleChangePassword}>
                  Cambiar Contraseña
                </Button>
              </div>
            </div>
          </div>
        );

      case 'clinic':
        return (
          <div className="space-y-6">
            <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100">
              Información de la Clínica
            </h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <Input
                label="Nombre de la Clínica"
                value={clinicData.name}
                onChange={(e) => setClinicData(prev => ({ ...prev, name: e.target.value }))}
              />
              <Input
                label="NIF/CIF"
                value={clinicData.taxId}
                onChange={(e) => setClinicData(prev => ({ ...prev, taxId: e.target.value }))}
              />
              <Input
                label="Teléfono"
                value={clinicData.phone}
                onChange={(e) => setClinicData(prev => ({ ...prev, phone: e.target.value }))}
                icon={Phone}
              />
              <Input
                label="correo"
                type="correo"
                value={clinicData.correo}
                onChange={(e) => setClinicData(prev => ({ ...prev, correo: e.target.value }))}
                icon={Mail}
              />
              <Input
                label="Sitio Web"
                value={clinicData.website}
                onChange={(e) => setClinicData(prev => ({ ...prev, website: e.target.value }))}
                icon={Globe}
              />
            </div>
            <Input
              label="Dirección"
              value={clinicData.address}
              onChange={(e) => setClinicData(prev => ({ ...prev, address: e.target.value }))}
              icon={MapPin}
            />
            <Button variant="primary" onClick={handleSaveClinic} icon={Save}>
              Guardar Información
            </Button>
          </div>
        );

      case 'notifications':
        return (
          <div className="space-y-6">
            <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100">
              Configuración de Notificaciones
            </h3>
            <div className="space-y-4">
              {Object.entries(notificationSettings).map(([key, value]) => (
                <div key={key} className="flex items-center justify-between p-4 bg-neutral-50 dark:bg-neutral-800 rounded-lg">
                  <div>
                    <p className="font-medium text-neutral-900 dark:text-neutral-100">
                      {key === 'correoNotifications' && 'Notificaciones por correo'}
                      {key === 'smsNotifications' && 'Notificaciones por SMS'}
                      {key === 'appointmentReminders' && 'Recordatorios de Citas'}
                      {key === 'inventoryAlerts' && 'Alertas de Inventario'}
                      {key === 'reportNotifications' && 'Notificaciones de Reportes'}
                    </p>
                    <p className="text-sm text-neutral-600 dark:text-neutral-400">
                      {key === 'correoNotifications' && 'Recibir notificaciones por correo electrónico'}
                      {key === 'smsNotifications' && 'Recibir notificaciones por mensaje de texto'}
                      {key === 'appointmentReminders' && 'Recordatorios automáticos de citas'}
                      {key === 'inventoryAlerts' && 'Alertas cuando el stock esté bajo'}
                      {key === 'reportNotifications' && 'Notificaciones cuando los reportes estén listos'}
                    </p>
                  </div>
                  <label className="relative inline-flex items-center cursor-pointer">
                    <input
                      type="checkbox"
                      checked={value}
                      onChange={(e) => setNotificationSettings(prev => ({ ...prev, [key]: e.target.checked }))}
                      className="sr-only peer"
                    />
                    <div className="w-11 h-6 bg-neutral-200 peer-focus:outline-none peer-focus:ring-4 peer-focus:ring-primary-300 dark:peer-focus:ring-primary-800 rounded-full peer dark:bg-neutral-700 peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-neutral-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-neutral-600 peer-checked:bg-primary-600"></div>
                  </label>
                </div>
              ))}
            </div>
            <Button variant="primary" onClick={handleSaveNotifications} icon={Save}>
              Guardar Configuración
            </Button>
          </div>
        );

      case 'appearance':
        return (
          <div className="space-y-6">
            <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100">
              Configuración de Apariencia
            </h3>
            <div className="space-y-4">
              <div className="flex items-center justify-between p-4 bg-neutral-50 dark:bg-neutral-800 rounded-lg">
                <div>
                  <p className="font-medium text-neutral-900 dark:text-neutral-100">
                    Modo Oscuro
                  </p>
                  <p className="text-sm text-neutral-600 dark:text-neutral-400">
                    Cambiar entre tema claro y oscuro
                  </p>
                </div>
                <Button
                  variant={isDark ? 'primary' : 'ghost'}
                  onClick={toggleTheme}
                >
                  {isDark ? 'Activado' : 'Desactivado'}
                </Button>
              </div>
            </div>
          </div>
        );

      case 'system':
        return (
          <div className="space-y-6">
            <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100">
              Información del Sistema
            </h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="p-4 bg-neutral-50 dark:bg-neutral-800 rounded-lg">
                <p className="font-medium text-neutral-900 dark:text-neutral-100">Versión</p>
                <p className="text-neutral-600 dark:text-neutral-400">OptiCare v1.0.0</p>
              </div>
              <div className="p-4 bg-neutral-50 dark:bg-neutral-800 rounded-lg">
                <p className="font-medium text-neutral-900 dark:text-neutral-100">Última Actualización</p>
                <p className="text-neutral-600 dark:text-neutral-400">15 de Enero, 2024</p>
              </div>
              <div className="p-4 bg-neutral-50 dark:bg-neutral-800 rounded-lg">
                <p className="font-medium text-neutral-900 dark:text-neutral-100">Base de Datos</p>
                <p className="text-neutral-600 dark:text-neutral-400">Conectada</p>
              </div>
              <div className="p-4 bg-neutral-50 dark:bg-neutral-800 rounded-lg">
                <p className="font-medium text-neutral-900 dark:text-neutral-100">Respaldo</p>
                <p className="text-neutral-600 dark:text-neutral-400">Último: Hoy 03:00</p>
              </div>
            </div>
          </div>
        );

      default:
        return null;
    }
  };

  return (
    <DashboardPageWrapper>
      <div className="space-y-6">
        {/* Header */}
        <div className="flex items-center space-x-3">
          <div className="p-3 bg-neutral-100 dark:bg-neutral-900 rounded-xl">
            <Settings className="w-6 h-6 text-neutral-600 dark:text-neutral-400" />
          </div>
          <div>
            <h1 className="text-3xl font-bold font-montserrat text-neutral-900 dark:text-neutral-100">
              Configuración
            </h1>
            <p className="text-neutral-600 dark:text-neutral-400">
              Gestiona tu perfil y preferencias del sistema
            </p>
          </div>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
          {/* Sidebar */}
          <div className="lg:col-span-1">
            <GlassCard className="p-4">
              <nav className="space-y-2">
                {tabs.map((tab) => (
                  <button
                    key={tab.id}
                    onClick={() => setActiveTab(tab.id)}
                    className={`
                      w-full flex items-center space-x-3 px-3 py-2 rounded-lg text-left transition-colors
                      ${activeTab === tab.id
                        ? 'bg-primary-100 dark:bg-primary-900/50 text-primary-700 dark:text-primary-300'
                        : 'text-neutral-600 dark:text-neutral-400 hover:bg-neutral-100 dark:hover:bg-neutral-800'
                      }
                    `}
                  >
                    <tab.icon className="w-4 h-4" />
                    <span className="text-sm font-medium">{tab.label}</span>
                  </button>
                ))}
              </nav>
            </GlassCard>
          </div>

          {/* Content */}
          <div className="lg:col-span-3">
            <GlassCard className="p-6">
              {renderTabContent()}
            </GlassCard>
          </div>
        </div>
      </div>
    </DashboardPageWrapper>
  );
}