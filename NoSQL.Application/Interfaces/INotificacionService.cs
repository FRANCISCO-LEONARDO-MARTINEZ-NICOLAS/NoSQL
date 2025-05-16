using NoSQL.Domain.Entities;

namespace NoSQL.Application.Interfaces
{
    public interface INotificacionService
    {
        Task<IEnumerable<Notificacion>> GetAllAsync();
        Task<Notificacion?> GetByIdAsync(Guid id);
        Task<IEnumerable<Notificacion>> GetByPacienteIdAsync(Guid pacienteId);
        Task<Notificacion> CreateAsync(Notificacion notificacion);
        Task UpdateAsync(Notificacion notificacion);
        Task DeleteAsync(Guid id);
        
        // Métodos específicos de negocio
        Task<bool> EnviarNotificacionAsync(Guid notificacionId);
        Task<IEnumerable<Notificacion>> GetPendientesAsync();
        Task<bool> MarcarComoEnviadaAsync(Guid notificacionId);
        Task<bool> CrearNotificacionProductoListoAsync(Guid pacienteId, string nombreProducto);
    }
} 