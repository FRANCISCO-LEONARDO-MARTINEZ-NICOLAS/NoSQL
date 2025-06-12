import api from '../config/api';
import { Product } from '../types';

export interface CreateProductRequest {
  name: string;
  type: 'frame' | 'lens' | 'contact-lens' | 'accessory';
  brand: string;
  model: string;
  price: number;
  stock: number;
  description: string;
  specifications: Record<string, string>;
}

export interface UpdateProductRequest {
  name?: string;
  type?: 'frame' | 'lens' | 'contact-lens' | 'accessory';
  brand?: string;
  model?: string;
  price?: number;
  stock?: number;
  description?: string;
  specifications?: Record<string, string>;
}

export const productService = {
  async getAll(): Promise<Product[]> {
    const response = await api.get<Product[]>('/api/productos');
    return response.data;
  },

  async getById(id: string): Promise<Product> {
    const response = await api.get<Product>(`/api/productos/${id}`);
    return response.data;
  },

  async create(product: CreateProductRequest): Promise<Product> {
    const response = await api.post<Product>('/api/productos', product);
    return response.data;
  },

  async update(id: string, product: UpdateProductRequest): Promise<Product> {
    const response = await api.put<Product>(`/api/productos/${id}`, product);
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await api.delete(`/api/productos/${id}`);
  },

  async search(query: string): Promise<Product[]> {
    const response = await api.get<Product[]>(`/api/productos/search?q=${encodeURIComponent(query)}`);
    return response.data;
  }
}; 