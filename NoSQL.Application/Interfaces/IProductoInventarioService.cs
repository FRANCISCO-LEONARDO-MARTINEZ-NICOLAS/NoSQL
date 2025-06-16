using NoSQL.Domain.Entities;

namespace NoSQL.Application.Interfaces
{
    public interface IProductoInventarioService
    {
        Task<IEnumerable<ProductoInventario>> GetAllAsync();
        Task<ProductoInventario?> GetByIdAsync(string id);
        Task<IEnumerable<ProductoInventario>> GetByTipoAsync(string tipo);
        Task<IEnumerable<ProductoInventario>> SearchAsync(string query);
        Task<(bool Success, string Message)> CreateAsync(ProductoInventario producto);
        Task<(bool Success, string Message)> UpdateAsync(string id, ProductoInventario producto);
        Task<(bool Success, string Message)> DeleteAsync(string id);
        Task<(bool Success, string Message)> UpdateStockAsync(string id, int cantidad);
    }
} 