using Couchbase;
using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using Couchbase.Query;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Infrastructure.Repositories
{
    public class CitaRepository : ICitaRepository
    {
        private readonly ICouchbaseCollection _collection;
        private readonly ICluster _cluster;

        public CitaRepository(CouchbaseDbContext context)
        {
            _collection = context.Bucket.DefaultCollection();
            _cluster = context.Cluster;
        }

        public async Task<IEnumerable<Cita>> GetAllAsync()
        {
            var query = "SELECT c.* FROM `citas` c";
            var result = await _cluster.QueryAsync<Cita>(query);
            return await result.Rows.ToListAsync();
        }

        public async Task<Cita?> GetByIdAsync(string id)
        {
            try
            {
                var result = await _collection.GetAsync(id);
                return result.ContentAs<Cita>();
            }
            catch (DocumentNotFoundException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Cita>> GetByPacienteIdAsync(string id)
        {
            var query = "SELECT c.* FROM `citas` c WHERE c.PacienteId = $id";
            var options = new QueryOptions().Parameter("id", id);
            var result = await _cluster.QueryAsync<Cita>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetByOptometristaIdAsync(string id)
        {
            var query = "SELECT c.* FROM `citas` c WHERE c.OptometristaId = $id";
            var options = new QueryOptions().Parameter("id", id);
            var result = await _cluster.QueryAsync<Cita>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetByPacienteEmailAsync(string email)
        {
            var query = "SELECT c.* FROM `citas` c WHERE c.PacienteEmail = $email";
            var options = new QueryOptions().Parameter("email", email);
            var result = await _cluster.QueryAsync<Cita>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Cita>> GetByOptometristaEmailAsync(string email)
        {
            var query = "SELECT c.* FROM `citas` c WHERE c.OptometristaEmail = $email";
            var options = new QueryOptions().Parameter("email", email);
            var result = await _cluster.QueryAsync<Cita>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task AddAsync(Cita cita)
        {
            await _collection.InsertAsync(cita.Id, cita);
        }

        public async Task UpdateAsync(string id, Cita cita)
        {
            await _collection.ReplaceAsync(id, cita);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.RemoveAsync(id);
        }
    }
}