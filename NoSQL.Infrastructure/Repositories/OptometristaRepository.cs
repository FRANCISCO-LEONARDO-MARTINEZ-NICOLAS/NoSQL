using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using NoSQL.Domain.Entities;
using Couchbase.Query;

namespace NoSQL.Infrastructure.Repositories
{
    public class OptometristaRepository
    {
        private readonly ICouchbaseCollection _collection;

        public OptometristaRepository(CouchbaseDbContext context)
        {
            _collection = context.GetCollection("optometristas");
        }

        public async Task<List<Optometrista>> GetAllAsync()
        {
            var query = "SELECT o.* FROM `optometristas` o";
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Optometrista>(query);
            return await result.Rows.ToListAsync();
        }

        public async Task<Optometrista?> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _collection.GetAsync(id.ToString());
                return result.ContentAs<Optometrista>();
            }
            catch (KeyValueException ex) when (ex.Message.Contains("Document not found"))
            {
                return null;
            }
        }

        public async Task AddAsync(Optometrista optometrista)
        {
            optometrista.Id = Guid.NewGuid();
            await _collection.InsertAsync(optometrista.Id.ToString(), optometrista);
        }

        public async Task UpdateAsync(Guid id, Optometrista optometrista)
        {
            await _collection.ReplaceAsync(id.ToString(), optometrista);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _collection.RemoveAsync(id.ToString());
        }

        public async Task<Optometrista?> GetByEmailAsync(string email)
        {
            var query = $"SELECT o.* FROM `optometristas` o WHERE o.Correo = $email";
            var options = new Couchbase.Query.QueryOptions().Parameter("email", email);
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Optometrista>(query, options);
            return await result.Rows.FirstOrDefaultAsync();
        }
    }
}