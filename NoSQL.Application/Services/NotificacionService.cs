using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace NoSQL.Application.Services
{
    public class NotificacionService : INotificacionService
    {
        private readonly INotificacionRepository _notificacionRepository;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public NotificacionService(
            INotificacionRepository notificacionRepository,
            IOptions<NotificacionOptions> options)
        {
            _notificacionRepository = notificacionRepository;
            _smtpServer = options.Value.SmtpServer;
            _smtpPort = options.Value.SmtpPort;
            _smtpUsername = options.Value.SmtpUsername;
            _smtpPassword = options.Value.SmtpPassword;
        }

        public async Task<IEnumerable<Notificacion>> GetAllAsync()
        {
            return await _notificacionRepository.GetAllAsync();
        }

        public async Task<Notificacion?> GetByIdAsync(string id)
        {
            return await _notificacionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Notificacion>> GetByPacienteIdAsync(string pacienteId)
        {
            return await _notificacionRepository.GetByPacienteIdAsync(pacienteId);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Notificacion notificacion)
        {
            try
            {
                notificacion.Id = Guid.NewGuid().ToString();
                notificacion.FechaEnvio = DateTime.UtcNow;
                notificacion.Estado = "Pendiente";
                await _notificacionRepository.CreateAsync(notificacion);
                return (true, "Notificación creada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear notificación: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string id, Notificacion notificacion)
        {
            try
            {
                var existing = await _notificacionRepository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Notificación no encontrada");
                notificacion.Id = id;
                await _notificacionRepository.UpdateAsync(notificacion);
                return (true, "Notificación actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar notificación: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            try
            {
                var existing = await _notificacionRepository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Notificación no encontrada");
                await _notificacionRepository.DeleteAsync(id);
                return (true, "Notificación eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar notificación: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> EnviarNotificacionAsync(string notificacionId)
        {
            var notificacion = await GetByIdAsync(notificacionId);
            if (notificacion == null)
                return (false, "Notificación no encontrada");
            try
            {
                using var smtpClient = new SmtpClient(_smtpServer, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                    EnableSsl = true
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpUsername),
                    Subject = "Notificación de Óptica",
                    Body = notificacion.Mensaje,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(notificacion.Correo);
                await smtpClient.SendMailAsync(mailMessage);
                notificacion.Estado = "Enviado";
                notificacion.FechaEnvio = DateTime.UtcNow;
                await _notificacionRepository.UpdateAsync(notificacion);
                return (true, "Notificación enviada exitosamente");
            }
            catch (Exception ex)
            {
                notificacion.Estado = "Error";
                notificacion.Error = ex.Message;
                await _notificacionRepository.UpdateAsync(notificacion);
                return (false, $"Error al enviar notificación: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Notificacion>> GetPendientesAsync()
        {
            return await _notificacionRepository.GetPendientesAsync();
        }

        public async Task<(bool Success, string Message)> MarcarComoEnviadaAsync(string notificacionId)
        {
            var notificacion = await GetByIdAsync(notificacionId);
            if (notificacion == null)
                return (false, "Notificación no encontrada");
            notificacion.Estado = "Enviado";
            notificacion.FechaEnvio = DateTime.UtcNow;
            await _notificacionRepository.UpdateAsync(notificacion);
            return (true, "Notificación marcada como enviada");
        }

        public async Task<(bool Success, string Message)> CrearNotificacionProductoListoAsync(string pacienteId, string nombreProducto)
        {
            try
            {
                var notificacion = new Notificacion
                {
                    Id = Guid.NewGuid().ToString(),
                    PacienteId = pacienteId,
                    Correo = "leonardomtz000426@gmail.com", // Correo configurado
                    Mensaje = $"Su producto {nombreProducto} está listo para recoger en la óptica.",
                    Estado = "Pendiente",
                    FechaEnvio = DateTime.UtcNow
                };
                await _notificacionRepository.CreateAsync(notificacion);
                return await EnviarNotificacionAsync(notificacion.Id);
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear notificación de producto listo: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> EnviarCorreoBienvenidaAsync(string email, string nombre)
        {
            try
            {
                var notificacion = new Notificacion
                {
                    Id = Guid.NewGuid().ToString(),
                    PacienteId = string.Empty, // No es necesario para correos de bienvenida
                    Correo = email,
                    Mensaje = $@"
                        <h2>¡Bienvenido/a a Óptica NoSQL, {nombre}!</h2>
                        <p>Gracias por registrarte en nuestro sistema. Estamos aquí para cuidar de tu salud visual.</p>
                        <p>Si tienes alguna pregunta, no dudes en contactarnos.</p>
                        <br>
                        <p>Saludos cordiales,</p>
                        <p>El equipo de Óptica NoSQL</p>",
                    Estado = "Pendiente",
                    FechaEnvio = DateTime.UtcNow
                };
                await _notificacionRepository.CreateAsync(notificacion);
                return await EnviarNotificacionAsync(notificacion.Id);
            }
            catch (Exception ex)
            {
                return (false, $"Error al enviar correo de bienvenida: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> EnviarCorreoCambioPasswordAsync(string email, string nombre)
        {
            try
            {
                var notificacion = new Notificacion
                {
                    Id = Guid.NewGuid().ToString(),
                    PacienteId = string.Empty, // No es necesario para correos de cambio de contraseña
                    Correo = email,
                    Mensaje = $@"
                        <h2>Cambio de Contraseña Exitoso</h2>
                        <p>Hola {nombre},</p>
                        <p>Tu contraseña ha sido cambiada exitosamente.</p>
                        <p>Si no realizaste este cambio, por favor contacta con nosotros inmediatamente.</p>
                        <br>
                        <p>Saludos cordiales,</p>
                        <p>El equipo de Óptica NoSQL</p>",
                    Estado = "Pendiente",
                    FechaEnvio = DateTime.UtcNow
                };
                await _notificacionRepository.CreateAsync(notificacion);
                return await EnviarNotificacionAsync(notificacion.Id);
            }
            catch (Exception ex)
            {
                return (false, $"Error al enviar correo de cambio de contraseña: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> EnviarCorreoResetPasswordAsync(string email, string nombre, string nuevaPassword)
        {
            try
            {
                var notificacion = new Notificacion
                {
                    Id = Guid.NewGuid().ToString(),
                    PacienteId = string.Empty, // No es necesario para correos de reset de contraseña
                    Correo = email,
                    Mensaje = $@"
                        <h2>Nueva Contraseña Generada</h2>
                        <p>Hola {nombre},</p>
                        <p>Se ha generado una nueva contraseña para tu cuenta:</p>
                        <p><strong>{nuevaPassword}</strong></p>
                        <p>Por favor, inicia sesión y cambia tu contraseña por una nueva lo antes posible.</p>
                        <br>
                        <p>Saludos cordiales,</p>
                        <p>El equipo de Óptica NoSQL</p>",
                    Estado = "Pendiente",
                    FechaEnvio = DateTime.UtcNow
                };
                await _notificacionRepository.CreateAsync(notificacion);
                return await EnviarNotificacionAsync(notificacion.Id);
            }
            catch (Exception ex)
            {
                return (false, $"Error al enviar correo de reset de contraseña: {ex.Message}");
            }
        }
    }
}