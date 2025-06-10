using NoSQL.Domain.Entities;

namespace NoSQL.Domain.Interfaces
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> GetAllAsync();
        Task<Producto?> GetByIdAsync(string id);
        Task<IEnumerable<Producto>> GetByPacienteEmailAsync(string email);
        Task<IEnumerable<Producto>> GetByOptometristaEmailAsync(string email);
        Task AddAsync(Producto producto);
        Task UpdateAsync(string id, Producto producto);
        Task DeleteAsync(string id);
    }
} 