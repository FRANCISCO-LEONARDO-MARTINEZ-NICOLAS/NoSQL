using Couchbase;
using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using Couchbase.Query;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Infrastructure.Repositories
{
    public class OptometristaRepository : IOptometristaRepository
    {
        private readonly CouchbaseDbContext _context;

        public OptometristaRepository(CouchbaseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Optometrista>> GetAllAsync()
        {
            var query = $"SELECT o.* FROM `{_context.BucketName}` o WHERE o.type = 'Optometrista'";
            var result = await _context.Bucket.Cluster.QueryAsync<Optometrista>(query);
            return await result.ToListAsync();
        }

        public async Task<Optometrista?> GetByIdAsync(string id)
        {
            try
            {
                var result = await _context.Bucket.DefaultCollection().GetAsync(id);
                return result.ContentAs<Optometrista>();
            }
            catch (DocumentNotFoundException)
            {
                return null;
            }
        }

        public async Task<Optometrista?> GetBycorreoAsync(string correo)
        {
            var query = $"SELECT o.* FROM `{_context.BucketName}` o WHERE o.type = 'Optometrista' AND o.correo = $correo LIMIT 1";
            var options = QueryOptions.Create().Parameter("correo", correo);
            var result = await _context.Bucket.Cluster.QueryAsync<Optometrista>(query, options);
            return (await result.ToListAsync()).FirstOrDefault();
        }

        public async Task AddAsync(Optometrista optometrista)
        {
            optometrista.type = "Optometrista";
            await _context.Bucket.DefaultCollection().InsertAsync(optometrista.Id, optometrista);
        }

        public async Task UpdateAsync(string id, Optometrista optometrista)
        {
            optometrista.type = "Optometrista";
            await _context.Bucket.DefaultCollection().ReplaceAsync(id, optometrista);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.Bucket.DefaultCollection().RemoveAsync(id);
        }
    }
}