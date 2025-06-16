import React, { useState, useEffect } from 'react';
import { DashboardPageWrapper } from '../components/layout/DashboardPageWrapper';
import { GlassCard } from '../components/ui/GlassCard';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';
import { Table } from '../components/ui/Table';
import { Modal } from '../components/ui/Modal';
import {
  Package, Plus, Search, Filter, Edit, Trash2,
  AlertTriangle, TrendingUp, Eye, Glasses, Shield
} from 'lucide-react';
import { Product } from '../types';
import { useAuth } from '../contexts/AuthContext';
import { productService, CreateProductRequest, UpdateProductRequest } from '../services/productService';

export function InventoryPage() {
  const [activeSection, setActiveSection] = useState('inventory');
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState('');
  const [typeFilter, setTypeFilter] = useState('all');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);
  const [formData, setFormData] = useState({
    name: '',
    type: 'frame' as Product['type'],
    brand: '',
    model: '',
    price: '',
    stock: '',
    description: '',
    // Campos específicos por tipo de producto
    material: '',
    color: '',
    size: '',
    prescription: '',
    coating: '',
    diameter: '',
    baseCurve: '',
    waterContent: '',
    replacementSchedule: '',
    category: ''
  });

  const { user } = useAuth();
  const isAdmin = user?.rol === 'admin' || user?.rol === 'Administrador';

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {
    try {
      setLoading(true);
      const data = await productService.getAll();
      setProducts(data);
    } catch (error) {
      console.error('Error loading products:', error);
      setProducts([]);
    } finally {
      setLoading(false);
    }
  };

  const filteredProducts = products.filter(product => {
    const matchesSearch = product.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
                         product.brand.toLowerCase().includes(searchQuery.toLowerCase()) ||
                         product.model.toLowerCase().includes(searchQuery.toLowerCase());
    const matchesType = typeFilter === 'all' || product.type === typeFilter;
    
    return matchesSearch && matchesType;
  });

  const handleOpenModal = (product?: Product) => {
    if (product) {
      setEditingProduct(product);
      setFormData({
        name: product.name,
        type: product.type,
        brand: product.brand,
        model: product.model,
        price: product.price.toString(),
        stock: product.stock.toString(),
        description: product.description,
        // Extraer datos de specifications
        material: product.specifications.material || '',
        color: product.specifications.color || '',
        size: product.specifications.size || '',
        prescription: product.specifications.prescription || '',
        coating: product.specifications.coating || '',
        diameter: product.specifications.diameter || '',
        baseCurve: product.specifications.baseCurve || '',
        waterContent: product.specifications.waterContent || '',
        replacementSchedule: product.specifications.replacementSchedule || '',
        category: product.specifications.category || ''
      });
    } else {
      setEditingProduct(null);
      setFormData({
        name: '',
        type: 'frame',
        brand: '',
        model: '',
        price: '',
        stock: '',
        description: '',
        material: '',
        color: '',
        size: '',
        prescription: '',
        coating: '',
        diameter: '',
        baseCurve: '',
        waterContent: '',
        replacementSchedule: '',
        category: ''
      });
    }
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setEditingProduct(null);
  };

  const buildSpecifications = () => {
    const specs: Record<string, string> = {};
    
    // Campos comunes
    if (formData.material) specs.material = formData.material;
    if (formData.color) specs.color = formData.color;
    if (formData.size) specs.size = formData.size;
    
    // Campos específicos por tipo
    switch (formData.type) {
      case 'frame':
        if (formData.category) specs.category = formData.category;
        break;
      case 'lens':
        if (formData.prescription) specs.prescription = formData.prescription;
        if (formData.coating) specs.coating = formData.coating;
        break;
      case 'contact-lens':
        if (formData.diameter) specs.diameter = formData.diameter;
        if (formData.baseCurve) specs.baseCurve = formData.baseCurve;
        if (formData.waterContent) specs.waterContent = formData.waterContent;
        if (formData.replacementSchedule) specs.replacementSchedule = formData.replacementSchedule;
        break;
      case 'accessory':
        if (formData.category) specs.category = formData.category;
        break;
    }
    
    return specs;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    try {
      const specifications = buildSpecifications();
      
      if (editingProduct) {
        // Actualizar producto existente
        const updateData: UpdateProductRequest = {
          name: formData.name,
          type: formData.type,
          brand: formData.brand,
          model: formData.model,
          price: parseFloat(formData.price),
          stock: parseInt(formData.stock),
          description: formData.description,
          specifications: specifications
        };

        await productService.update(editingProduct.id, updateData);
      } else {
        // Crear nuevo producto
        const createData: CreateProductRequest = {
          name: formData.name,
          type: formData.type,
          brand: formData.brand,
          model: formData.model,
          price: parseFloat(formData.price),
          stock: parseInt(formData.stock),
          description: formData.description,
          specifications: specifications
        };

        await productService.create(createData);
      }

      await loadProducts(); // Recargar datos
      handleCloseModal();
    } catch (error) {
      console.error('Error saving product:', error);
      alert('Error al guardar el producto. Por favor, verifica los datos ingresados.');
    }
  };

  const handleDelete = async (productId: string) => {
    if (!isAdmin) {
      alert('Solo los administradores pueden eliminar productos.');
      return;
    }
    
    if (confirm('¿Estás seguro de que quieres eliminar este producto?')) {
      try {
        await productService.delete(productId);
        await loadProducts(); // Recargar datos
      } catch (error) {
        console.error('Error deleting product:', error);
        alert('Error al eliminar el producto. Por favor, intenta de nuevo.');
      }
    }
  };

  const getTypeIcon = (type: string) => {
    switch (type) {
      case 'frame':
        return <Glasses className="w-4 h-4" />;
      case 'lens':
        return <Eye className="w-4 h-4" />;
      case 'contact-lens':
        return <Eye className="w-4 h-4" />;
      case 'accessory':
        return <Package className="w-4 h-4" />;
      default:
        return <Package className="w-4 h-4" />;
    }
  };

  const getTypeText = (type: string) => {
    switch (type) {
      case 'frame':
        return 'Montura';
      case 'lens':
        return 'Lente';
      case 'contact-lens':
        return 'Lentilla';
      case 'accessory':
        return 'Accesorio';
      default:
        return type;
    }
  };

  const getStockStatus = (stock: number) => {
    if (stock === 0) return { color: 'text-red-600', text: 'Agotado' };
    if (stock < 5) return { color: 'text-yellow-600', text: 'Bajo stock' };
    return { color: 'text-green-600', text: 'En stock' };
  };

  const lowStockCount = products.filter(p => p.stock < 5).length;
  const outOfStockCount = products.filter(p => p.stock === 0).length;
  const totalValue = products.reduce((sum, p) => sum + (p.price * p.stock), 0);

  const columns = [
    {
      key: 'name',
      label: 'Producto',
      render: (value: string, product: Product) => (
        <div>
          <div className="flex items-center space-x-2">
            {getTypeIcon(product.type)}
            <span className="font-medium text-neutral-900 dark:text-neutral-100">{value}</span>
          </div>
          <p className="text-sm text-neutral-500">{product.brand} {product.model}</p>
        </div>
      )
    },
    {
      key: 'type',
      label: 'Tipo',
      render: (value: string) => (
        <span className={`px-2 py-1 rounded-full text-xs font-medium ${
          value === 'frame' ? 'bg-blue-100 text-blue-800' :
          value === 'lens' ? 'bg-green-100 text-green-800' :
          value === 'contact-lens' ? 'bg-purple-100 text-purple-800' :
          'bg-gray-100 text-gray-800'
        }`}>
          {getTypeText(value)}
        </span>
      )
    },
    {
      key: 'price',
      label: 'Precio',
      render: (value: number) => (
        <span className="font-semibold">${value.toFixed(2)}</span>
      )
    },
    {
      key: 'stock',
      label: 'Stock',
      render: (value: number) => {
        const status = getStockStatus(value);
        return (
          <div className="flex items-center space-x-2">
            <span className="font-medium">{value}</span>
            <span className={`text-xs ${status.color}`}>
              {status.text}
            </span>
            {value < 5 && value > 0 && (
              <AlertTriangle className="w-4 h-4 text-yellow-500" />
            )}
            {value === 0 && (
              <AlertTriangle className="w-4 h-4 text-red-500" />
            )}
          </div>
        );
      }
    },
    {
      key: 'value',
      label: 'Valor Total',
      render: (_: any, product: Product) => (
        <span className="font-medium">${(product.price * product.stock).toFixed(2)}</span>
      )
    },
    {
      key: 'actions',
      label: 'Acciones',
      render: (_: any, product: Product) => (
        <div className="flex items-center space-x-2">
          <Button
            variant="ghost"
            size="sm"
            icon={Edit}
            onClick={() => handleOpenModal(product)}
            className="p-1 hover:bg-green-50"
          />
          {isAdmin && (
            <Button
              variant="ghost"
              size="sm"
              icon={Trash2}
              onClick={() => handleDelete(product.id)}
              className="p-1 hover:bg-red-50 text-red-600"
            />
          )}
        </div>
      )
    }
  ];

  const renderSpecificationFields = () => {
    switch (formData.type) {
      case 'frame':
        return (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <Input
              label="Material"
              value={formData.material}
              onChange={(e) => setFormData(prev => ({ ...prev, material: e.target.value }))}
              placeholder="Ej: Acetato, Metal, Titanio"
            />
            <Input
              label="Color"
              value={formData.color}
              onChange={(e) => setFormData(prev => ({ ...prev, color: e.target.value }))}
              placeholder="Ej: Negro, Marrón, Azul"
            />
            <Input
              label="Tamaño"
              value={formData.size}
              onChange={(e) => setFormData(prev => ({ ...prev, size: e.target.value }))}
              placeholder="Ej: 54mm, Grande, Mediano"
            />
            <Input
              label="Categoría"
              value={formData.category}
              onChange={(e) => setFormData(prev => ({ ...prev, category: e.target.value }))}
              placeholder="Ej: Masculino, Femenino, Unisex"
            />
          </div>
        );
      
      case 'lens':
        return (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <Input
              label="Material"
              value={formData.material}
              onChange={(e) => setFormData(prev => ({ ...prev, material: e.target.value }))}
              placeholder="Ej: Policarbonato, CR-39, Alto índice"
            />
            <Input
              label="Color"
              value={formData.color}
              onChange={(e) => setFormData(prev => ({ ...prev, color: e.target.value }))}
              placeholder="Ej: Transparente, Fotocromático, Polarizado"
            />
            <Input
              label="Receta"
              value={formData.prescription}
              onChange={(e) => setFormData(prev => ({ ...prev, prescription: e.target.value }))}
              placeholder="Ej: Monofocal, Bifocal, Progresivo"
            />
            <Input
              label="Tratamiento"
              value={formData.coating}
              onChange={(e) => setFormData(prev => ({ ...prev, coating: e.target.value }))}
              placeholder="Ej: Antirreflejo, Antirrayas, Blue Block"
            />
          </div>
        );
      
      case 'contact-lens':
        return (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <Input
              label="Material"
              value={formData.material}
              onChange={(e) => setFormData(prev => ({ ...prev, material: e.target.value }))}
              placeholder="Ej: Hidrogel, Silicona hidrogel"
            />
            <Input
              label="Diámetro"
              value={formData.diameter}
              onChange={(e) => setFormData(prev => ({ ...prev, diameter: e.target.value }))}
              placeholder="Ej: 14.0mm, 14.2mm"
            />
            <Input
              label="Curva Base"
              value={formData.baseCurve}
              onChange={(e) => setFormData(prev => ({ ...prev, baseCurve: e.target.value }))}
              placeholder="Ej: 8.6mm, 8.8mm"
            />
            <Input
              label="Contenido de Agua"
              value={formData.waterContent}
              onChange={(e) => setFormData(prev => ({ ...prev, waterContent: e.target.value }))}
              placeholder="Ej: 38%, 55%, 70%"
            />
            <Input
              label="Horario de Reemplazo"
              value={formData.replacementSchedule}
              onChange={(e) => setFormData(prev => ({ ...prev, replacementSchedule: e.target.value }))}
              placeholder="Ej: Diario, Quincenal, Mensual"
            />
          </div>
        );
      
      case 'accessory':
        return (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <Input
              label="Material"
              value={formData.material}
              onChange={(e) => setFormData(prev => ({ ...prev, material: e.target.value }))}
              placeholder="Ej: Microfibra, Cuero, Plástico"
            />
            <Input
              label="Color"
              value={formData.color}
              onChange={(e) => setFormData(prev => ({ ...prev, color: e.target.value }))}
              placeholder="Ej: Negro, Azul, Rojo"
            />
            <Input
              label="Tamaño"
              value={formData.size}
              onChange={(e) => setFormData(prev => ({ ...prev, size: e.target.value }))}
              placeholder="Ej: Pequeño, Mediano, Grande"
            />
            <Input
              label="Categoría"
              value={formData.category}
              onChange={(e) => setFormData(prev => ({ ...prev, category: e.target.value }))}
              placeholder="Ej: Estuche, Limpiador, Cordón"
            />
          </div>
        );
      
      default:
        return null;
    }
  };

  return (
    <DashboardPageWrapper>
      <div className="space-y-6">
        {/* Header */}
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div className="flex items-center space-x-3">
            <div className="p-3 bg-primary-100 dark:bg-primary-900 rounded-xl">
              <Package className="w-6 h-6 text-primary-600 dark:text-primary-400" />
            </div>
            <div>
              <h1 className="text-3xl font-bold font-montserrat text-neutral-900 dark:text-neutral-100">
                Gestión de Inventario
              </h1>
              <p className="text-neutral-600 dark:text-neutral-400">
                {products.length} productos registrados
              </p>
            </div>
          </div>
          {isAdmin ? (
            <Button 
              variant="primary" 
              icon={Plus}
              onClick={() => handleOpenModal()}
            >
              Nuevo Producto
            </Button>
          ) : (
            <div className="flex items-center space-x-2 text-neutral-500">
              <Shield className="w-5 h-5" />
              <span className="text-sm">Solo administradores pueden agregar productos</span>
            </div>
          )}
        </div>

        {/* Stats Cards */}
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-green-100 dark:bg-green-900 rounded-lg">
                <Package className="w-5 h-5 text-green-600 dark:text-green-400" />
              </div>
              <div>
                <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                  {products.length}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Total Productos
                </p>
              </div>
            </div>
          </GlassCard>
          
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-blue-100 dark:bg-blue-900 rounded-lg">
                <TrendingUp className="w-5 h-5 text-blue-600 dark:text-blue-400" />
              </div>
              <div>
                <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                  ${totalValue.toFixed(0)}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Valor Total
                </p>
              </div>
            </div>
          </GlassCard>
          
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-yellow-100 dark:bg-yellow-900 rounded-lg">
                <AlertTriangle className="w-5 h-5 text-yellow-600 dark:text-yellow-400" />
              </div>
              <div>
                <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                  {lowStockCount}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Bajo Stock
                </p>
              </div>
            </div>
          </GlassCard>
          
          <GlassCard className="p-4">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-red-100 dark:bg-red-900 rounded-lg">
                <AlertTriangle className="w-5 h-5 text-red-600 dark:text-red-400" />
              </div>
              <div>
                <p className="text-2xl font-bold text-neutral-900 dark:text-neutral-100">
                  {outOfStockCount}
                </p>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Agotados
                </p>
              </div>
            </div>
          </GlassCard>
        </div>

        {/* Filters */}
        <GlassCard className="p-6">
          <div className="flex flex-col sm:flex-row gap-4">
            <Input
              placeholder="Buscar por nombre, marca o modelo..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              icon={Search}
              className="flex-1"
            />
            <select
              value={typeFilter}
              onChange={(e) => setTypeFilter(e.target.value)}
              className="px-4 py-2 border border-neutral-300 dark:border-neutral-600 rounded-lg bg-white/50 dark:bg-neutral-800/50 backdrop-blur-sm text-neutral-900 dark:text-neutral-100"
            >
              <option value="all">Todos los tipos</option>
              <option value="frame">Monturas</option>
              <option value="lens">Lentes</option>
              <option value="contact-lens">Lentillas</option>
              <option value="accessory">Accesorios</option>
            </select>
            <Button variant="ghost" icon={Filter}>
              Más Filtros
            </Button>
          </div>
        </GlassCard>

        {/* Products Table */}
        <GlassCard className="overflow-hidden">
          <div className="p-6 border-b border-neutral-200 dark:border-neutral-700">
            <div className="flex items-center justify-between">
              <div>
                <h2 className="text-xl font-semibold text-neutral-900 dark:text-neutral-100">
                  Inventario de Productos
                </h2>
                <p className="text-sm text-neutral-600 dark:text-neutral-400">
                  Mostrando {filteredProducts.length} de {products.length} productos
                </p>
              </div>
            </div>
          </div>
          <Table
            data={filteredProducts}
            columns={columns}
            searchable={false}
          />
        </GlassCard>

        {/* Product Form Modal */}
        <Modal
          isOpen={isModalOpen}
          onClose={handleCloseModal}
          title={editingProduct ? 'Editar Producto' : 'Nuevo Producto'}
          size="lg"
        >
          <form onSubmit={handleSubmit} className="space-y-6">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <Input
                label="Nombre del Producto"
                value={formData.name}
                onChange={(e) => setFormData(prev => ({ ...prev, name: e.target.value }))}
                required
              />
              <div>
                <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-300 mb-2">
                  Tipo *
                </label>
                <select
                  value={formData.type}
                  onChange={(e) => setFormData(prev => ({ ...prev, type: e.target.value as Product['type'] }))}
                  required
                  className="w-full px-4 py-2.5 border border-neutral-300 dark:border-neutral-600 rounded-lg bg-white/50 dark:bg-neutral-800/50 backdrop-blur-sm text-neutral-900 dark:text-neutral-100"
                >
                  <option value="frame">Montura</option>
                  <option value="lens">Lente</option>
                  <option value="contact-lens">Lentilla</option>
                  <option value="accessory">Accesorio</option>
                </select>
              </div>
              <Input
                label="Marca"
                value={formData.brand}
                onChange={(e) => setFormData(prev => ({ ...prev, brand: e.target.value }))}
                required
              />
              <Input
                label="Modelo"
                value={formData.model}
                onChange={(e) => setFormData(prev => ({ ...prev, model: e.target.value }))}
                required
              />
              <Input
                label="Precio ($)"
                type="number"
                step="0.01"
                value={formData.price}
                onChange={(e) => setFormData(prev => ({ ...prev, price: e.target.value }))}
                required
              />
              <Input
                label="Stock"
                type="number"
                value={formData.stock}
                onChange={(e) => setFormData(prev => ({ ...prev, stock: e.target.value }))}
                required
              />
            </div>
            
            <div>
              <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-300 mb-2">
                Descripción
              </label>
              <textarea
                value={formData.description}
                onChange={(e) => setFormData(prev => ({ ...prev, description: e.target.value }))}
                rows={3}
                className="w-full px-4 py-2.5 border border-neutral-300 dark:border-neutral-600 rounded-lg bg-white/50 dark:bg-neutral-800/50 backdrop-blur-sm text-neutral-900 dark:text-neutral-100 placeholder-neutral-500 dark:placeholder-neutral-400 focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
                placeholder="Descripción del producto..."
              />
            </div>
            
            {/* Campos específicos por tipo de producto */}
            <div>
              <label className="block text-sm font-medium text-neutral-700 dark:text-neutral-300 mb-4">
                Especificaciones del Producto
              </label>
              {renderSpecificationFields()}
            </div>
            
            <div className="flex justify-end space-x-3 pt-4">
              <Button variant="ghost" onClick={handleCloseModal}>
                Cancelar
              </Button>
              <Button variant="primary" type="submit">
                {editingProduct ? 'Actualizar' : 'Crear'} Producto
              </Button>
            </div>
          </form>
        </Modal>
      </div>
    </DashboardPageWrapper>
  );
}