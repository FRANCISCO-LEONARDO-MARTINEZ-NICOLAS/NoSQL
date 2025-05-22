using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using NoSQL.Domain.Entities;
using Couchbase.Query;

namespace NoSQL.Infrastructure.Repositories
{
    public class CitaRepository
    {
        private readonly ICouchbaseCollection _collection;

        public CitaRepository(CouchbaseDbContext context)
        {
            _collection = context.GetCollection("citas");
        }

        public async Task<List<Cita>> GetAllAsync()
        {
            var query = "SELECT c.* FROM `citas` c";
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Cita>(query);
            return await result.Rows.ToListAsync();
        }

        public async Task<Cita?> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _collection.GetAsync(id.ToString());
                return result.ContentAs<Cita>();
            }
            catch (KeyValueException ex) when (ex.Message.Contains("Document not found"))
            {
                return null;
            }
        }

        public async Task AddAsync(Cita cita)
        {
            cita.Id = Guid.NewGuid();
            await _collection.InsertAsync(cita.Id.ToString(), cita);
        }

        public async Task UpdateAsync(Guid id, Cita cita)
        {
            await _collection.ReplaceAsync(id.ToString(), cita);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _collection.RemoveAsync(id.ToString());
        }

        public async Task<IEnumerable<Cita>> GetByPacienteEmailAsync(string pacienteEmail)
        {
            var query = $"SELECT c.* FROM `citas` c WHERE c.PacienteEmail = $pacienteEmail";
            var options = new Couchbase.Query.QueryOptions().Parameter("pacienteEmail", pacienteEmail);
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Cita>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetByOptometristaEmailAsync(string optometristaEmail)
        {
            var query = $"SELECT c.* FROM `citas` c WHERE c.OptometristaEmail = $optometristaEmail";
            var options = new Couchbase.Query.QueryOptions().Parameter("optometristaEmail", optometristaEmail);
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Cita>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetByPacienteIdAsync(Guid pacienteId)
        {
            var query = $"SELECT c.* FROM `citas` c WHERE c.PacienteId = $pacienteId";
            var options = new QueryOptions().Parameter("pacienteId", pacienteId.ToString());
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Cita>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetByOptometristaIdAsync(Guid optometristaId)
        {
            var query = $"SELECT c.* FROM `citas` c WHERE c.OptometristaId = $optometristaId";
            var options = new QueryOptions().Parameter("optometristaId", optometristaId.ToString());
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Cita>(query, options);
            return await result.Rows.ToListAsync();
        }
    }
}