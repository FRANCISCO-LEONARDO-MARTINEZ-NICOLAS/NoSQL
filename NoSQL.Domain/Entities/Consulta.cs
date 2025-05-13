namespace NoSQL.Domain.Entities
{
    public class Consulta
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public DateTime Fecha { get; set; }
        public required string Diagnostico { get; set; }
        public required string Recomendaciones { get; set; }
        public string? Derivaciones { get; set; } // Opcional
    }
}