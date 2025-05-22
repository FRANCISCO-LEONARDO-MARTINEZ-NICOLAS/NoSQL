namespace NoSQL.Domain.Entities
{
    public class Producto
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string Tipo { get; set; }
        public required string Descripcion { get; set; }
        public required decimal Precio { get; set; }
        public required string PacienteEmail { get; set; }
        public required string OptometristaEmail { get; set; }
        public required DateTime FechaVenta { get; set; }
        public required DateTime FechaEntregaEstimada { get; set; }
        public required string Estado { get; set; }
        public string? Observaciones { get; set; }
    }
}