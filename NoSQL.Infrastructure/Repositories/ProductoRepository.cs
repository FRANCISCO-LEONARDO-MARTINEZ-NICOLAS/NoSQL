using Couchbase;
using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using Couchbase.Query;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly ICouchbaseCollection _collection;
        private readonly ICluster _cluster;

        public ProductoRepository(CouchbaseDbContext context)
        {
            _collection = context.Bucket.DefaultCollection();
            _cluster = context.Cluster;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            var query = "SELECT p.* FROM `productos` p";
            var result = await _cluster.QueryAsync<Producto>(query);
            return await result.Rows.ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(string id)
        {
            try
            {
                var result = await _collection.GetAsync(id);
                return result.ContentAs<Producto>();
            }
            catch (DocumentNotFoundException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Producto>> GetByPacienteEmailAsync(string email)
        {
            var query = "SELECT p.* FROM `productos` p WHERE p.PacienteEmail = $email";
            var options = new QueryOptions().Parameter("email", email);
            var result = await _cluster.QueryAsync<Producto>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetByOptometristaEmailAsync(string email)
        {
            var query = "SELECT p.* FROM `productos` p WHERE p.OptometristaEmail = $email";
            var options = new QueryOptions().Parameter("email", email);
            var result = await _cluster.QueryAsync<Producto>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task AddAsync(Producto producto)
        {
            await _collection.InsertAsync(producto.Id, producto);
        }

        public async Task UpdateAsync(string id, Producto producto)
        {
            await _collection.ReplaceAsync(id, producto);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.RemoveAsync(id);
        }

        public async Task UpdateStatusAsync(string id, string nuevoEstado)
        {
            var producto = await GetByIdAsync(id);
            if (producto != null)
            {
                producto.Estado = nuevoEstado;
                await UpdateAsync(id, producto);
            }
        }
    }
}