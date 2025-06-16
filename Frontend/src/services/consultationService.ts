import api from '../config/api';
import { Consultation } from '../types';

export interface CreateConsultationRequest {
  pacienteId: string;
  optometristaId: string;
  pacientecorreo: string;
  optometristacorreo: string;
  fecha: string;
  motivo: string;
  sintomas: string;
  diagnostico: string;
  tratamiento: string;
  recomendaciones: string;
  observaciones?: string;
  // Datos adicionales para el frontend
  agudezaVisual?: {
    ojoDerecho: string;
    ojoIzquierdo: string;
  };
  refraccion?: {
    ojoDerecho: {
      esfera: number;
      cilindro: number;
      eje: number;
    };
    ojoIzquierdo: {
      esfera: number;
      cilindro: number;
      eje: number;
    };
  };
  fechaSeguimiento?: string;
}

export interface UpdateConsultationRequest {
  motivo?: string;
  sintomas?: string;
  diagnostico?: string;
  tratamiento?: string;
  recomendaciones?: string;
  observaciones?: string;
  agudezaVisual?: {
    ojoDerecho: string;
    ojoIzquierdo: string;
  };
  refraccion?: {
    ojoDerecho: {
      esfera: number;
      cilindro: number;
      eje: number;
    };
    ojoIzquierdo: {
      esfera: number;
      cilindro: number;
      eje: number;
    };
  };
  fechaSeguimiento?: string;
}

export const consultationService = {
  async getAll(): Promise<Consultation[]> {
    console.log('Fetching all consultations...');
    const response = await api.get<Consultation[]>('/api/consultas');
    console.log('Consultations fetched:', response.data);
    return response.data;
  },

  async getById(id: string): Promise<Consultation> {
    console.log('Fetching consultation by id:', id);
    const response = await api.get<Consultation>(`/api/consultas/${id}`);
    console.log('Consultation fetched:', response.data);
    return response.data;
  },

  async create(consultation: CreateConsultationRequest): Promise<Consultation> {
    console.log('Creating consultation:', consultation);
    const response = await api.post<Consultation>('/api/consultas', consultation);
    console.log('Consultation created:', response.data);
    return response.data;
  },

  async update(id: string, consultation: UpdateConsultationRequest): Promise<Consultation> {
    console.log('Updating consultation:', id, consultation);
    const response = await api.put<Consultation>(`/api/consultas/${id}`, consultation);
    console.log('Consultation updated:', response.data);
    return response.data;
  },

  async delete(id: string): Promise<void> {
    console.log('Deleting consultation:', id);
    await api.delete(`/api/consultas/${id}`);
    console.log('Consultation deleted successfully');
  },

  async getByPatient(patientId: string): Promise<Consultation[]> {
    console.log('Fetching consultations by patient:', patientId);
    const response = await api.get<Consultation[]>(`/api/consultas/patient/${patientId}`);
    console.log('Patient consultations fetched:', response.data);
    return response.data;
  },

  async getByOptometrist(optometristId: string): Promise<Consultation[]> {
    console.log('Fetching consultations by optometrist:', optometristId);
    const response = await api.get<Consultation[]>(`/api/consultas/optometrist/${optometristId}`);
    console.log('Optometrist consultations fetched:', response.data);
    return response.data;
  },

  async getByDate(date: string): Promise<Consultation[]> {
    console.log('Fetching consultations by date:', date);
    const response = await api.get<Consultation[]>(`/api/consultas/date/${date}`);
    console.log('Date consultations fetched:', response.data);
    return response.data;
  }
}; 