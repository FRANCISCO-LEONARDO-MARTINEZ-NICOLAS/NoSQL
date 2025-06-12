using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repository;

        public ProductoService(IProductoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Producto?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Producto>> GetByPacientecorreoAsync(string correo)
        {
            return await _repository.GetByPacientecorreoAsync(correo);
        }

        public async Task<IEnumerable<Producto>> GetByOptometristacorreoAsync(string correo)
        {
            return await _repository.GetByOptometristacorreoAsync(correo);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Producto producto)
        {
            try
            {
                producto.Id = Guid.NewGuid().ToString();
                producto.FechaVenta = DateTime.UtcNow;
                await _repository.AddAsync(producto);
                return (true, "Producto creado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear producto: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string id, Producto producto)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Producto no encontrado");
                producto.Id = id;
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
                producto.Stock += cantidad;
                await _repository.UpdateAsync(id, producto);
                return (true, "Stock actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar stock: {ex.Message}");
            }
        }
    }
}