namespace NoSQL.API.DTOs
{
    public class CrearVentaDto
    {
        public required Guid PacienteId { get; set; }
        public required Guid OptometristaId { get; set; }
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
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid OptometristaId { get; set; }
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