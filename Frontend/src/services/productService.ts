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

interface ProductoInventarioResponse {
  id: string;
  nombre: string;
  tipo: string;
  marca: string;
  modelo: string;
  precio: number;
  stock: number;
  descripcion: string;
  especificaciones: Record<string, string>;
  fechaCreacion: string;
  fechaActualizacion: string;
  activo: boolean;
  type: string;
}

export const productService = {
  async getAll(): Promise<Product[]> {
    const response = await api.get<ProductoInventarioResponse[]>('/api/inventario');
    return response.data.map(product => ({
      id: product.id,
      name: product.nombre,
      type: product.tipo as Product['type'],
      brand: product.marca,
      model: product.modelo,
      price: product.precio,
      stock: product.stock,
      description: product.descripcion,
      specifications: product.especificaciones
    }));
  },

  async getById(id: string): Promise<Product> {
    const response = await api.get<ProductoInventarioResponse>(`/api/inventario/${id}`);
    const product = response.data;
    return {
      id: product.id,
      name: product.nombre,
      type: product.tipo as Product['type'],
      brand: product.marca,
      model: product.modelo,
      price: product.precio,
      stock: product.stock,
      description: product.descripcion,
      specifications: product.especificaciones
    };
  },

  async create(product: CreateProductRequest): Promise<Product> {
    const requestData = {
      nombre: product.name,
      tipo: product.type,
      marca: product.brand,
      modelo: product.model,
      precio: product.price,
      stock: product.stock,
      descripcion: product.description,
      especificaciones: product.specifications
    };
    
    const response = await api.post<ProductoInventarioResponse>('/api/inventario', requestData);
    const createdProduct = response.data;
    return {
      id: createdProduct.id,
      name: createdProduct.nombre,
      type: createdProduct.tipo as Product['type'],
      brand: createdProduct.marca,
      model: createdProduct.modelo,
      price: createdProduct.precio,
      stock: createdProduct.stock,
      description: createdProduct.descripcion,
      specifications: createdProduct.especificaciones
    };
  },

  async update(id: string, product: UpdateProductRequest): Promise<Product> {
    const requestData = {
      nombre: product.name,
      tipo: product.type,
      marca: product.brand,
      modelo: product.model,
      precio: product.price,
      stock: product.stock,
      descripcion: product.description,
      especificaciones: product.specifications
    };
    
    const response = await api.put<ProductoInventarioResponse>(`/api/inventario/${id}`, requestData);
    const updatedProduct = response.data;
    return {
      id: updatedProduct.id,
      name: updatedProduct.nombre,
      type: updatedProduct.tipo as Product['type'],
      brand: updatedProduct.marca,
      model: updatedProduct.modelo,
      price: updatedProduct.precio,
      stock: updatedProduct.stock,
      description: updatedProduct.descripcion,
      specifications: updatedProduct.especificaciones
    };
  },

  async delete(id: string): Promise<void> {
    await api.delete(`/api/inventario/${id}`);
  },

  async search(query: string): Promise<Product[]> {
    const response = await api.get<ProductoInventarioResponse[]>(`/api/inventario/search?q=${encodeURIComponent(query)}`);
    return response.data.map(product => ({
      id: product.id,
      name: product.nombre,
      type: product.tipo as Product['type'],
      brand: product.marca,
      model: product.modelo,
      price: product.precio,
      stock: product.stock,
      description: product.descripcion,
      specifications: product.especificaciones
    }));
  }
}; 