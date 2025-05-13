namespace NoSQL.Domain.Entities
{
    public class Producto
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}