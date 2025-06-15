import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { authService, LoginRequest, RegisterRequest } from '../services/authService';
import { User } from '../types';
import { systemMonitoringService } from '../services/systemMonitoringService';

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  login: (credentials: LoginRequest) => Promise<void>;
  register: (userData: RegisterRequest) => Promise<void>;
  logout: () => void;
  loading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Verificar si hay un usuario autenticado al cargar la aplicación
    const currentUser = authService.getCurrentUser();
    if (currentUser && authService.isAuthenticated()) {
      setUser(currentUser);
    }
    setLoading(false);
  }, []);

  const logActivity = async (
    action: string,
    details: string,
    userEmail: string,
    userRole: any,
    success: boolean = true
  ) => {
    try {
      await systemMonitoringService.logActivity({
        userEmail,
        userRole,
        action,
        details,
        module: 'Authentication',
        success,
        ipAddress: '127.0.0.1'
      });
    } catch (error) {
      console.error('Error logging activity:', error);
      // No lanzamos error para no interrumpir el flujo principal
    }
  };

  const login = async (credentials: LoginRequest) => {
    try {
      const response = await authService.login(credentials);
      localStorage.setItem('token', response.token);
      
      // Transformar los datos del backend al formato del frontend
      const transformedUser: User = {
        id: response.user.id,
        nombre: response.user.nombre,
        correo: response.user.correo,
        rol: response.user.rol
      };
      
      localStorage.setItem('user', JSON.stringify(transformedUser));
      setUser(transformedUser);

      // Log successful login
      await logActivity(
        'Login',
        'Inicio de sesión exitoso',
        transformedUser.correo,
        transformedUser.rol,
        true
      );
    } catch (error) {
      console.error('Error during login:', error);
      
      // Log failed login attempt
      await logActivity(
        'Login',
        `Intento de inicio de sesión fallido para ${credentials.correo}`,
        credentials.correo,
        'unknown' as any,
        false
      );
      
      throw error;
    }
  };

  const register = async (userData: RegisterRequest) => {
    try {
      const response = await authService.register(userData);
      localStorage.setItem('token', response.token);
      
      // Transformar los datos del backend al formato del frontend
      const transformedUser: User = {
        id: response.user.id,
        nombre: response.user.nombre,
        correo: response.user.correo,
        rol: response.user.rol
      };
      
      localStorage.setItem('user', JSON.stringify(transformedUser));
      setUser(transformedUser);

      // Log successful registration
      await logActivity(
        'Register',
        'Registro de usuario exitoso',
        transformedUser.correo,
        transformedUser.rol,
        true
      );
    } catch (error) {
      console.error('Error during registration:', error);
      
      // Log failed registration
      await logActivity(
        'Register',
        `Intento de registro fallido para ${userData.correo}`,
        userData.correo,
        'unknown' as any,
        false
      );
      
      throw error;
    }
  };

  const logout = async () => {
    if (user) {
      // Log logout before clearing user data
      await logActivity(
        'Logout',
        'Cierre de sesión',
        user.correo,
        user.rol,
        true
      );
    }
    
    authService.logout();
    setUser(null);
  };

  const value: AuthContextType = {
    user,
    isAuthenticated: !!user,
    login,
    register,
    logout,
    loading
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}; 