namespace NoSQL.Domain.Entities
{
    public class Producto
    {
        public string Id { get; set; } = string.Empty;
        public required string Nombre { get; set; }
        public required string Tipo { get; set; }
        public required string Descripcion { get; set; }
        public required decimal Precio { get; set; }
        public required string PacienteEmail { get; set; }
        public required string OptometristaEmail { get; set; }
        public required DateTime FechaVenta { get; set; }
        public required DateTime FechaEntregaEstimada { get; set; }
        public required string Estado { get; set; }
        public int Stock { get; set; } = 0;
        public string? Observaciones { get; set; }
    }
}