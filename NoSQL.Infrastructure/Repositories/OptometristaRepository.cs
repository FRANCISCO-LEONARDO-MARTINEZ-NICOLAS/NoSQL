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
        private readonly ICouchbaseCollection _collection;
        private readonly ICluster _cluster;

        public OptometristaRepository(CouchbaseDbContext context)
        {
            _collection = context.Bucket.DefaultCollection();
            _cluster = context.Cluster;
        }

        public async Task<IEnumerable<Optometrista>> GetAllAsync()
        {
            var query = "SELECT o.* FROM `optometristas` o";
            var result = await _cluster.QueryAsync<Optometrista>(query);
            return await result.Rows.ToListAsync();
        }

        public async Task<Optometrista?> GetByIdAsync(string id)
        {
            try
            {
                var result = await _collection.GetAsync(id);
                return result.ContentAs<Optometrista>();
            }
            catch (DocumentNotFoundException)
            {
                return null;
            }
        }

        public async Task<Optometrista?> GetByEmailAsync(string correo)
        {
            var query = "SELECT o.* FROM `optometristas` o WHERE o.Correo = $correo LIMIT 1";
            var options = new QueryOptions().Parameter("correo", correo);
            var result = await _cluster.QueryAsync<Optometrista>(query, options);
            return await result.Rows.FirstOrDefaultAsync();
        }

        public async Task AddAsync(Optometrista optometrista)
        {
            await _collection.InsertAsync(optometrista.Id, optometrista);
        }

        public async Task UpdateAsync(string id, Optometrista optometrista)
        {
            await _collection.ReplaceAsync(id, optometrista);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.RemoveAsync(id);
        }
    }
}