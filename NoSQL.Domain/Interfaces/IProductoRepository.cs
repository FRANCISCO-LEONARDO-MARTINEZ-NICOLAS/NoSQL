using NoSQL.Domain.Entities;

namespace NoSQL.Domain.Interfaces
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> GetAllAsync();
        Task<Producto?> GetByIdAsync(string id);
        Task<IEnumerable<Producto>> GetByPacientecorreoAsync(string correo);
        Task<IEnumerable<Producto>> GetByOptometristacorreoAsync(string correo);
        Task AddAsync(Producto producto);
        Task UpdateAsync(string id, Producto producto);
        Task DeleteAsync(string id);
    }
} 