using NoSQL.Domain.Entities;

namespace NoSQL.Domain.Interfaces
{
    public interface IProductoInventarioRepository
    {
        Task<IEnumerable<ProductoInventario>> GetAllAsync();
        Task<ProductoInventario?> GetByIdAsync(string id);
        Task<IEnumerable<ProductoInventario>> GetByTipoAsync(string tipo);
        Task<IEnumerable<ProductoInventario>> SearchAsync(string query);
        Task AddAsync(ProductoInventario producto);
        Task UpdateAsync(string id, ProductoInventario producto);
        Task DeleteAsync(string id);
        Task UpdateStockAsync(string id, int cantidad);
    }
} 