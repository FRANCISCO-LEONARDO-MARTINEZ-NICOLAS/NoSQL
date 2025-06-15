namespace NoSQL.Domain.Entities
{
    public class Optometrista
    {
        public string Id { get; set; } = string.Empty;
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string CedulaProfesional { get; set; }
        public required string Especialidad { get; set; }
        public required string Correo { get; set; }
        public required string Celular { get; set; }
        public required string NumeroEmergencia { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }

        // Agrega estas propiedades:
        public DateTime FechaContratacion { get; set; } = DateTime.UtcNow;
        public bool Activo { get; set; } = true;
        public string type { get; set; } = "Optometrista";
        
        // Credenciales para login
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public bool HasLoginCredentials { get; set; } = false;
    }
}