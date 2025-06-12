using System;

namespace NoSQL.Domain.Entities
{
    public class Usuario
    {
        public string Id { get; set; } = string.Empty;
        public required string Nombre { get; set; }
        public required string Correo { get; set; }
        public required string PasswordHash { get; set; }
        public required string Rol { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoAcceso { get; set; }
        public string type { get; set; } = "usuario"; // Campo para consultas N1QL
    }
}