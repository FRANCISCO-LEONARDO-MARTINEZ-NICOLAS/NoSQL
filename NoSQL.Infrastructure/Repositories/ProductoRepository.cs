using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using NoSQL.Domain.Entities;

namespace NoSQL.Infrastructure.Repositories
{
    public class ProductoRepository
    {
        private readonly ICouchbaseCollection _collection;

        public ProductoRepository(CouchbaseDbContext context)
        {
            _collection = context.GetCollection("productos");
        }

        public async Task<List<Producto>> GetAllAsync()
        {
            // Implementa una consulta N1QL si es necesario
            throw new NotImplementedException("Consulta para obtener todos los productos no implementada.");
        }

        public async Task<Producto?> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _collection.GetAsync(id.ToString());
                return result.ContentAs<Producto>();
            }
            catch (KeyValueException ex) when (ex.Message.Contains("Document not found"))
            {
                return null;
            }
        }

        public async Task AddAsync(Producto producto)
        {
            producto.Id = Guid.NewGuid();
            await _collection.InsertAsync(producto.Id.ToString(), producto);
        }

        public async Task UpdateAsync(Guid id, Producto producto)
        {
            await _collection.ReplaceAsync(id.ToString(), producto);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _collection.RemoveAsync(id.ToString());
        }
    }
}