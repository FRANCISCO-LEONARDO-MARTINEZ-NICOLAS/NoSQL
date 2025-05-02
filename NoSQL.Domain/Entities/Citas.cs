namespace NoSQL.Domain.Entities
{
    public class Cita
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid OptometristaId { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; }
    }
}