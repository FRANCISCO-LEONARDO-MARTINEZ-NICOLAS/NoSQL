export interface User {
  id: string;
  nombre: string;
  correo: string;
  rol: 'admin' | 'optometrist' | 'Optometrista' | 'Administrador';
  avatar?: string;
  specialty?: string;
  phone?: string;
}

export interface Patient {
  id: string;
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
  historialClinico: any[];
}

export interface Appointment {
  id: string;
  patientId: string;
  patientName: string;
  optometristId: string;
  optometristName: string;
  date: string;
  time: string;
  type: 'consultation' | 'follow-up' | 'emergency';
  status: 'scheduled' | 'in-progress' | 'completed' | 'cancelled';
  notes?: string;
}

export interface Consultation {
  id: string;
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
  nombrePaciente?: string;
  apellidoPaciente?: string;
  nombreOptometrista?: string;
  apellidoOptometrista?: string;
  patientId?: string;
  optometristId?: string;
  date?: string;
  visualAcuity?: {
    rightEye: string;
    leftEye: string;
  };
  refraction?: {
    rightEye: {
      sphere: number;
      cylinder: number;
      axis: number;
    };
    leftEye: {
      sphere: number;
      cylinder: number;
      axis: number;
    };
  };
  diagnosis?: string;
  treatment?: string;
  recommendations?: string;
  followUpDate?: string;
}

export interface Product {
  id: string;
  name: string;
  type: 'frame' | 'lens' | 'contact-lens' | 'accessory';
  brand: string;
  model: string;
  price: number;
  stock: number;
  description: string;
  specifications: Record<string, string>;
}

export interface Sale {
  id: string;
  patientId: string;
  patientName: string;
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

export interface DashboardMetrics {
  totalPatients: number;
  todayAppointments: number;
  monthlyRevenue: number;
  pendingOrders: number;
  completedConsultations: number;
  activeOptometrists: number;
}