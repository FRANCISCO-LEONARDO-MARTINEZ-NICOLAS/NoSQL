import api from '../config/api';
import { Patient } from '../types';

export interface CreatePatientRequest {
  nombre: string;
  apellido: string;
  fechaNacimiento: string;
  genero: string;
  direccion: string;
  telefono: string;
  correo: string;
  dni?: string;
  ocupacion?: string;
  seguroMedico?: string;
}

export interface UpdatePatientRequest {
  nombre?: string;
  apellido?: string;
  fechaNacimiento?: string;
  genero?: string;
  direccion?: string;
  telefono?: string;
  correo?: string;
  dni?: string;
  ocupacion?: string;
  seguroMedico?: string;
}

export const patientService = {
  async getAll(): Promise<Patient[]> {
    const response = await api.get<Patient[]>('/api/pacientes');
    return response.data;
  },

  async getById(id: string): Promise<Patient> {
    const response = await api.get<Patient>(`/api/pacientes/${id}`);
    return response.data;
  },

  async create(patient: CreatePatientRequest): Promise<Patient> {
    const response = await api.post<Patient>('/api/pacientes', patient);
    return response.data;
  },

  async update(id: string, patient: UpdatePatientRequest): Promise<Patient> {
    const response = await api.put<Patient>(`/api/pacientes/${id}`, patient);
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await api.delete(`/api/pacientes/${id}`);
  },

  async search(query: string): Promise<Patient[]> {
    const response = await api.get<Patient[]>(`/api/pacientes/search?q=${encodeURIComponent(query)}`);
    return response.data;
  }
}; 