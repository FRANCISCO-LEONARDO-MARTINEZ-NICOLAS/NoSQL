using NoSQL.Domain.Entities;

namespace NoSQL.Application.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> GetAllAsync();
        Task<Producto?> GetByIdAsync(string id);
        Task<(bool Success, string Message)> CreateAsync(Producto producto);
        Task<(bool Success, string Message)> UpdateAsync(string id, Producto producto);
        Task<(bool Success, string Message)> DeleteAsync(string id);
        Task<(bool Success, string Message)> UpdateStockAsync(string id, int cantidad);
    }
} 