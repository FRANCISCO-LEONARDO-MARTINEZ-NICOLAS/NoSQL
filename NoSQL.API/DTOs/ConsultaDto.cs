using System.ComponentModel.DataAnnotations;

namespace NoSQL.API.DTOs
{
    public class ConsultaDto
    {
        public string Id { get; set; } = string.Empty;
        public string PacienteId { get; set; } = string.Empty;
        public string OptometristaId { get; set; } = string.Empty;
        public string Pacientecorreo { get; set; } = string.Empty;
        public string Optometristacorreo { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string Sintomas { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;
        public string Tratamiento { get; set; } = string.Empty;
        public string Recomendaciones { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        
        // Datos adicionales para el frontend
        public AgudezaVisualDto? AgudezaVisual { get; set; }
        public RefraccionDto? Refraccion { get; set; }
        public DateTime? FechaSeguimiento { get; set; }
        
        // Datos del paciente para mostrar en el frontend
        public string? NombrePaciente { get; set; }
        public string? ApellidoPaciente { get; set; }
        public string? NombreOptometrista { get; set; }
        public string? ApellidoOptometrista { get; set; }
    }

    public class AgudezaVisualDto
    {
        public string OjoDerecho { get; set; } = string.Empty;
        public string OjoIzquierdo { get; set; } = string.Empty;
    }

    public class RefraccionDto
    {
        public OjoRefraccionDto OjoDerecho { get; set; } = new();
        public OjoRefraccionDto OjoIzquierdo { get; set; } = new();
    }

    public class OjoRefraccionDto
    {
        public double Esfera { get; set; }
        public double Cilindro { get; set; }
        public int Eje { get; set; }
    }

    public class CreateConsultaDto
    {
        [Required]
        public string PacienteId { get; set; } = string.Empty;
        
        [Required]
        public string OptometristaId { get; set; } = string.Empty;
        
        [Required]
        public string Pacientecorreo { get; set; } = string.Empty;
        
        [Required]
        public string Optometristacorreo { get; set; } = string.Empty;
        
        [Required]
        public DateTime Fecha { get; set; }
        
        [Required]
        public string Motivo { get; set; } = string.Empty;
        
        [Required]
        public string Sintomas { get; set; } = string.Empty;
        
        [Required]
        public string Diagnostico { get; set; } = string.Empty;
        
        [Required]
        public string Tratamiento { get; set; } = string.Empty;
        
        [Required]
        public string Recomendaciones { get; set; } = string.Empty;
        
        public string? Observaciones { get; set; }
        
        public AgudezaVisualDto? AgudezaVisual { get; set; }
        public RefraccionDto? Refraccion { get; set; }
        public DateTime? FechaSeguimiento { get; set; }
    }

    public class UpdateConsultaDto
    {
        public string? Motivo { get; set; }
        public string? Sintomas { get; set; }
        public string? Diagnostico { get; set; }
        public string? Tratamiento { get; set; }
        public string? Recomendaciones { get; set; }
        public string? Observaciones { get; set; }
        public AgudezaVisualDto? AgudezaVisual { get; set; }
        public RefraccionDto? Refraccion { get; set; }
        public DateTime? FechaSeguimiento { get; set; }
    }
} 