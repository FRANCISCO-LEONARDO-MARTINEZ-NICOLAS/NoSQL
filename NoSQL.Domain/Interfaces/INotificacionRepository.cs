using NoSQL.Domain.Entities;

namespace NoSQL.Domain.Interfaces
{
    public interface INotificacionRepository
    {
        Task<IEnumerable<Notificacion>> GetAllAsync();
        Task<Notificacion?> GetByIdAsync(Guid id);
        Task<IEnumerable<Notificacion>> GetByPacienteIdAsync(Guid pacienteId);
        Task<IEnumerable<Notificacion>> GetPendientesAsync();
        Task CreateAsync(Notificacion notificacion);
        Task UpdateAsync(Notificacion notificacion);
        Task DeleteAsync(Guid id);
    }
} 