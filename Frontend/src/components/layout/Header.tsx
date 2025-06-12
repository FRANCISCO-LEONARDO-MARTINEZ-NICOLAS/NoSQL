import React from 'react';
import { Bell, Moon, Sun, User, LogOut, Settings } from 'lucide-react';
import { Button } from '../ui/Button';
import { useAuth } from '../../contexts/AuthContext';
import { useTheme } from '../../hooks/useTheme';

export function Header() {
  const { user, logout } = useAuth();
  const { isDark, toggleTheme } = useTheme();
  const [showUserMenu, setShowUserMenu] = React.useState(false);

  const handleLogout = () => {
    logout();
    setShowUserMenu(false);
  };

  return (
    <header className="sticky top-0 z-40 backdrop-blur-xl bg-white/80 dark:bg-neutral-900/80 border-b border-neutral-200/50 dark:border-neutral-700/50">
      <div className="px-6 py-4">
        <div className="flex items-center justify-between">
          {/* Logo */}
          <div className="flex items-center space-x-3">
            <div className="w-8 h-8 bg-gradient-to-br from-primary-500 to-primary-600 rounded-lg flex items-center justify-center">
              <span className="text-white font-bold text-sm">OC</span>
            </div>
            <div>
              <h1 className="text-xl font-bold font-montserrat text-neutral-900 dark:text-neutral-100">
                OptiCare
              </h1>
              <p className="text-xs text-neutral-500 dark:text-neutral-400">
                Sistema de Gestión Optométrica
              </p>
            </div>
          </div>

          {/* Actions */}
          <div className="flex items-center space-x-3">
            {/* Theme Toggle */}
            <Button
              variant="ghost"
              size="sm"
              icon={isDark ? Sun : Moon}
              onClick={toggleTheme}
              className="p-2"
            >
              {isDark ? 'Claro' : 'Oscuro'}
            </Button>

            {/* Notifications */}
            <Button
              variant="ghost"
              size="sm"
              icon={Bell}
              className="p-2 relative"
            >
              Notificaciones
              <span className="absolute -top-1 -right-1 w-3 h-3 bg-accent-400 rounded-full text-xs flex items-center justify-center text-white">
                3
              </span>
            </Button>

            {/* User Menu */}
            <div className="relative">
              <button
                onClick={() => setShowUserMenu(!showUserMenu)}
                className="flex items-center space-x-3 p-2 rounded-lg hover:bg-neutral-100 dark:hover:bg-neutral-800 transition-colors"
              >
                <img
                  src={user?.avatar || `https://ui-avatars.com/api/?name=${user?.nombre}&background=2A5CAA&color=fff`}
                  alt={user?.nombre}
                  className="w-8 h-8 rounded-full"
                />
                <div className="text-left">
                  <p className="text-sm font-medium text-neutral-900 dark:text-neutral-100">
                    {user?.nombre}
                  </p>
                  <p className="text-xs text-neutral-500 dark:text-neutral-400 capitalize">
                    {user?.rol === 'admin' || user?.rol === 'Administrador' ? 'Administrador' : 'Optometrista'}
                  </p>
                </div>
              </button>

              {/* Dropdown Menu */}
              {showUserMenu && (
                <div className="absolute right-0 mt-2 w-48 backdrop-blur-xl bg-white/95 dark:bg-neutral-800/95 border border-neutral-200 dark:border-neutral-700 rounded-lg shadow-glass py-2">
                  <button
                    onClick={() => setShowUserMenu(false)}
                    className="flex items-center space-x-2 w-full px-4 py-2 text-sm text-neutral-700 dark:text-neutral-300 hover:bg-neutral-100 dark:hover:bg-neutral-700"
                  >
                    <User className="w-4 h-4" />
                    <span>Perfil</span>
                  </button>
                  <button
                    onClick={() => setShowUserMenu(false)}
                    className="flex items-center space-x-2 w-full px-4 py-2 text-sm text-neutral-700 dark:text-neutral-300 hover:bg-neutral-100 dark:hover:bg-neutral-700"
                  >
                    <Settings className="w-4 h-4" />
                    <span>Configuración</span>
                  </button>
                  <hr className="my-2 border-neutral-200 dark:border-neutral-700" />
                  <button
                    onClick={handleLogout}
                    className="flex items-center space-x-2 w-full px-4 py-2 text-sm text-red-600 hover:bg-red-50 dark:hover:bg-red-900/20"
                  >
                    <LogOut className="w-4 h-4" />
                    <span>Cerrar Sesión</span>
                  </button>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </header>
  );
}