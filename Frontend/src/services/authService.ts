import api from '../config/api';

export interface LoginRequest {
  correo: string;
  password: string;
  rol: string;
}

export interface LoginResponse {
  token: string;
  user: {
    id: string;
    nombre: string;
    correo: string;
    rol: string;
  };
}

export interface RegisterRequest {
  nombre: string;
  correo: string;
  password: string;
  rol: string;
}

export const authService = {
  async login(credentials: LoginRequest): Promise<LoginResponse> {
    const response = await api.post<LoginResponse>('/api/auth/login', credentials);
    return response.data;
  },

  async register(userData: RegisterRequest): Promise<LoginResponse> {
    const response = await api.post<LoginResponse>('/api/auth/register', userData);
    return response.data;
  },

  async logout(): Promise<void> {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  },

  getCurrentUser() {
    const userStr = localStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
  },

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }
}; 