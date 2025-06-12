import api from '../config/api';
import { Sale } from '../types';

export interface CreateSaleRequest {
  patientId: string;
  optometristId: string;
  date: string;
  items: {
    productId: string;
    productName: string;
    quantity: number;
    unitPrice: number;
    total: number;
  }[];
  subtotal: number;
  tax: number;
  total: number;
  status: 'pending' | 'preparing' | 'ready' | 'delivered';
  paymentMethod: 'cash' | 'card' | 'transfer';
}

export interface UpdateSaleRequest {
  status?: 'pending' | 'preparing' | 'ready' | 'delivered';
  paymentMethod?: 'cash' | 'card' | 'transfer';
}

export const saleService = {
  async getAll(): Promise<Sale[]> {
    const response = await api.get<Sale[]>('/api/ventas');
    return response.data;
  },

  async getById(id: string): Promise<Sale> {
    const response = await api.get<Sale>(`/api/ventas/${id}`);
    return response.data;
  },

  async create(sale: CreateSaleRequest): Promise<Sale> {
    const response = await api.post<Sale>('/api/ventas', sale);
    return response.data;
  },

  async update(id: string, sale: UpdateSaleRequest): Promise<Sale> {
    const response = await api.put<Sale>(`/api/ventas/${id}/estado`, sale);
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await api.delete(`/api/ventas/${id}`);
  },
}; 