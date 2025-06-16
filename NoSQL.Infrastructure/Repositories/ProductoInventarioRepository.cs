using Couchbase;
using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using Couchbase.Query;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Infrastructure.Repositories
{
    public class ProductoInventarioRepository : IProductoInventarioRepository
    {
        private readonly ICouchbaseCollection _collection;
        private readonly ICluster _cluster;

        public ProductoInventarioRepository(CouchbaseDbContext context)
        {
            _collection = context.Bucket.DefaultCollection();
            _cluster = context.Cluster;
        }

        public async Task<IEnumerable<ProductoInventario>> GetAllAsync()
        {
            var query = "SELECT p.* FROM `productos` p WHERE p.Type = 'ProductoInventario'";
            var result = await _cluster.QueryAsync<ProductoInventario>(query);
            return await result.Rows.ToListAsync();
        }

        public async Task<ProductoInventario?> GetByIdAsync(string id)
        {
            try
            {
                var result = await _collection.GetAsync(id);
                var producto = result.ContentAs<ProductoInventario>();
                return producto.Type == "ProductoInventario" ? producto : null;
            }
            catch (DocumentNotFoundException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ProductoInventario>> GetByTipoAsync(string tipo)
        {
            var query = "SELECT p.* FROM `productos` p WHERE p.Type = 'ProductoInventario' AND p.Tipo = $tipo";
            var options = new QueryOptions().Parameter("tipo", tipo);
            var result = await _cluster.QueryAsync<ProductoInventario>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<ProductoInventario>> SearchAsync(string query)
        {
            var searchQuery = "SELECT p.* FROM `productos` p WHERE p.Type = 'ProductoInventario' AND (LOWER(p.Nombre) LIKE LOWER($search) OR LOWER(p.Marca) LIKE LOWER($search) OR LOWER(p.Modelo) LIKE LOWER($search))";
            var options = new QueryOptions().Parameter("search", $"%{query}%");
            var result = await _cluster.QueryAsync<ProductoInventario>(searchQuery, options);
            return await result.Rows.ToListAsync();
        }

        public async Task AddAsync(ProductoInventario producto)
        {
            producto.FechaCreacion = DateTime.UtcNow;
            producto.FechaActualizacion = DateTime.UtcNow;
            await _collection.InsertAsync(producto.Id, producto);
        }

        public async Task UpdateAsync(string id, ProductoInventario producto)
        {
            producto.FechaActualizacion = DateTime.UtcNow;
            await _collection.ReplaceAsync(id, producto);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.RemoveAsync(id);
        }

        public async Task UpdateStockAsync(string id, int cantidad)
        {
            var producto = await GetByIdAsync(id);
            if (producto != null)
            {
                producto.Stock = cantidad;
                producto.FechaActualizacion = DateTime.UtcNow;
                await UpdateAsync(id, producto);
            }
        }
    }
} 