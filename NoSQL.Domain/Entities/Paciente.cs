namespace NoSQL.Domain.Entities
{
    public class Paciente
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Correo { get; set; }
        public required string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}