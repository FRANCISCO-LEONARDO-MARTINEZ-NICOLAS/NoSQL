using System;

namespace NoSQL.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty; // "Admin" o "Optometrista"
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoAcceso { get; set; }
        public string type { get; set; } = "usuario"; // Campo para consultas N1QL
    }
} 