import React, { useState } from 'react';
import { Navigate } from 'react-router-dom';
import { Eye, EyeOff, Mail, Lock, Stethoscope, Shield } from 'lucide-react';
import { useAuth } from '../contexts/AuthContext';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';
import { GlassCard } from '../components/ui/GlassCard';

export function LoginPage() {
  const { user, login, loading } = useAuth();
  const [formData, setFormData] = useState({
    correo: '',
    password: '',
    rol: 'admin' // Por defecto admin
  });
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState('');
  const [loginLoading, setLoginLoading] = useState(false);

  if (user) {
    return <Navigate to="/dashboard" replace />;
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoginLoading(true);

    try {
      await login(formData);
    } catch (err: any) {
      console.error('Login error:', err);
      if (err.response?.data?.message) {
        setError(err.response.data.message);
      } else {
        setError('Error de conexión. Intenta más tarde.');
      }
    } finally {
      setLoginLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setFormData(prev => ({
      ...prev,
      [e.target.name]: e.target.value
    }));
  };

  const fillDemoCredentials = (role: 'admin' | 'optometrist') => {
    const credentials = {
      admin: { correo: 'admin@opticare.com', password: '123456', rol: 'admin' },
      optometrist: { correo: 'panchoforo@gmail.com', password: '123456', rol: 'Optometrista' }
    };
    
    setFormData(credentials[role]);
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-primary-50 to-secondary-50 flex items-center justify-center">
        <div className="text-center">
          <div className="w-16 h-16 border-4 border-primary-200 border-t-primary-600 rounded-full animate-spin mx-auto mb-4"></div>
          <p className="text-neutral-600">Iniciando OptiCare...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-primary-50 via-neutral-50 to-secondary-50 relative overflow-hidden">
      {/* Background Pattern */}
      <div className="absolute inset-0 opacity-5">
        <div className="absolute top-20 left-20 w-72 h-72 bg-primary-400 rounded-full mix-blend-multiply filter blur-xl animate-pulse"></div>
        <div className="absolute top-40 right-20 w-64 h-64 bg-secondary-400 rounded-full mix-blend-multiply filter blur-xl animate-pulse delay-1000"></div>
        <div className="absolute bottom-20 left-1/2 w-80 h-80 bg-accent-400 rounded-full mix-blend-multiply filter blur-xl animate-pulse delay-2000"></div>
      </div>

      <div className="relative min-h-screen flex">
        {/* Left Side - Branding */}
        <div className="hidden lg:flex lg:w-1/2 items-center justify-center p-12">
          <div className="max-w-md text-center">
            <div className="w-24 h-24 bg-gradient-to-br from-primary-500 to-primary-600 rounded-3xl flex items-center justify-center mx-auto mb-8 shadow-glass">
              <Eye className="w-12 h-12 text-white" />
            </div>
            <h1 className="text-4xl font-bold font-montserrat text-neutral-900 mb-4">
              OptiCare
            </h1>
            <p className="text-xl text-neutral-600 mb-8">
              Sistema de Gestión Optométrica Profesional
            </p>
            <div className="space-y-4 text-left">
              <div className="flex items-center space-x-3">
                <div className="w-8 h-8 bg-secondary-100 rounded-lg flex items-center justify-center">
                  <Stethoscope className="w-4 h-4 text-secondary-600" />
                </div>
                <span className="text-neutral-700">Gestión integral de pacientes</span>
              </div>
              <div className="flex items-center space-x-3">
                <div className="w-8 h-8 bg-primary-100 rounded-lg flex items-center justify-center">
                  <Shield className="w-4 h-4 text-primary-600" />
                </div>
                <span className="text-neutral-700">Control administrativo completo</span>
              </div>
            </div>
          </div>
        </div>

        {/* Right Side - Login Form */}
        <div className="w-full lg:w-1/2 flex items-center justify-center p-6">
          <div className="w-full max-w-md">
            {/* Mobile Logo */}
            <div className="lg:hidden text-center mb-8">
              <div className="w-16 h-16 bg-gradient-to-br from-primary-500 to-primary-600 rounded-2xl flex items-center justify-center mx-auto mb-4 shadow-glass">
                <Eye className="w-8 h-8 text-white" />
              </div>
              <h1 className="text-2xl font-bold font-montserrat text-neutral-900">OptiCare</h1>
              <p className="text-neutral-600">Sistema de Gestión Optométrica</p>
            </div>

            <GlassCard className="p-8">
              <div className="text-center mb-8">
                <h2 className="text-2xl font-bold font-montserrat text-neutral-900 mb-2">
                  Iniciar Sesión
                </h2>
                <p className="text-neutral-600">
                  Accede a tu panel de gestión
                </p>
              </div>

              <form onSubmit={handleSubmit} className="space-y-6">
                <Input
                  label="Correo Electrónico"
                  type="email"
                  name="correo"
                  value={formData.correo}
                  onChange={handleChange}
                  placeholder="usuario@opticare.com"
                  icon={Mail}
                  required
                />

                <div className="space-y-2">
                  <Input
                    label="Contraseña"
                    type={showPassword ? 'text' : 'password'}
                    name="password"
                    value={formData.password}
                    onChange={handleChange}
                    placeholder="Ingresa tu contraseña"
                    icon={Lock}
                    required
                  />
                  <button
                    type="button"
                    onClick={() => setShowPassword(!showPassword)}
                    className="text-sm text-primary-600 hover:text-primary-700 flex items-center space-x-1"
                  >
                    {showPassword ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                    <span>{showPassword ? 'Ocultar' : 'Mostrar'} contraseña</span>
                  </button>
                </div>

                {/* Rol selector */}
                <div className="space-y-2">
                  <label className="block text-sm font-medium text-neutral-700">
                    Rol *
                  </label>
                  <select
                    name="rol"
                    value={formData.rol}
                    onChange={handleChange}
                    className="w-full px-3 py-2 border border-neutral-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-primary-500"
                    required
                  >
                    <option value="admin">Administrador</option>
                    <option value="Optometrista">Optometrista</option>
                  </select>
                </div>

                {error && (
                  <div className="p-3 bg-red-50 border border-red-200 rounded-lg">
                    <p className="text-sm text-red-600">{error}</p>
                  </div>
                )}

                <Button
                  type="submit"
                  variant="primary"
                  size="lg"
                  loading={loginLoading}
                  className="w-full"
                >
                  {loginLoading ? 'Iniciando sesión...' : 'Iniciar Sesión'}
                </Button>
              </form>

              {/* Demo Credentials */}
              <div className="mt-8 pt-6 border-t border-neutral-200">
                <p className="text-sm text-neutral-600 text-center mb-4">
                  Cuentas de demostración:
                </p>
                <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => fillDemoCredentials('admin')}
                    className="flex items-center space-x-2"
                  >
                    <Shield className="w-4 h-4" />
                    <span>Administrador</span>
                  </Button>
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => fillDemoCredentials('optometrist')}
                    className="flex items-center space-x-2"
                  >
                    <Stethoscope className="w-4 h-4" />
                    <span>Optometrista</span>
                  </Button>
                </div>
                <p className="text-xs text-neutral-500 text-center mt-3">
                  Usuario: admin@opticare.com / panchoforo@gmail.com
                  <br />
                  Contraseña: 123456
                </p>
              </div>
            </GlassCard>
          </div>
        </div>
      </div>
    </div>
  );
}