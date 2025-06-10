namespace NoSQL.Domain.Entities
{
    public class Cita
    {
        public string Id { get; set; } = string.Empty;
        public required string PacienteId { get; set; }
        public required string OptometristaId { get; set; }
        public required string PacienteEmail { get; set; }
        public required string OptometristaEmail { get; set; }
        public required DateTime FechaHora { get; set; }
        public required string Tipo { get; set; }
        public required string Motivo { get; set; }
        public required string Estado { get; set; }
        public string? Observaciones { get; set; }
    }
} 