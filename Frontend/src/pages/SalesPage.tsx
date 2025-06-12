import React, { useState, useEffect } from 'react';
import { DashboardPageWrapper } from '../components/layout/DashboardPageWrapper';
import { GlassCard } from '../components/ui/GlassCard';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';
import { Table } from '../components/ui/Table';
import { Modal } from '../components/ui/Modal';
import {
  ShoppingCart, Plus, Search, Filter, Eye, 
  CreditCard, Banknote, Smartphone, Package,
  CheckCircle, Clock, Truck
} from 'lucide-react';
import { format } from 'date-fns';
import { es } from 'date-fns/locale';
import { Sale } from '../types';
import { useAuth } from '../contexts/AuthContext';
import { saleService } from '../services/saleService';

const mockSales: Sale[] = [
  {
    id: '1',
    patientId: '1',
    patientName: 'María García López',
    optometristId: '2',
    date: '2024-01-15',
    items: [
      {
        productId: '1',
        productName: 'Monturas Ray-Ban RB2140',
        quantity: 1,
        unitPrice: 150.00,
        total: 150.00
      },
      {
        productId: '2',
        productName: 'Lentes antirreflejo',
        quantity: 2,
        unitPrice: 45.00,
        total: 90.00
      }
    ],
    subtotal: 240.00,
    tax: 50.40,
    total: 290.40,
    status: 'delivered',
    paymentMethod: 'card'
  },
  {
    id: '2',
    patientId: '2',
    patientName: 'Carlos López Martín',
    optometristId: '2',
    date: '2024-01-14',
    items: [
      {
        productId: '3',
        productName: 'Lentes de contacto mensuales',
        quantity: 6,
        unitPrice: 25.00,
        total: 150.00
      }
    ],
    subtotal: 150.00,
    tax: 31.50,
    total: 181.50,
    status: 'preparing',
    paymentMethod: 'cash'
  }
];

export function SalesPage() {
  const [activeSection, setActiveSection] = useState('sales');
  const [sales, setSales] = useState<Sale[]>([]);
  const [searchQuery, setSearchQuery] = useState('');
  const [statusFilter, setStatusFilter] = useState('all');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [viewingSale, setViewingSale] = useState<Sale | null>(null);

  useEffect(() => {
    loadSales();
  }, []);

  const loadSales = async () => {
    try {
      const data = await saleService.getAll();
      setSales(data);
    } catch (error) {
      setSales([]);
    }
  };

  // Helper para saber si una venta es de hoy
  const isSaleToday = (sale: Sale) => (sale.date || '').slice(0, 10) === new Date().toISOString().slice(0, 10);

  const filteredSales = sales.filter(sale => {
    const matchesSearch = sale.patientName.toLowerCase().includes(searchQuery.toLowerCase()) ||
                         sale.id.includes(searchQuery);
    const matchesStatus = statusFilter === 'all' || sale.status === statusFilter;
    
    return matchesSearch && matchesStatus;
  });

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'pending':
        return <Clock className="w-4 h-4" />;
      case 'preparing':
        return <Package className="w-4 h-4" />;
      case 'ready':
        return <CheckCircle className="w-4 h-4" />;
      case 'delivered':
        return <Truck className="w-4 h-4" />;
      default:
        return <Clock className="w-4 h-4" />;
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'pending':
        return 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900/20 dark:text-yellow-400';
      case 'preparing':
        return 'bg-blue-100 text-blue-800 dark:bg-blue-900/20 dark:text-blue-400';
      case 'ready':
        return 'bg-green-100 text-green-800 dark:bg-green-900/20 dark:text-green-400';
      case 'delivered':
        return 'bg-gray-100 text-gray-800 dark:bg-gray-900/20 dark:text-gray-400';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'pending':
        return 'Pendiente';
      case 'preparing':
        return 'Preparando';
      case 'ready':
        return 'Listo';
      case 'delivered':
        return 'Entregado';
      default:
        return status;
    }
  };

  const getPaymentIcon = (method: string) => {
    switch (method) {
      case 'cash':
        return <Banknote className="w-4 h-4" />;
      case 'card':
        return <CreditCard className="w-4 h-4" />;
      case 'transfer':
        return <Smartphone className="w-4 h-4" />;
      default:
        return <CreditCard className="w-4 h-4" />;
    }
  };

  const getPaymentText = (method: string) => {
    switch (method) {
      case 'cash':
        return 'Efectivo';
      case 'card':
        return 'Tarjeta';
      case 'transfer':
        return 'Transferencia';
      default:
        return method;
    }
  };

  const handleViewSale = (sale: Sale) => {
    setViewingSale(sale);
    setIsModalOpen(true);
  };

  const handleStatusChange = (saleId: string, newStatus: string) => {
    setSales(prev => prev.map(sale => 
      sale.id === saleId ? { ...sale, status: newStatus as any } : sale
    ));
  };

  const totalRevenue = sales.reduce((sum, sale) => sum + sale.total, 0);
  const pendingOrders = sales.filter(s => s.status === 'pending' || s.status === 'preparing').length;
  const todaySales = sales.filter(isSaleToday).length;

  const columns = [
    {
      key: 'id',
      label: 'Pedido',
      render: (value: string) => (
        <span className="font-mono text-sm">#{value}</span>
      )
    },
    {
      key: 'date',
      label: 'Fecha',
      render: (value: string) => format(new Date(value), 'dd/MM/yyyy', { locale: es })
    },
    {
      key: 'patientName',
      label: 'Cliente',
      render: (value: string) => (
        <span className="font-medium">{value}</span>
      )
    },
    {
      key: 'items',
      label: 'Productos',
      render: (items: Sale['items']) => (
        <div>
          <span className="text-sm font-medium">{items.length} producto(s)</span>
          <p className="text-xs text-neutral-500">
            {items[0]?.productName}
            {items.length > 1 && ` +${items.length - 1} más`}
          </p>
        </div>
      )
    },
    {
      key: 'total',
      label: 'Total',
      render: (value: number) => (
        <span className="font-semibold">${value.toFixed(2)}</span>
      )
    },
    {
      key: 'paymentMethod',
      label: 'Pago',
      render: (value: string) => (
        <div className="flex items-center space-x-1">
          {getPaymentIcon(value)}
          <span className="text-sm">{getPaymentText(value)}</span>
        </div>
      )
    },
    {
      key: 'status',
      label: 'Estado',
      render: (value: string, sale: Sale) => (
        <div className="flex items-center space-x-2">
          <div className={`flex items-center space-x-1 px-2 py-1 rounded-full text-xs font-medium ${getStatusColor(value)}`}>
            {getStatusIcon(value)}
            <span>{getStatusText(value)}</span>
          </div>
          {value === 'preparing' && (
            <Button
              variant="ghost"
              size="sm"
              onClick={() => handleStatusChange(sale.id, 'ready')}
              className="p-1 hover:bg-green-50 text-green-600"
            >
              Marcar Listo
            </Button>
          )}
          {value === 'ready' && (
            <Button
              variant="ghost"
              size="sm"
              onClick={() => handleStatusChange(sale.id, 'delivered')}
              className="p-1 hover:bg-gray-50 text-gray-600"
            >
              Entregar
            </Button>
          )}
        </div>
      )
    },
    {
      key: 'actions',
      label: 'Acciones',
      render: (_: any, sale: Sale) => (
        <Button
          variant="ghost"
          size="sm"
          icon={Eye}
          onClick={() => handleViewSale(sale)}
          className="p-1 hover:bg-blue-50"
        />
      )
    }
  ];

  return (
    <DashboardPageWrapper>
      <div className="space-y-6">
        {/* Header */}
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div className="flex items-center space-x-3">
            <div className="p-3 bg-accent-100 dark:bg-accent-900 rounded-xl">
              <ShoppingCart className="w-6 h-6 text-accent-600 dark:text-accent-400" />
            </div>
            <div>
              <h1 className="text-3xl font-bold font-montserrat text-neutral-900 dark:text-neutral-100">
                Gestión de Ventas
              </h1>
              <p className="text-neutral-600 dark:text-neutral-400">
                {sales.length} ventas registradas
              </p>
            </div>
          </div>
          <Button variant="primary" icon={Plus}>
            Nueva Venta
          </Button>
        </div>

        {/* Stats Cards */}
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-green-100 dark:bg-green-900 rounded-lg">
                <ShoppingCart className="w-5 h-5 text-green-600 dark:text-green-400" />
              </div>
              <div>
                <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                  ${totalRevenue.toFixed(2)}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Ingresos Totales
                </p>
              </div>
            </div>
          </GlassCard>
          
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-blue-100 dark:bg-blue-900 rounded-lg">
                <Package className="w-5 h-5 text-blue-600 dark:text-blue-400" />
              </div>
              <div>
                <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                  {pendingOrders}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Pedidos Pendientes
                </p>
              </div>
            </div>
          </GlassCard>
          
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-yellow-100 dark:bg-yellow-900 rounded-lg">
                <Clock className="w-5 h-5 text-yellow-600 dark:text-yellow-400" />
              </div>
              <div>
                <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                  {todaySales}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Ventas Hoy
                </p>
              </div>
            </div>
          </GlassCard>
          
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-purple-100 dark:bg-purple-900 rounded-lg">
                <CheckCircle className="w-5 h-5 text-purple-600 dark:text-purple-400" />
              </div>
              <div>
                <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                  {sales.filter(s => s.status === 'delivered').length}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Entregados
                </p>
              </div>
            </div>
          </GlassCard>
        </div>

        {/* Filters */}
        <GlassCard className="p-6">
          <div className="flex flex-col sm:flex-row gap-4">
            <Input
              placeholder="Buscar por cliente o número de pedido..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              icon={Search}
              className="flex-1"
            />
            <select
              value={statusFilter}
              onChange={(e) => setStatusFilter(e.target.value)}
              className="px-4 py-2 border border-neutral-300 dark:border-neutral-600 rounded-lg bg-white/50 dark:bg-neutral-800/50 backdrop-blur-sm text-neutral-900 dark:text-neutral-100"
            >
              <option value="all">Todos los estados</option>
              <option value="pending">Pendientes</option>
              <option value="preparing">Preparando</option>
              <option value="ready">Listos</option>
              <option value="delivered">Entregados</option>
            </select>
            <Button variant="ghost" icon={Filter}>
              Más Filtros
            </Button>
          </div>
        </GlassCard>

        {/* Sales Table */}
        <GlassCard className="overflow-hidden">
          <div className="p-6 border-b border-neutral-200 dark:border-neutral-700">
            <div className="flex items-center justify-between">
              <div>
                <h2 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100">
                  Lista de Ventas
                </h2>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Mostrando {filteredSales.length} de {sales.length} ventas
                </p>
              </div>
            </div>
          </div>
          <Table
            data={filteredSales}
            columns={columns}
            searchable={false}
          />
        </GlassCard>

        {/* Sale Detail Modal */}
        <Modal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          title={`Detalle de Venta #${viewingSale?.id}`}
          size="lg"
        >
          {viewingSale && (
            <div className="space-y-6">
              {/* Sale Info */}
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-300">
                    Cliente
                  </label>
                  <p className="text-neutral-900 dark:text-neutral-100 font-medium">
                    {viewingSale.patientName}
                  </p>
                </div>
                <div>
                  <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-300">
                    Fecha
                  </label>
                  <p className="text-neutral-900 dark:text-neutral-100">
                    {format(new Date(viewingSale.date), 'dd/MM/yyyy', { locale: es })}
                  </p>
                </div>
                <div>
                  <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-300">
                    Estado
                  </label>
                  <div className={`inline-flex items-center space-x-1 px-2 py-1 rounded-full text-xs font-medium ${getStatusColor(viewingSale.status)}`}>
                    {getStatusIcon(viewingSale.status)}
                    <span>{getStatusText(viewingSale.status)}</span>
                  </div>
                </div>
                <div>
                  <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-300">
                    Método de Pago
                  </label>
                  <div className="flex items-center space-x-1">
                    {getPaymentIcon(viewingSale.paymentMethod)}
                    <span>{getPaymentText(viewingSale.paymentMethod)}</span>
                  </div>
                </div>
              </div>

              {/* Items */}
              <div>
                <h3 className="text-lg font-medium text-neutral-900 dark:text-neutral-100 mb-3">
                  Productos
                </h3>
                <div className="space-y-3">
                  {viewingSale.items.map((item, index) => (
                    <div key={index} className="flex justify-between items-center p-3 bg-neutral-50 dark:bg-neutral-800 rounded-lg">
                      <div>
                        <p className="font-medium text-neutral-900 dark:text-neutral-100">
                          {item.productName}
                        </p>
                        <p className="text-sm text-neutral-600 dark:text-neutral-400">
                          Cantidad: {item.quantity} × ${item.unitPrice.toFixed(2)}
                        </p>
                      </div>
                      <p className="font-semibold text-neutral-900 dark:text-neutral-100">
                        ${item.total.toFixed(2)}
                      </p>
                    </div>
                  ))}
                </div>
              </div>

              {/* Summary */}
              <div className="border-t border-neutral-200 dark:border-neutral-700 pt-4">
                <div className="space-y-2">
                  <div className="flex justify-between">
                    <span className="text-neutral-600 dark:text-neutral-400">Subtotal:</span>
                    <span className="text-neutral-900 dark:text-neutral-100">${viewingSale.subtotal.toFixed(2)}</span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-neutral-600 dark:text-neutral-400">IVA (21%):</span>
                    <span className="text-neutral-900 dark:text-neutral-100">${viewingSale.tax.toFixed(2)}</span>
                  </div>
                  <div className="flex justify-between text-lg font-semibold border-t border-neutral-200 dark:border-neutral-700 pt-2">
                    <span className="text-neutral-900 dark:text-neutral-100">Total:</span>
                    <span className="text-neutral-900 dark:text-neutral-100">${viewingSale.total.toFixed(2)}</span>
                  </div>
                </div>
              </div>

              {/* Actions */}
              <div className="flex justify-end space-x-3 pt-4">
                <Button variant="ghost" onClick={() => setIsModalOpen(false)}>
                  Cerrar
                </Button>
                <Button variant="primary">
                  Imprimir Recibo
                </Button>
              </div>
            </div>
          )}
        </Modal>
      </div>
    </DashboardPageWrapper>
  );
}