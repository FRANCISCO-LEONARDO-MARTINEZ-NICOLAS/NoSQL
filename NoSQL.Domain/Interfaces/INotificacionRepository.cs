using NoSQL.Domain.Entities;

namespace NoSQL.Domain.Interfaces
{
    public interface INotificacionRepository
    {
        Task<IEnumerable<Notificacion>> GetAllAsync();
        Task<Notificacion?> GetByIdAsync(string id);
        Task<IEnumerable<Notificacion>> GetByPacienteIdAsync(string pacienteId);
        Task<IEnumerable<Notificacion>> GetPendientesAsync();
        Task CreateAsync(Notificacion notificacion);
        Task UpdateAsync(Notificacion notificacion);
        Task DeleteAsync(string id);
    }
} 