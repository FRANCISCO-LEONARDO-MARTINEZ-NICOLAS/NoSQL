import api from '../config/api';

export interface DashboardMetrics {
  totalPatients: number;
  todayAppointments: number;
  monthlyRevenue: number;
  pendingOrders: number;
  completedConsultations: number;
  activeOptometrists: number;
}

export interface Appointment {
  id: string;
  time: string;
  patientName: string;
  type: string;
  status: 'scheduled' | 'in-progress' | 'completed';
  duration: string;
}

export interface Sale {
  id: string;
  patient: string;
  product: string;
  amount: number;
  status: string;
  date: string;
}

export const dashboardService = {
  async getMetrics(): Promise<DashboardMetrics> {
    const response = await api.get<DashboardMetrics>('/api/dashboard/metrics');
    return response.data;
  },

  async getTodayAppointments(): Promise<Appointment[]> {
    const response = await api.get<Appointment[]>('/api/citas/today');
    return response.data;
  },

  async getRecentSales(): Promise<Sale[]> {
    const response = await api.get<Sale[]>('/api/ventas/recent');
    return response.data;
  }
}; 