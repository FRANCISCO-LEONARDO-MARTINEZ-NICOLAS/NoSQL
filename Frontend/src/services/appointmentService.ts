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
    console.log('Fetching all appointments...');
    const response = await api.get<Appointment[]>('/api/appointments');
    console.log('Appointments fetched:', response.data);
    return response.data;
  },

  async getById(id: string): Promise<Appointment> {
    console.log('Fetching appointment by id:', id);
    const response = await api.get<Appointment>(`/api/appointments/${id}`);
    console.log('Appointment fetched:', response.data);
    return response.data;
  },

  async create(appointment: CreateAppointmentRequest): Promise<Appointment> {
    console.log('Creating appointment:', appointment);
    const response = await api.post<Appointment>('/api/appointments', appointment);
    console.log('Appointment created:', response.data);
    return response.data;
  },

  async update(id: string, appointment: UpdateAppointmentRequest): Promise<Appointment> {
    console.log('Updating appointment:', id, appointment);
    const response = await api.put<Appointment>(`/api/appointments/${id}`, appointment);
    console.log('Appointment updated:', response.data);
    return response.data;
  },

  async delete(id: string): Promise<void> {
    console.log('Deleting appointment:', id);
    await api.delete(`/api/appointments/${id}`);
    console.log('Appointment deleted successfully');
  },

  async updateStatus(id: string, status: string): Promise<Appointment> {
    console.log('Updating appointment status:', id, 'to:', status);
    const response = await api.patch<Appointment>(`/api/appointments/${id}/status`, { status });
    console.log('Appointment status updated:', response.data);
    return response.data;
  },

  async getByDate(date: string): Promise<Appointment[]> {
    console.log('Fetching appointments by date:', date);
    const response = await api.get<Appointment[]>(`/api/appointments/date/${date}`);
    console.log('Date appointments fetched:', response.data);
    return response.data;
  },

  async getByPatient(patientId: string): Promise<Appointment[]> {
    console.log('Fetching appointments by patient:', patientId);
    const response = await api.get<Appointment[]>(`/api/appointments/patient/${patientId}`);
    console.log('Patient appointments fetched:', response.data);
    return response.data;
  }
}; 