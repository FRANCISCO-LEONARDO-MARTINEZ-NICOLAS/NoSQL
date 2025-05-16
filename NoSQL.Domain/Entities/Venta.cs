namespace NoSQL.Domain.Entities
{
    public class Venta
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid OptometristaId { get; set; }
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