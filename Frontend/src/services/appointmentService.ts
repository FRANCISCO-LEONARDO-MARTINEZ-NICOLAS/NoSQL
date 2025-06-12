import api from '../config/api';
import { Appointment } from '../types';

export interface CreateAppointmentRequest {
  patientId: string;
  optometristId: string;
  date: string;
  time: string;
  type: 'consultation' | 'follow-up' | 'emergency';
  notes?: string;
}

export interface UpdateAppointmentRequest {
  date?: string;
  time?: string;
  type?: 'consultation' | 'follow-up' | 'emergency';
  status?: 'scheduled' | 'in-progress' | 'completed' | 'cancelled';
  notes?: string;
}

export const appointmentService = {
  async getAll(): Promise<Appointment[]> {
    const response = await api.get<Appointment[]>('/api/appointments');
    return response.data;
  },

  async getById(id: string): Promise<Appointment> {
    const response = await api.get<Appointment>(`/api/appointments/${id}`);
    return response.data;
  },

  async create(appointment: CreateAppointmentRequest): Promise<Appointment> {
    const response = await api.post<Appointment>('/api/appointments', appointment);
    return response.data;
  },

  async update(id: string, appointment: UpdateAppointmentRequest): Promise<Appointment> {
    const response = await api.put<Appointment>(`/api/appointments/${id}`, appointment);
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await api.delete(`/api/appointments/${id}`);
  },

  async updateStatus(id: string, status: string): Promise<Appointment> {
    const response = await api.patch<Appointment>(`/api/appointments/${id}/status`, { status });
    return response.data;
  },

  async getByDate(date: string): Promise<Appointment[]> {
    const response = await api.get<Appointment[]>(`/api/appointments/date/${date}`);
    return response.data;
  },

  async getByPatient(patientId: string): Promise<Appointment[]> {
    const response = await api.get<Appointment[]>(`/api/appointments/patient/${patientId}`);
    return response.data;
  }
}; 