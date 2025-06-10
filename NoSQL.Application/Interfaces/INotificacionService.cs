using NoSQL.Domain.Entities;
using System.Threading.Tasks;

namespace NoSQL.Application.Interfaces
{
    public interface INotificacionService
    {
        Task<IEnumerable<Notificacion>> GetAllAsync();
        Task<Notificacion?> GetByIdAsync(string id);
        Task<IEnumerable<Notificacion>> GetByPacienteIdAsync(string pacienteId);
        Task<(bool Success, string Message)> CreateAsync(Notificacion notificacion);
        Task<(bool Success, string Message)> UpdateAsync(string id, Notificacion notificacion);
        Task<(bool Success, string Message)> DeleteAsync(string id);
        
        // Métodos específicos de negocio
        Task<(bool Success, string Message)> EnviarNotificacionAsync(string notificacionId);
        Task<IEnumerable<Notificacion>> GetPendientesAsync();
        Task<(bool Success, string Message)> MarcarComoEnviadaAsync(string notificacionId);
        Task<(bool Success, string Message)> CrearNotificacionProductoListoAsync(string pacienteId, string nombreProducto);

        // Métodos de notificación por correo
        Task<(bool Success, string Message)> EnviarCorreoBienvenidaAsync(string email, string nombre);
        Task<(bool Success, string Message)> EnviarCorreoCambioPasswordAsync(string email, string nombre);
        Task<(bool Success, string Message)> EnviarCorreoResetPasswordAsync(string email, string nombre, string nuevaPassword);
    }
} 