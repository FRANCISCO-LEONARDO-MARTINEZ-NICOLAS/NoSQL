import React, { useState, useEffect } from 'react';
import { DashboardLayout } from '../components/layout/DashboardLayout';
import { useAuth } from '../contexts/AuthContext';
import { MetricCard } from '../components/ui/MetricCard';
import { GlassCard } from '../components/ui/GlassCard';
import { Button } from '../components/ui/Button';
import { Table } from '../components/ui/Table';
import { dashboardService, DashboardMetrics, Appointment, Sale } from '../services/dashboardService';
import {
  Users, Calendar, DollarSign, Package, 
  Eye, TrendingUp, Clock, AlertCircle,
  Plus, Search, Filter
} from 'lucide-react';
import { format } from 'date-fns';
import { es } from 'date-fns/locale';

export function DashboardPage() {
  const { user } = useAuth();
  const [metrics, setMetrics] = useState<DashboardMetrics | null>(null);
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [sales, setSales] = useState<Sale[]>([]);
  const [loading, setLoading] = useState(true);
  const isAdmin = user?.rol === 'admin' || user?.rol === 'Administrador';

  useEffect(() => {
    const loadDashboardData = async () => {
      try {
        setLoading(true);
        const [metricsData, appointmentsData, salesData] = await Promise.all([
          dashboardService.getMetrics(),
          dashboardService.getTodayAppointments(),
          dashboardService.getRecentSales()
        ]);
        
        setMetrics(metricsData);
        setAppointments(appointmentsData);
        setSales(salesData);
      } catch (error) {
        console.error('Error loading dashboard data:', error);
        // Si hay error, usar datos mock como fallback
        setAppointments([
          {
            id: '1',
            time: '09:00',
            patientName: 'María García',
            type: 'Consulta',
            status: 'scheduled',
            duration: '30 min'
          },
          {
            id: '2',
            time: '10:30',
            patientName: 'Carlos López',
            type: 'Seguimiento',
            status: 'in-progress',
            duration: '20 min'
          },
          {
            id: '3',
            time: '11:00',
            patientName: 'Ana Martínez',
            type: 'Urgencia',
            status: 'scheduled',
            duration: '45 min'
          },
        ]);
        setSales([
          {
            id: '1',
            patient: 'Pedro Ruiz',
            product: 'Monturas Ray-Ban',
            amount: 150.00,
            status: 'Entregado',
            date: '2024-01-15'
          },
          {
            id: '2',
            patient: 'Laura Silva',
            product: 'Lentes de contacto',
            amount: 89.99,
            status: 'Preparando',
            date: '2024-01-15'
          },
        ]);
      } finally {
        setLoading(false);
      }
    };

    loadDashboardData();
  }, []);

  const adminMetrics = [
    {
      title: 'Total Pacientes',
      value: metrics?.totalPatients?.toString() || '0',
      icon: Users,
      trend: { value: 12, isPositive: true },
      color: 'primary' as const
    },
    {
      title: 'Citas Hoy',
      value: metrics?.todayAppointments?.toString() || '0',
      icon: Calendar,
      trend: { value: 8, isPositive: true },
      color: 'secondary' as const
    },
    {
      title: 'Ingresos Mensuales',
      value: `$${(metrics?.monthlyRevenue || 0).toLocaleString()}`,
      icon: DollarSign,
      trend: { value: 15, isPositive: true },
      color: 'accent' as const
    },
    {
      title: 'Optometristas Activos',
      value: metrics?.activeOptometrists?.toString() || '0',
      icon: Eye,
      color: 'neutral' as const
    }
  ];

  const optometristMetrics = [
    {
      title: 'Pacientes Hoy',
      value: metrics?.todayAppointments?.toString() || '0',
      icon: Users,
      color: 'secondary' as const
    },
    {
      title: 'Consultas Completadas',
      value: metrics?.completedConsultations?.toString() || '0',
      icon: Eye,
      trend: { value: 5, isPositive: true },
      color: 'primary' as const
    },
    {
      title: 'Ventas del Mes',
      value: `$${(metrics?.monthlyRevenue || 0).toLocaleString()}`,
      icon: DollarSign,
      trend: { value: 18, isPositive: true },
      color: 'accent' as const
    },
    {
      title: 'Productos Pendientes',
      value: metrics?.pendingOrders?.toString() || '0',
      icon: Package,
      color: 'neutral' as const
    }
  ];

  const currentMetrics = isAdmin ? adminMetrics : optometristMetrics;

  if (loading) {
    return (
      <DashboardLayout>
        <div className="flex items-center justify-center h-64">
          <div className="text-center">
            <div className="w-16 h-16 border-4 border-primary-200 border-t-primary-600 rounded-full animate-spin mx-auto mb-4"></div>
            <p className="text-neutral-600">Cargando datos del dashboard...</p>
          </div>
        </div>
      </DashboardLayout>
    );
  }

  const appointmentsColumns = [
    {
      key: 'time',
      label: 'Hora',
      width: 'w-20'
    },
    {
      key: 'patientName',
      label: 'Paciente'
    },
    {
      key: 'type',
      label: 'Tipo'
    },
    {
      key: 'status',
      label: 'Estado',
      render: (value: string) => (
        <span className={`px-2 py-1 rounded-full text-xs font-medium ${
          value === 'scheduled' ? 'bg-blue-100 text-blue-800' :
          value === 'in-progress' ? 'bg-yellow-100 text-yellow-800' :
          'bg-green-100 text-green-800'
        }`}>
          {value === 'scheduled' ? 'Programada' :
           value === 'in-progress' ? 'En curso' : 'Completada'}
        </span>
      )
    },
    {
      key: 'duration',
      label: 'Duración',
      width: 'w-24'
    }
  ];

  const salesColumns = [
    {
      key: 'patient',
      label: 'Paciente'
    },
    {
      key: 'product',
      label: 'Producto'
    },
    {
      key: 'amount',
      label: 'Importe',
      render: (value: number) => `$${value.toFixed(2)}`
    },
    {
      key: 'status',
      label: 'Estado',
      render: (value: string) => (
        <span className={`px-2 py-1 rounded-full text-xs font-medium ${
          value === 'Entregado' ? 'bg-green-100 text-green-800' :
          'bg-yellow-100 text-yellow-800'
        }`}>
          {value}
        </span>
      )
    }
  ];

  return (
    <DashboardLayout>
      <div className="space-y-8">
        {/* Header */}
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div>
            <h1 className="text-3xl font-bold font-montserrat text-neutral-900 dark:text-neutral-100">
              {isAdmin ? 'Panel de Administración' : 'Panel Clínico'}
            </h1>
            <p className="text-neutral-600 dark:text-neutral-400 mt-1">
              {format(new Date(), "EEEE, d 'de' MMMM 'de' yyyy", { locale: es })}
            </p>
          </div>
          <div className="flex items-center space-x-3">
            <Button variant="ghost" icon={Search} size="sm">
              Buscar
            </Button>
            <Button variant="ghost" icon={Filter} size="sm">
              Filtrar
            </Button>
            <Button variant="primary" icon={Plus} size="sm">
              Nueva Cita
            </Button>
          </div>
        </div>

        {/* Metrics Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          {currentMetrics.map((metric, index) => (
            <MetricCard
              key={index}
              title={metric.title}
              value={metric.value}
              icon={metric.icon}
              trend={metric.trend}
              color={metric.color}
            />
          ))}
        </div>

        {/* Main Content Grid */}
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Today's Appointments */}
          <div className="lg:col-span-2">
            <GlassCard className="p-6">
              <div className="flex items-center justify-between mb-6">
                <div className="flex items-center space-x-3">
                  <div className="p-2 bg-primary-100 dark:bg-primary-900 rounded-lg">
                    <Calendar className="w-5 h-5 text-primary-600 dark:text-primary-400" />
                  </div>
                  <div>
                    <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100">
                      Citas de Hoy
                    </h3>
                    <p className="text-sm text-neutral-600 dark:text-neutral-400">
                      {appointments.length} citas programadas
                    </p>
                  </div>
                </div>
                <Button variant="ghost" size="sm" icon={Plus}>
                  Añadir
                </Button>
              </div>
              <Table
                data={appointments}
                columns={appointmentsColumns}
              />
            </GlassCard>
          </div>

          {/* Quick Actions & Notifications */}
          <div className="space-y-6">
            {/* Quick Actions */}
            <GlassCard className="p-6">
              <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100 mb-4">
                Acciones Rápidas
              </h3>
              <div className="space-y-3">
                <Button variant="ghost" className="w-full justify-start" icon={Plus}>
                  Nuevo Paciente
                </Button>
                <Button variant="ghost" className="w-full justify-start" icon={Calendar}>
                  Programar Cita
                </Button>
                <Button variant="ghost" className="w-full justify-start" icon={Eye}>
                  Nueva Consulta
                </Button>
                <Button variant="ghost" className="w-full justify-start" icon={Package}>
                  Gestionar Inventario
                </Button>
              </div>
            </GlassCard>

            {/* Notifications */}
            <GlassCard className="p-6">
              <div className="flex items-center space-x-3 mb-4">
                <div className="p-2 bg-accent-100 dark:bg-accent-900 rounded-lg">
                  <AlertCircle className="w-5 h-5 text-accent-600 dark:text-accent-400" />
                </div>
                <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100">
                  Notificaciones
                </h3>
              </div>
              <div className="space-y-3">
                <div className="p-3 bg-yellow-50 dark:bg-yellow-900/20 border border-yellow-200 dark:border-yellow-800 rounded-lg">
                  <div className="flex items-center space-x-2">
                    <Clock className="w-4 h-4 text-yellow-600" />
                    <p className="text-sm text-yellow-800 dark:text-yellow-200">
                      Cita en 15 minutos: María García
                    </p>
                  </div>
                </div>
                <div className="p-3 bg-blue-50 dark:bg-blue-900/20 border border-blue-200 dark:border-blue-800 rounded-lg">
                  <div className="flex items-center space-x-2">
                    <Package className="w-4 h-4 text-blue-600" />
                    <p className="text-sm text-blue-800 dark:text-blue-200">
                      Monturas listas para entrega (3)
                    </p>
                  </div>
                </div>
                <div className="p-3 bg-green-50 dark:bg-green-900/20 border border-green-200 dark:border-green-800 rounded-lg">
                  <div className="flex items-center space-x-2">
                    <TrendingUp className="w-4 h-4 text-green-600" />
                    <p className="text-sm text-green-800 dark:text-green-200">
                      Ventas del mes: +15% vs anterior
                    </p>
                  </div>
                </div>
              </div>
            </GlassCard>
          </div>
        </div>

        {/* Recent Sales */}
        <GlassCard className="p-6">
          <div className="flex items-center justify-between mb-6">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-secondary-100 dark:bg-secondary-900 rounded-lg">
                <DollarSign className="w-5 h-5 text-secondary-600 dark:text-secondary-400" />
              </div>
              <div>
                <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100">
                  Ventas Recientes
                </h3>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Últimas transacciones
                </p>
              </div>
            </div>
            <Button variant="ghost" size="sm">
              Ver Todas
            </Button>
          </div>
          <Table
            data={sales}
            columns={salesColumns}
          />
        </GlassCard>
      </div>
    </DashboardLayout>
  );
}