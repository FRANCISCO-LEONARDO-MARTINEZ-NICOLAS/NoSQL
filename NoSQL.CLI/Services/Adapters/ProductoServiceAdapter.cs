using NoSQL.Application.Interfaces;
using NoSQL.Application.Services;
using NoSQL.CLI.Services;
using NoSQL.Domain.Entities;

namespace NoSQL.CLI.Services.Adapters
{
    public class ProductoServiceAdapter : IProductoService
    {
        private readonly ProductoService _productoService;

        public ProductoServiceAdapter(ProductoService productoService)
        {
            _productoService = productoService;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _productoService.GetAllAsync();
        }

        public async Task<Producto?> GetByIdAsync(string id)
        {
            return await _productoService.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Producto>> GetByPacienteEmailAsync(string email)
        {
            var productos = await GetAllAsync();
            return productos.Where(p => p.PacienteEmail == email);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Producto producto)
        {
            return await _productoService.CreateAsync(producto);
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string id, Producto producto)
        {
            return await _productoService.UpdateAsync(id, producto);
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            return await _productoService.DeleteAsync(id);
        }

        public async Task<(bool Success, string Message)> UpdateStockAsync(string id, int cantidad)
        {
            return await _productoService.UpdateStockAsync(id, cantidad);
        }
    }
} 