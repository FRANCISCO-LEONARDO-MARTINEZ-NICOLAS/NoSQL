namespace NoSQL.API.DTOs
{
    public class CrearNotificacionDto
    {
        public required string PacienteId { get; set; }
        public required string correo { get; set; }
        public required string Mensaje { get; set; }
    }

    public class NotificacionResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string PacienteId { get; set; } = string.Empty;
        public string correo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaEnvio { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Error { get; set; }
    }
} 