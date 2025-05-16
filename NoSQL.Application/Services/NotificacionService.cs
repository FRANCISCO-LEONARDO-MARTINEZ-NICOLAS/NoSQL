using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;
using System.Net.Mail;
using System.Net;

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
            string smtpServer,
            int smtpPort,
            string smtpUsername,
            string smtpPassword)
        {
            _notificacionRepository = notificacionRepository;
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
        }

        public async Task<IEnumerable<Notificacion>> GetAllAsync()
        {
            return await _notificacionRepository.GetAllAsync();
        }

        public async Task<Notificacion?> GetByIdAsync(Guid id)
        {
            return await _notificacionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Notificacion>> GetByPacienteIdAsync(Guid pacienteId)
        {
            return await _notificacionRepository.GetByPacienteIdAsync(pacienteId);
        }

        public async Task<Notificacion> CreateAsync(Notificacion notificacion)
        {
            notificacion.Id = Guid.NewGuid();
            notificacion.FechaEnvio = DateTime.UtcNow;
            notificacion.Estado = "Pendiente";
            
            await _notificacionRepository.CreateAsync(notificacion);
            return notificacion;
        }

        public async Task UpdateAsync(Notificacion notificacion)
        {
            await _notificacionRepository.UpdateAsync(notificacion);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _notificacionRepository.DeleteAsync(id);
        }

        public async Task<bool> EnviarNotificacionAsync(Guid notificacionId)
        {
            var notificacion = await GetByIdAsync(notificacionId);
            if (notificacion == null)
                return false;

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
                await UpdateAsync(notificacion);

                return true;
            }
            catch (Exception ex)
            {
                notificacion.Estado = "Error";
                notificacion.Error = ex.Message;
                await UpdateAsync(notificacion);
                return false;
            }
        }

        public async Task<IEnumerable<Notificacion>> GetPendientesAsync()
        {
            return await _notificacionRepository.GetPendientesAsync();
        }

        public async Task<bool> MarcarComoEnviadaAsync(Guid notificacionId)
        {
            var notificacion = await GetByIdAsync(notificacionId);
            if (notificacion == null)
                return false;

            notificacion.Estado = "Enviado";
            notificacion.FechaEnvio = DateTime.UtcNow;
            await UpdateAsync(notificacion);
            return true;
        }

        public async Task<bool> CrearNotificacionProductoListoAsync(Guid pacienteId, string nombreProducto)
        {
            var notificacion = new Notificacion
            {
                Id = Guid.NewGuid(),
                PacienteId = pacienteId,
                Correo = "leonardomtz000426@gmail.com", // Correo configurado
                Mensaje = $"Su producto {nombreProducto} está listo para recoger en la óptica.",
                Estado = "Pendiente",
                FechaEnvio = DateTime.UtcNow
            };

            await _notificacionRepository.CreateAsync(notificacion);
            return await EnviarNotificacionAsync(notificacion.Id);
        }
    }
} 