namespace NoSQL.Domain.Entities
{
    public class Paciente
    {
        public string Id { get; set; } = string.Empty;
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required DateTime FechaNacimiento { get; set; }
        public required string Genero { get; set; }
        public required string Direccion { get; set; }
        public required string Telefono { get; set; }
        public required string Correo { get; set; }
        public string? Dni { get; set; }
        public string? Ocupacion { get; set; }
        public string? SeguroMedico { get; set; }
        public List<HistorialClinico> HistorialClinico { get; set; } = new();

        // Agrega esta propiedad:
        public string type { get; set; } = "paciente";
    }
}