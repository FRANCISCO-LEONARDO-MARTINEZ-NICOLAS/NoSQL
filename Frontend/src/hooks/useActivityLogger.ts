import { useCallback } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { systemMonitoringService } from '../services/systemMonitoringService';

export function useActivityLogger() {
  const { user } = useAuth();

  const logActivity = useCallback(async (
    action: string,
    details: string,
    module?: string,
    success: boolean = true
  ) => {
    if (!user) return;

    try {
      await systemMonitoringService.logActivity({
        userEmail: user.correo,
        userRole: user.rol || 'unknown',
        action,
        details,
        module,
        success,
        ipAddress: '127.0.0.1' // En producción, obtener la IP real
      });
    } catch (error) {
      console.error('Error logging activity:', error);
      // No lanzamos error para no interrumpir el flujo principal
    }
  }, [user]);

  const logLogin = useCallback((success: boolean = true) => {
    logActivity(
      'Login',
      success ? 'Inicio de sesión exitoso' : 'Intento de inicio de sesión fallido',
      'Authentication',
      success
    );
  }, [logActivity]);

  const logLogout = useCallback(() => {
    logActivity(
      'Logout',
      'Cierre de sesión',
      'Authentication',
      true
    );
  }, [logActivity]);

  const logCreate = useCallback((entity: string, details: string) => {
    logActivity(
      'Create',
      `Creación de ${entity}: ${details}`,
      entity,
      true
    );
  }, [logActivity]);

  const logUpdate = useCallback((entity: string, details: string) => {
    logActivity(
      'Update',
      `Actualización de ${entity}: ${details}`,
      entity,
      true
    );
  }, [logActivity]);

  const logDelete = useCallback((entity: string, details: string) => {
    logActivity(
      'Delete',
      `Eliminación de ${entity}: ${details}`,
      entity,
      true
    );
  }, [logActivity]);

  const logView = useCallback((entity: string, details: string) => {
    logActivity(
      'View',
      `Visualización de ${entity}: ${details}`,
      entity,
      true
    );
  }, [logActivity]);

  const logError = useCallback((action: string, error: string, module?: string) => {
    logActivity(
      action,
      `Error: ${error}`,
      module,
      false
    );
  }, [logActivity]);

  return {
    logActivity,
    logLogin,
    logLogout,
    logCreate,
    logUpdate,
    logDelete,
    logView,
    logError
  };
} 