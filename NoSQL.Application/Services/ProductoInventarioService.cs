using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Application.Services
{
    public class ProductoInventarioService : IProductoInventarioService
    {
        private readonly IProductoInventarioRepository _repository;

        public ProductoInventarioService(IProductoInventarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductoInventario>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ProductoInventario?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ProductoInventario>> GetByTipoAsync(string tipo)
        {
            return await _repository.GetByTipoAsync(tipo);
        }

        public async Task<IEnumerable<ProductoInventario>> SearchAsync(string query)
        {
            return await _repository.SearchAsync(query);
        }

        public async Task<(bool Success, string Message)> CreateAsync(ProductoInventario producto)
        {
            try
            {
                producto.Id = Guid.NewGuid().ToString();
                await _repository.AddAsync(producto);
                return (true, "Producto creado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear producto: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string id, ProductoInventario producto)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Producto no encontrado");
                
                producto.Id = id;
                producto.FechaCreacion = existing.FechaCreacion;
                await _repository.UpdateAsync(id, producto);
                return (true, "Producto actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar producto: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Producto no encontrado");
                
                await _repository.DeleteAsync(id);
                return (true, "Producto eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar producto: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateStockAsync(string id, int cantidad)
        {
            try
            {
                var producto = await _repository.GetByIdAsync(id);
                if (producto == null)
                    return (false, "Producto no encontrado");
                
                await _repository.UpdateStockAsync(id, cantidad);
                return (true, "Stock actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar stock: {ex.Message}");
            }
        }
    }
} 