namespace NoSQL.Domain.Entities
{
    public class ProductoInventario
    {
        public string Id { get; set; } = string.Empty;
        public required string Nombre { get; set; }
        public required string Tipo { get; set; }
        public required string Marca { get; set; }
        public required string Modelo { get; set; }
        public required decimal Precio { get; set; }
        public required int Stock { get; set; }
        public required string Descripcion { get; set; }
        public Dictionary<string, string> Especificaciones { get; set; } = new();
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;
        public bool Activo { get; set; } = true;
        public string Type { get; set; } = "ProductoInventario";
    }
} 