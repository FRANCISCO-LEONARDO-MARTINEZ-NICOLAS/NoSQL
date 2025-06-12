namespace NoSQL.Domain.Entities
{
    public class Consulta
    {
        public string Id { get; set; } = string.Empty;
        public required string PacienteId { get; set; }
        public required string OptometristaId { get; set; }
        public required string Pacientecorreo { get; set; }
        public required string Optometristacorreo { get; set; }
        public required DateTime Fecha { get; set; }
        public required string Motivo { get; set; }
        public required string Sintomas { get; set; }
        public required string Diagnostico { get; set; }
        public required string Tratamiento { get; set; }
        public required string Recomendaciones { get; set; }
        public string? Observaciones { get; set; }
        
        // Datos adicionales para el frontend
        public AgudezaVisual? AgudezaVisual { get; set; }
        public Refraccion? Refraccion { get; set; }
        public DateTime? FechaSeguimiento { get; set; }
    }

    public class AgudezaVisual
    {
        public string OjoDerecho { get; set; } = string.Empty;
        public string OjoIzquierdo { get; set; } = string.Empty;
    }

    public class Refraccion
    {
        public OjoRefraccion OjoDerecho { get; set; } = new();
        public OjoRefraccion OjoIzquierdo { get; set; } = new();
    }

    public class OjoRefraccion
    {
        public double Esfera { get; set; }
        public double Cilindro { get; set; }
        public int Eje { get; set; }
    }
}