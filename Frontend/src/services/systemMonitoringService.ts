import api from '../config/api';

export interface ActivityLog {
  id: string;
  userEmail: string;
  userRole: string;
  action: string;
  details: string;
  ipAddress: string;
  timestamp: string;
  module?: string;
  success: boolean;
}

export interface SystemStats {
  totalActivities: number;
  activitiesLast24Hours: number;
  activitiesLast7Days: number;
  activitiesLast30Days: number;
  topUsers: Array<{ user: string; count: number }>;
  topActions: Array<{ action: string; count: number }>;
  recentActivities: ActivityLog[];
}

export interface SystemHealth {
  status: string;
  timestamp: string;
  uptime: string;
  memoryUsage: number;
  cpuUsage: number;
  diskSpace: DiskInfo;
  databaseStatus: string;
  services: ServiceStatus[];
}

export interface ServiceStatus {
  name: string;
  status: string;
}

export interface DiskInfo {
  totalSpace: number;
  freeSpace: number;
  usedSpace: number;
}

export interface ActivityFilters {
  user?: string;
  action?: string;
  startDate?: string;
  endDate?: string;
}

class SystemMonitoringService {
  private baseUrl = '/api/systemmonitoring';

  async getActivities(filters?: ActivityFilters): Promise<ActivityLog[]> {
    try {
      const params = new URLSearchParams();
      if (filters?.user) params.append('user', filters.user);
      if (filters?.action) params.append('action', filters.action);
      if (filters?.startDate) params.append('startDate', filters.startDate);
      if (filters?.endDate) params.append('endDate', filters.endDate);

      const response = await api.get<ActivityLog[]>(`${this.baseUrl}/activities?${params.toString()}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching activities:', error);
      throw new Error('Error al obtener las actividades');
    }
  }

  async getSystemStats(): Promise<SystemStats> {
    try {
      const response = await api.get<SystemStats>(`${this.baseUrl}/stats`);
      return response.data;
    } catch (error) {
      console.error('Error fetching system stats:', error);
      throw new Error('Error al obtener las estadísticas del sistema');
    }
  }

  async getSystemHealth(): Promise<SystemHealth> {
    try {
      const response = await api.get<SystemHealth>(`${this.baseUrl}/health`);
      return response.data;
    } catch (error) {
      console.error('Error fetching system health:', error);
      throw new Error('Error al obtener el estado del sistema');
    }
  }

  async logActivity(activity: Omit<ActivityLog, 'id' | 'timestamp'>): Promise<void> {
    try {
      await api.post(`${this.baseUrl}/log`, activity);
    } catch (error) {
      console.error('Error logging activity:', error);
      // No lanzamos error aquí para no interrumpir el flujo principal
    }
  }

  async clearLogs(daysToKeep?: number): Promise<void> {
    try {
      const params = daysToKeep ? `?daysToKeep=${daysToKeep}` : '';
      await api.delete(`${this.baseUrl}/logs${params}`);
    } catch (error) {
      console.error('Error clearing logs:', error);
      throw new Error('Error al limpiar los logs');
    }
  }
}

export const systemMonitoringService = new SystemMonitoringService(); 