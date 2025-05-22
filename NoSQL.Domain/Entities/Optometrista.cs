namespace NoSQL.Domain.Entities
{
    public class Optometrista
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string CedulaProfesional { get; set; }
        public required string Especialidad { get; set; }
        public required string Correo { get; set; }
        public required string Celular { get; set; }
        public required string NumeroEmergencia { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
    }
}