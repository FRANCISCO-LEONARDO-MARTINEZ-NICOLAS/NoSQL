namespace NoSQL.Domain.Entities
{
    public class HistorialClinico
    {
        public DateTime Fecha { get; set; }
        public required string Sintomas { get; set; }
        public required string Antecedentes { get; set; }
        public required string Medicamentos { get; set; }
        public string? Observaciones { get; set; }
    }
} 