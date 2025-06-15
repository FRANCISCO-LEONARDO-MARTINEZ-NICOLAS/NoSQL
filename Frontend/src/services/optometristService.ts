import api from '../config/api';

export interface Optometrist {
  id: string;
  nombre: string;
  apellido: string;
  cedulaProfesional: string;
  especialidad: string;
  correo: string;
  celular: string;
  numeroEmergencia: string;
  telefono?: string;
  direccion?: string;
  fechaContratacion: string;
  activo: boolean;
  type: string;
  username?: string;
  passwordHash?: string;
  hasLoginCredentials?: boolean;
}

export interface CreateOptometristRequest {
  nombre: string;
  apellido: string;
  cedulaProfesional: string;
  especialidad: string;
  correo: string;
  celular: string;
  numeroEmergencia: string;
  telefono?: string;
  direccion?: string;
}

export interface UpdateOptometristRequest {
  nombre?: string;
  apellido?: string;
  cedulaProfesional?: string;
  especialidad?: string;
  correo?: string;
  celular?: string;
  numeroEmergencia?: string;
  telefono?: string;
  direccion?: string;
  activo?: boolean;
}

export interface CreateCredentialsRequest {
  username: string;
  password: string;
}

class OptometristService {
  private baseUrl = '/api/optometristas';

  async getAll(): Promise<Optometrist[]> {
    try {
      const response = await api.get<Optometrist[]>(this.baseUrl);
      return response.data;
    } catch (error) {
      console.error('Error fetching optometrists:', error);
      throw new Error('Error al obtener los optometristas');
    }
  }

  async getById(id: string): Promise<Optometrist> {
    try {
      const response = await api.get<Optometrist>(`${this.baseUrl}/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching optometrist:', error);
      throw new Error('Error al obtener el optometrista');
    }
  }

  async create(optometrist: CreateOptometristRequest): Promise<Optometrist> {
    try {
      const response = await api.post<Optometrist>(this.baseUrl, optometrist);
      return response.data;
    } catch (error) {
      console.error('Error creating optometrist:', error);
      throw new Error('Error al crear el optometrista');
    }
  }

  async createCredentials(id: string, credentials: CreateCredentialsRequest): Promise<void> {
    try {
      await api.post(`${this.baseUrl}/${id}/credentials`, credentials);
    } catch (error) {
      console.error('Error creating credentials:', error);
      throw new Error('Error al crear las credenciales');
    }
  }

  async update(id: string, optometrist: UpdateOptometristRequest): Promise<void> {
    try {
      await api.put(`${this.baseUrl}/${id}`, optometrist);
    } catch (error) {
      console.error('Error updating optometrist:', error);
      throw new Error('Error al actualizar el optometrista');
    }
  }

  async delete(id: string): Promise<void> {
    try {
      await api.delete(`${this.baseUrl}/${id}`);
    } catch (error) {
      console.error('Error deleting optometrist:', error);
      throw new Error('Error al eliminar el optometrista');
    }
  }

  async searchByEmail(email: string): Promise<Optometrist | null> {
    try {
      const allOptometrists = await this.getAll();
      return allOptometrists.find(o => o.correo === email) || null;
    } catch (error) {
      console.error('Error searching optometrist by email:', error);
      return null;
    }
  }

  async searchByName(name: string): Promise<Optometrist[]> {
    try {
      const allOptometrists = await this.getAll();
      const searchTerm = name.toLowerCase();
      return allOptometrists.filter(o => 
        o.nombre.toLowerCase().includes(searchTerm) ||
        o.apellido.toLowerCase().includes(searchTerm) ||
        `${o.nombre} ${o.apellido}`.toLowerCase().includes(searchTerm)
      );
    } catch (error) {
      console.error('Error searching optometrists by name:', error);
      return [];
    }
  }

  async searchBySpecialty(specialty: string): Promise<Optometrist[]> {
    try {
      const allOptometrists = await this.getAll();
      const searchTerm = specialty.toLowerCase();
      return allOptometrists.filter(o => 
        o.especialidad.toLowerCase().includes(searchTerm)
      );
    } catch (error) {
      console.error('Error searching optometrists by specialty:', error);
      return [];
    }
  }

  async getActiveOptometrists(): Promise<Optometrist[]> {
    try {
      const allOptometrists = await this.getAll();
      return allOptometrists.filter(o => o.activo);
    } catch (error) {
      console.error('Error fetching active optometrists:', error);
      return [];
    }
  }
}

export const optometristService = new OptometristService(); 