namespace NoSQL.Domain.Entities
{
    public class Consulta
    {
        public Guid Id { get; set; }
        public required Guid PacienteId { get; set; }
        public required Guid OptometristaId { get; set; }
        public required string PacienteEmail { get; set; }
        public required string OptometristaEmail { get; set; }
        public required DateTime Fecha { get; set; }
        public required string Motivo { get; set; }
        public required string Sintomas { get; set; }
        public required string Diagnostico { get; set; }
        public required string Tratamiento { get; set; }
        public required string Recomendaciones { get; set; }
        public string? Observaciones { get; set; }
    }
}