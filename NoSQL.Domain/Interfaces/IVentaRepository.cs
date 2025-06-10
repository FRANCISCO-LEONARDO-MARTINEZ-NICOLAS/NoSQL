using NoSQL.Domain.Entities;

namespace NoSQL.Domain.Interfaces
{
    public interface IVentaRepository
    {
        Task<IEnumerable<Venta>> GetAllAsync();
        Task<Venta?> GetByIdAsync(string id);
        Task<IEnumerable<Venta>> GetByPacienteIdAsync(string pacienteId);
        Task<IEnumerable<Venta>> GetByOptometristaIdAsync(string optometristaId);
        Task CreateAsync(Venta venta);
        Task UpdateAsync(Venta venta);
        Task DeleteAsync(string id);
    }
} 