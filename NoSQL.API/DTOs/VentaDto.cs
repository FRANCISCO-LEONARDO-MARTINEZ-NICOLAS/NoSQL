namespace NoSQL.API.DTOs
{
    public class CrearVentaDto
    {
        public required string PacienteId { get; set; }
        public required string OptometristaId { get; set; }
        public required string MetodoPago { get; set; }
        public List<CrearProductoVentaDto> Productos { get; set; } = new();
    }

    public class CrearProductoVentaDto
    {
        public required string Nombre { get; set; }
        public required int Cantidad { get; set; }
        public required decimal PrecioUnitario { get; set; }
    }

    public class ActualizarEstadoVentaDto
    {
        public required string NuevoEstado { get; set; }
    }

    public class VentaResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string PacienteId { get; set; } = string.Empty;
        public string OptometristaId { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal MontoTotal { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public List<ProductoVentaResponseDto> Productos { get; set; } = new();
    }

    public class ProductoVentaResponseDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
} 