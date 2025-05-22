using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Infrastructure.Repositories;
using NoSQL.Application.Services.Interfaces;

namespace NoSQL.Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly ProductoRepository _repository;

        public ProductoService(ProductoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Producto?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Producto>> GetByPacienteEmailAsync(string email)
        {
            return await _repository.GetByPacienteEmailAsync(email);
        }

        public async Task<IEnumerable<Producto>> GetByOptometristaEmailAsync(string email)
        {
            return await _repository.GetByOptometristaEmailAsync(email);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Producto producto)
        {
            try
            {
                producto.Id = Guid.NewGuid();
                producto.FechaVenta = DateTime.UtcNow;
                await _repository.AddAsync(producto);
                return (true, "Producto creado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear producto: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateStatusAsync(string id, string newStatus)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid guidId))
                {
                    return (false, "ID de producto inválido");
                }

                var producto = await _repository.GetByIdAsync(guidId);
                if (producto == null)
                {
                    return (false, "Producto no encontrado");
                }

                producto.Estado = newStatus;
                await _repository.UpdateAsync(guidId, producto);
                return (true, "Estado del producto actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar estado del producto: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(Guid id, Producto producto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return (false, "Producto no encontrado");
            producto.Id = id;
            await _repository.UpdateAsync(id, producto);
            return (true, "Producto actualizado exitosamente");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return (false, "Producto no encontrado");
            await _repository.DeleteAsync(id);
            return (true, "Producto eliminado exitosamente");
        }
    }
}