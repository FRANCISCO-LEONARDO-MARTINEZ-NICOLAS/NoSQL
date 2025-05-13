namespace NoSQL.Domain.Entities
{
    public class Cita
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public DateTime FechaHora { get; set; }
        public required string Motivo { get; set; }
        public required string Estado { get; set; }
    }
}