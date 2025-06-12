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
    const response = await api.get<Consultation[]>('/api/consultas');
    return response.data;
  },

  async getById(id: string): Promise<Consultation> {
    const response = await api.get<Consultation>(`/api/consultas/${id}`);
    return response.data;
  },

  async create(consultation: CreateConsultationRequest): Promise<Consultation> {
    const response = await api.post<Consultation>('/api/consultas', consultation);
    return response.data;
  },

  async update(id: string, consultation: UpdateConsultationRequest): Promise<Consultation> {
    const response = await api.put<Consultation>(`/api/consultas/${id}`, consultation);
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await api.delete(`/api/consultas/${id}`);
  },

  async getByPatient(patientId: string): Promise<Consultation[]> {
    const response = await api.get<Consultation[]>(`/api/consultas/patient/${patientId}`);
    return response.data;
  },

  async getByOptometrist(optometristId: string): Promise<Consultation[]> {
    const response = await api.get<Consultation[]>(`/api/consultas/optometrist/${optometristId}`);
    return response.data;
  },

  async getByDate(date: string): Promise<Consultation[]> {
    const response = await api.get<Consultation[]>(`/api/consultas/date/${date}`);
    return response.data;
  }
}; 