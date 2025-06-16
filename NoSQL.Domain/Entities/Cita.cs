namespace NoSQL.Domain.Entities
{
    public class Cita
    {
        public string Id { get; set; } = string.Empty;
        public required string PacienteId { get; set; }
        public required string OptometristaId { get; set; }
        public required string Pacientecorreo { get; set; }
        public required string Optometristacorreo { get; set; }
        public required DateTime FechaHora { get; set; }
        public required string Tipo { get; set; }
        public required string Motivo { get; set; }
        public required string Estado { get; set; }
        public string? Observaciones { get; set; }
        public string? Notas { get; set; }
        
        // Campo para identificar el tipo de documento en Couchbase
        public string type { get; set; } = "cita";
    }
} 