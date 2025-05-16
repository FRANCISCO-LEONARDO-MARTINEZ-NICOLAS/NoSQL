namespace NoSQL.API.DTOs
{
    public class CrearNotificacionDto
    {
        public required Guid PacienteId { get; set; }
        public required string Correo { get; set; }
        public required string Mensaje { get; set; }
    }

    public class NotificacionResponseDto
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaEnvio { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Error { get; set; }
    }
} 