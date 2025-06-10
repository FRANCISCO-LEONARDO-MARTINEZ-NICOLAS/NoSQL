using Couchbase;
using Couchbase.Core.Exceptions.KeyValue;
using Couchbase.Query;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Infrastructure.Repositories
{
    public class VentaRepository : IVentaRepository
    {
        private readonly CouchbaseDbContext _context;
        private const string CollectionName = "ventas";

        public VentaRepository(CouchbaseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Venta>> GetAllAsync()
        {
            var query = $"SELECT v.* FROM `{_context.BucketName}` v WHERE v.type = 'venta'";
            var result = await _context.Bucket.Cluster.QueryAsync<Venta>(query);
            return await result.ToListAsync();
        }

        public async Task<Venta?> GetByIdAsync(string id)
        {
            try
            {
                var result = await _context.Bucket.DefaultCollection()
                    .GetAsync(id);
                return result.ContentAs<Venta>();
            }
            catch (DocumentNotFoundException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Venta>> GetByPacienteIdAsync(string pacienteId)
        {
            var query = $"SELECT v.* FROM `{_context.BucketName}` v WHERE v.pacienteId = $pacienteId";
            var options = QueryOptions.Create().Parameter("pacienteId", pacienteId);
            var result = await _context.Bucket.Cluster.QueryAsync<Venta>(query, options);
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<Venta>> GetByOptometristaIdAsync(string optometristaId)
        {
            var query = $"SELECT v.* FROM `{_context.BucketName}` v WHERE v.optometristaId = $optometristaId";
            var options = QueryOptions.Create().Parameter("optometristaId", optometristaId);
            var result = await _context.Bucket.Cluster.QueryAsync<Venta>(query, options);
            return await result.ToListAsync();
        }

        public async Task CreateAsync(Venta venta)
        {
            await _context.Bucket.DefaultCollection()
                .InsertAsync(venta.Id, venta);
        }

        public async Task UpdateAsync(Venta venta)
        {
            await _context.Bucket.DefaultCollection()
                .ReplaceAsync(venta.Id, venta);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.Bucket.DefaultCollection()
                .RemoveAsync(id);
        }
    }
}