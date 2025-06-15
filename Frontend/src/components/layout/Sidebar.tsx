import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { 
  Home, Users, Calendar, FileText, Package, 
  BarChart3, Settings, Stethoscope, ShoppingCart,
  UserCheck, Eye, Server
} from 'lucide-react';
import { useAuth } from '../../contexts/AuthContext';
import { GlassCard } from '../ui/GlassCard';

interface MenuItem {
  id: string;
  label: string;
  icon: any;
  role: 'admin' | 'optometrist' | 'both';
  path: string;
  badge?: number;
}

const menuItems: MenuItem[] = [
  { id: 'dashboard', label: 'Dashboard', icon: Home, role: 'both', path: '/dashboard' },
  { id: 'appointments', label: 'Citas', icon: Calendar, role: 'both', path: '/appointments', badge: 5 },
  { id: 'patients', label: 'Pacientes', icon: Users, role: 'both', path: '/patients' },
  { id: 'consultations', label: 'Consultas', icon: Eye, role: 'optometrist', path: '/consultations' },
  { id: 'sales', label: 'Ventas', icon: ShoppingCart, role: 'optometrist', path: '/sales' },
  { id: 'inventory', label: 'Inventario', icon: Package, role: 'both', path: '/inventory' },
  { id: 'optometrists', label: 'Optometristas', icon: UserCheck, role: 'admin', path: '/optometrists' },
  { id: 'reports', label: 'Reportes', icon: BarChart3, role: 'admin', path: '/reports' },
  { id: 'admin-system', label: 'Administración', icon: Server, role: 'admin', path: '/admin/system' },
  { id: 'settings', label: 'Configuración', icon: Settings, role: 'both', path: '/settings' },
];

interface SidebarProps {
  activeSection?: string;
  onSectionChange?: (section: string) => void;
}

export function Sidebar({ activeSection, onSectionChange }: SidebarProps) {
  const { user } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const filteredItems = menuItems.filter(item => {
    if (item.role === 'both') return true;
    
    // Mapear roles en español a inglés para la comparación
    const userRole = user?.rol;
    if (item.role === 'admin' && (userRole === 'admin' || userRole === 'Administrador')) return true;
    if (item.role === 'optometrist' && (userRole === 'optometrist' || userRole === 'Optometrista')) return true;
    
    return false;
  });

  const isAdmin = user?.rol === 'admin' || user?.rol === 'Administrador';

  const handleItemClick = (item: MenuItem) => {
    navigate(item.path);
    if (onSectionChange) {
      onSectionChange(item.id);
    }
  };

  const getActiveItem = () => {
    const currentPath = location.pathname;
    const activeItem = menuItems.find(item => item.path === currentPath);
    return activeItem?.id || 'dashboard';
  };

  return (
    <aside className="w-64 h-full bg-gradient-to-b from-neutral-50 to-neutral-100 dark:from-neutral-900 dark:to-neutral-800 border-r border-neutral-200 dark:border-neutral-700">
      <div className="p-6">
        {/* Role Badge */}
        <GlassCard className="p-4 mb-6">
          <div className="flex items-center space-x-3">
            <div className={`p-3 rounded-xl ${isAdmin ? 'bg-primary-100 dark:bg-primary-900' : 'bg-secondary-100 dark:bg-secondary-900'}`}>
              {isAdmin ? (
                <Settings className={`w-5 h-5 ${isAdmin ? 'text-primary-600 dark:text-primary-400' : 'text-secondary-600 dark:text-secondary-400'}`} />
              ) : (
                <Stethoscope className={`w-5 h-5 ${isAdmin ? 'text-primary-600 dark:text-primary-400' : 'text-secondary-600 dark:text-secondary-400'}`} />
              )}
            </div>
            <div>
              <p className="font-semibold text-neutral-900 dark:text-neutral-100">
                {isAdmin ? 'Panel Admin' : 'Panel Clínico'}
              </p>
              <p className="text-xs text-neutral-500 dark:text-neutral-400">
                {isAdmin ? 'Gestión completa' : 'Atención al paciente'}
              </p>
            </div>
          </div>
        </GlassCard>

        {/* Navigation Menu */}
        <nav className="space-y-2">
          {filteredItems.map((item) => {
            const Icon = item.icon;
            const isActive = getActiveItem() === item.id;
            
            return (
              <button
                key={item.id}
                onClick={() => handleItemClick(item)}
                className={`
                  w-full flex items-center space-x-3 px-4 py-3 rounded-xl 
                  text-left transition-all duration-200 group
                  ${isActive 
                    ? `${isAdmin ? 'bg-primary-100 dark:bg-primary-900/50 text-primary-700 dark:text-primary-300' : 'bg-secondary-100 dark:bg-secondary-900/50 text-secondary-700 dark:text-secondary-300'}` 
                    : 'text-neutral-600 dark:text-neutral-400 hover:bg-neutral-100 dark:hover:bg-neutral-800 hover:text-neutral-900 dark:hover:text-neutral-100'
                  }
                `}
              >
                <Icon className={`
                  w-5 h-5 flex-shrink-0 transition-transform duration-200 
                  ${isActive ? 'scale-110' : 'group-hover:scale-105'}
                `} />
                <span className="font-medium flex-1">{item.label}</span>
                {item.badge && (
                  <span className={`
                    px-2 py-1 text-xs font-medium rounded-full
                    ${isActive 
                      ? `${isAdmin ? 'bg-primary-200 text-primary-800' : 'bg-secondary-200 text-secondary-800'}` 
                      : 'bg-accent-100 text-accent-800 dark:bg-accent-900 dark:text-accent-200'
                    }
                  `}>
                    {item.badge}
                  </span>
                )}
              </button>
            );
          })}
        </nav>
      </div>
    </aside>
  );
}