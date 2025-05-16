namespace NoSQL.Domain.Entities
{
    public class Notificacion
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public required string Correo { get; set; }
        public required string Mensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public string? Error { get; set; }
    }
} 