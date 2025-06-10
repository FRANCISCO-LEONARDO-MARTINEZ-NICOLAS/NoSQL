namespace NoSQL.Domain.Entities
{
    public class Venta
    {
        public string Id { get; set; } = string.Empty;
        public string PacienteId { get; set; } = string.Empty;
        public string OptometristaId { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public decimal MontoTotal { get; set; }
        public required string MetodoPago { get; set; }
        public List<ProductoVenta> Productos { get; set; } = new();
        public string Estado { get; set; } = "Pendiente";
        
        // Método para calcular el monto total
        public void CalcularMontoTotal()
        {
            MontoTotal = Productos.Sum(p => p.Subtotal);
        }
    }
} 