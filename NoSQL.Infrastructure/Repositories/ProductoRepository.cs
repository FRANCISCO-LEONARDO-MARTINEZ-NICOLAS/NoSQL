using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using NoSQL.Domain.Entities;
using Couchbase.Query;

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
            var query = "SELECT p.* FROM `productos` p";
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Producto>(query);
            return await result.Rows.ToListAsync();
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

        public async Task<IEnumerable<Producto>> GetByPacienteEmailAsync(string pacienteEmail)
        {
            var query = $"SELECT p.* FROM `productos` p WHERE p.PacienteEmail = $pacienteEmail";
            var options = new QueryOptions().Parameter("pacienteEmail", pacienteEmail);
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Producto>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetByOptometristaEmailAsync(string optometristaEmail)
        {
            var query = $"SELECT p.* FROM `productos` p WHERE p.OptometristaEmail = $optometristaEmail";
            var options = new QueryOptions().Parameter("optometristaEmail", optometristaEmail);
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Producto>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task UpdateStatusAsync(Guid id, string nuevoEstado)
        {
            var producto = await GetByIdAsync(id);
            if (producto != null)
            {
                producto.Estado = nuevoEstado;
                await UpdateAsync(id, producto);
            }
        }

        public async Task UpdateStatusAsync(string id, string nuevoEstado)
        {
            if (Guid.TryParse(id, out Guid guidId))
            {
                await UpdateStatusAsync(guidId, nuevoEstado);
            }
            else
            {
                throw new ArgumentException("ID inválido", nameof(id));
            }
        }
    }
}