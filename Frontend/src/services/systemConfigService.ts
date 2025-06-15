import api from '../config/api';

export interface SystemConfig {
  roles: RoleConfig[];
  notifications: NotificationConfig;
  backup: BackupConfig;
}

export interface RoleConfig {
  name: string;
  permissions: string[];
}

export interface NotificationConfig {
  emailSettings: EmailSettings;
  templates: EmailTemplate[];
}

export interface EmailSettings {
  smtpServer: string;
  smtpPort: number;
  senderEmail: string;
  senderName: string;
  username?: string;
  password?: string;
  useSsl: boolean;
}

export interface EmailTemplate {
  name: string;
  subject: string;
  body: string;
}

export interface BackupConfig {
  autoBackup: boolean;
  backupFrequency: string;
  retentionDays: number;
  backupPath: string;
}

export interface BackupInfo {
  name: string;
  path: string;
  created: string;
  size: number;
}

export interface EmailTestRequest {
  to: string;
  subject: string;
  body: string;
}

class SystemConfigService {
  private baseUrl = '/api/systemconfig';

  async getSystemConfig(): Promise<SystemConfig> {
    try {
      const response = await api.get<SystemConfig>(this.baseUrl);
      return response.data;
    } catch (error) {
      console.error('Error fetching system config:', error);
      throw new Error('Error al obtener la configuración del sistema');
    }
  }

  async updateSystemConfig(config: SystemConfig): Promise<void> {
    try {
      await api.put(this.baseUrl, config);
    } catch (error) {
      console.error('Error updating system config:', error);
      throw new Error('Error al actualizar la configuración del sistema');
    }
  }

  async createBackup(): Promise<{ message: string; path: string }> {
    try {
      const response = await api.post<{ message: string; path: string }>(`${this.baseUrl}/backup`);
      return response.data;
    } catch (error) {
      console.error('Error creating backup:', error);
      throw new Error('Error al crear el respaldo');
    }
  }

  async getBackups(): Promise<BackupInfo[]> {
    try {
      const response = await api.get<BackupInfo[]>(`${this.baseUrl}/backups`);
      return response.data;
    } catch (error) {
      console.error('Error fetching backups:', error);
      throw new Error('Error al obtener los respaldos');
    }
  }

  async testEmail(request: EmailTestRequest): Promise<{ message: string }> {
    try {
      const response = await api.post<{ message: string }>(`${this.baseUrl}/test-email`, request);
      return response.data;
    } catch (error) {
      console.error('Error testing email:', error);
      throw new Error('Error al enviar correo de prueba');
    }
  }
}

export const systemConfigService = new SystemConfigService(); 