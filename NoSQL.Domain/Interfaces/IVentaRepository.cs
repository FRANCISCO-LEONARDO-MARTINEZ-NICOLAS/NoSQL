using NoSQL.Domain.Entities;

namespace NoSQL.Domain.Interfaces
{
    public interface IVentaRepository
    {
        Task<IEnumerable<Venta>> GetAllAsync();
        Task<Venta?> GetByIdAsync(Guid id);
        Task<IEnumerable<Venta>> GetByPacienteIdAsync(Guid pacienteId);
        Task<IEnumerable<Venta>> GetByOptometristaIdAsync(Guid optometristaId);
        Task CreateAsync(Venta venta);
        Task UpdateAsync(Venta venta);
        Task DeleteAsync(Guid id);
    }
} 