namespace NoSQL.Domain.Entities
{
    public class Notificacion
    {
        public string Id { get; set; } = string.Empty;
        public string PacienteId { get; set; } = string.Empty;
        public required string Correo { get; set; }
        public required string Mensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public string? Error { get; set; }
    }
} 