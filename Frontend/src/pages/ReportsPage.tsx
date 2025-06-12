import React, { useState } from 'react';
import { DashboardPageWrapper } from '../components/layout/DashboardPageWrapper';
import { GlassCard } from '../components/ui/GlassCard';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';
import {
  BarChart3, Download, Calendar, TrendingUp, 
  Users, DollarSign, Eye, Package, Filter
} from 'lucide-react';
import { format, subDays, subMonths } from 'date-fns';
import { es } from 'date-fns/locale';
import { useAuth } from '../contexts/AuthContext';

export function ReportsPage() {
  const [activeSection, setActiveSection] = useState('reports');
  const [dateRange, setDateRange] = useState({
    start: format(subMonths(new Date(), 1), 'yyyy-MM-dd'),
    end: format(new Date(), 'yyyy-MM-dd')
  });

  const reportTypes = [
    {
      id: 'financial',
      title: 'Reporte Financiero',
      description: 'Ingresos, gastos y rentabilidad',
      icon: DollarSign,
      color: 'bg-green-100 text-green-600 dark:bg-green-900 dark:text-green-400'
    },
    {
      id: 'patients',
      title: 'Reporte de Pacientes',
      description: 'Estadísticas de pacientes y consultas',
      icon: Users,
      color: 'bg-blue-100 text-blue-600 dark:bg-blue-900 dark:text-blue-400'
    },
    {
      id: 'consultations',
      title: 'Reporte de Consultas',
      description: 'Análisis de consultas y diagnósticos',
      icon: Eye,
      color: 'bg-purple-100 text-purple-600 dark:bg-purple-900 dark:text-purple-400'
    },
    {
      id: 'inventory',
      title: 'Reporte de Inventario',
      description: 'Stock, rotación y valoración',
      icon: Package,
      color: 'bg-orange-100 text-orange-600 dark:bg-orange-900 dark:text-orange-400'
    },
    {
      id: 'performance',
      title: 'Reporte de Rendimiento',
      description: 'KPIs y métricas de rendimiento',
      icon: TrendingUp,
      color: 'bg-red-100 text-red-600 dark:bg-red-900 dark:text-red-400'
    },
    {
      id: 'optometrists',
      title: 'Reporte de Optometristas',
      description: 'Productividad y carga de trabajo',
      icon: BarChart3,
      color: 'bg-indigo-100 text-indigo-600 dark:bg-indigo-900 dark:text-indigo-400'
    }
  ];

  const quickStats = [
    {
      title: 'Ingresos del Período',
      value: '$18,450',
      change: '+15%',
      positive: true,
      icon: DollarSign
    },
    {
      title: 'Nuevos Pacientes',
      value: '127',
      change: '+8%',
      positive: true,
      icon: Users
    },
    {
      title: 'Consultas Realizadas',
      value: '342',
      change: '+12%',
      positive: true,
      icon: Eye
    },
    {
      title: 'Productos Vendidos',
      value: '89',
      change: '-3%',
      positive: false,
      icon: Package
    }
  ];

  const handleGenerateReport = (reportType: string) => {
    // Simulate report generation
    alert(`Generando reporte: ${reportTypes.find(r => r.id === reportType)?.title}`);
  };

  return (
    <DashboardPageWrapper>
      <div className="space-y-6">
        {/* Header */}
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div className="flex items-center space-x-3">
            <div className="p-3 bg-primary-100 dark:bg-primary-900 rounded-xl">
              <BarChart3 className="w-6 h-6 text-primary-600 dark:text-primary-400" />
            </div>
            <div>
              <h1 className="text-3xl font-bold font-montserrat text-neutral-900 dark:text-neutral-100">
                Reportes y Análisis
              </h1>
              <p className="text-neutral-600 dark:text-neutral-400">
                Informes detallados y métricas de rendimiento
              </p>
            </div>
          </div>
        </div>

        {/* Date Range Filter */}
        <GlassCard className="p-6">
          <div className="flex flex-col sm:flex-row items-start sm:items-center gap-4">
            <div className="flex items-center space-x-2">
              <Calendar className="w-5 h-5 text-neutral-500" />
              <span className="font-medium text-neutral-900 dark:text-neutral-100">
                Período de Análisis:
              </span>
            </div>
            <div className="flex items-center space-x-3">
              <Input
                type="date"
                value={dateRange.start}
                onChange={(e) => setDateRange(prev => ({ ...prev, start: e.target.value }))}
                className="w-auto"
              />
              <span className="text-neutral-500">hasta</span>
              <Input
                type="date"
                value={dateRange.end}
                onChange={(e) => setDateRange(prev => ({ ...prev, end: e.target.value }))}
                className="w-auto"
              />
              <Button variant="ghost" icon={Filter}>
                Aplicar
              </Button>
            </div>
          </div>
        </GlassCard>

        {/* Quick Stats */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          {quickStats.map((stat, index) => (
            <GlassCard key={index} className="p-6">
              <div className="flex items-center justify-between">
                <div className="flex-1">
                  <p className="text-sm font-medium text-neutral-600 dark:text-neutral-400 mb-1">
                    {stat.title}
                  </p>
                  <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                    {stat.value}
                  </p>
                  <div className="flex items-center mt-2">
                    <span className={`text-sm font-medium ${
                      stat.positive ? 'text-green-600' : 'text-red-600'
                    }`}>
                      {stat.change}
                    </span>
                    <span className="text-xs text-neutral-500 ml-1">vs período anterior</span>
                  </div>
                </div>
                <div className="p-3 bg-neutral-100 dark:bg-neutral-800 rounded-xl">
                  <stat.icon className="w-6 h-6 text-neutral-600 dark:text-neutral-400" />
                </div>
              </div>
            </GlassCard>
          ))}
        </div>

        {/* Report Types Grid */}
        <div>
          <h2 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100 mb-4">
            Tipos de Reportes Disponibles
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {reportTypes.map((report) => (
              <GlassCard key={report.id} className="p-6 hover:shadow-glass-lg transition-all duration-300">
                <div className="flex items-start space-x-4">
                  <div className={`p-3 rounded-xl ${report.color}`}>
                    <report.icon className="w-6 h-6" />
                  </div>
                  <div className="flex-1">
                    <h3 className="text-lg font-semibold text-neutral-900 dark:text-neutral-100 mb-2">
                      {report.title}
                    </h3>
                    <p className="text-sm text-neutral-600 dark:text-neutral-400 mb-4">
                      {report.description}
                    </p>
                    <div className="flex space-x-2">
                      <Button
                        variant="primary"
                        size="sm"
                        icon={Download}
                        onClick={() => handleGenerateReport(report.id)}
                      >
                        Generar
                      </Button>
                      <Button
                        variant="ghost"
                        size="sm"
                      >
                        Vista Previa
                      </Button>
                    </div>
                  </div>
                </div>
              </GlassCard>
            ))}
          </div>
        </div>

        {/* Recent Reports */}
        <GlassCard className="p-6">
          <div className="flex items-center justify-between mb-6">
            <h2 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100">
              Reportes Recientes
            </h2>
            <Button variant="ghost" size="sm">
              Ver Todos
            </Button>
          </div>
          
          <div className="space-y-4">
            {[
              {
                name: 'Reporte Financiero - Enero 2024',
                type: 'Financiero',
                date: '2024-01-31',
                size: '2.4 MB',
                status: 'Completado'
              },
              {
                name: 'Análisis de Pacientes - Q4 2023',
                type: 'Pacientes',
                date: '2024-01-15',
                size: '1.8 MB',
                status: 'Completado'
              },
              {
                name: 'Inventario - Diciembre 2023',
                type: 'Inventario',
                date: '2024-01-05',
                size: '956 KB',
                status: 'Completado'
              }
            ].map((report, index) => (
              <div key={index} className="flex items-center justify-between p-4 bg-neutral-50 dark:bg-neutral-800 rounded-lg">
                <div className="flex items-center space-x-3">
                  <div className="p-2 bg-blue-100 dark:bg-blue-900 rounded-lg">
                    <BarChart3 className="w-4 h-4 text-blue-600 dark:text-blue-400" />
                  </div>
                  <div>
                    <p className="font-medium text-neutral-900 dark:text-neutral-100">
                      {report.name}
                    </p>
                    <div className="flex items-center space-x-4 text-sm text-neutral-500">
                      <span>{report.type}</span>
                      <span>•</span>
                      <span>{format(new Date(report.date), 'dd/MM/yyyy', { locale: es })}</span>
                      <span>•</span>
                      <span>{report.size}</span>
                    </div>
                  </div>
                </div>
                <div className="flex items-center space-x-2">
                  <span className="px-2 py-1 bg-green-100 text-green-800 dark:bg-green-900/20 dark:text-green-400 rounded-full text-xs font-medium">
                    {report.status}
                  </span>
                  <Button variant="ghost" size="sm" icon={Download}>
                    Descargar
                  </Button>
                </div>
              </div>
            ))}
          </div>
        </GlassCard>

        {/* Chart Placeholder */}
        <GlassCard className="p-6">
          <h2 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100 mb-6">
            Tendencias de Ingresos
          </h2>
          <div className="h-64 bg-gradient-to-r from-primary-50 to-secondary-50 dark:from-primary-900/20 dark:to-secondary-900/20 rounded-lg flex items-center justify-center">
            <div className="text-center">
              <BarChart3 className="w-16 h-16 text-neutral-400 mx-auto mb-4" />
              <p className="text-neutral-600 dark:text-neutral-400">
                Gráfico de tendencias de ingresos
              </p>
              <p className="text-sm text-neutral-500 mt-2">
                Integración con biblioteca de gráficos pendiente
              </p>
            </div>
          </div>
        </GlassCard>
      </div>
    </DashboardPageWrapper>
  );
}