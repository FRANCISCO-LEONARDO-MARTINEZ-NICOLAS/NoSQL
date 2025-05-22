using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using NoSQL.Domain.Entities;
using Couchbase.Query;

namespace NoSQL.Infrastructure.Repositories
{
    public class PacienteRepository
    {
        private readonly ICouchbaseCollection _collection;

        public PacienteRepository(CouchbaseDbContext context)
        {
            _collection = context.GetCollection("pacientes");
        }

        public async Task<List<Paciente>> GetAllAsync()
        {
            var query = "SELECT p.* FROM `pacientes` p";
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Paciente>(query);
            return await result.Rows.ToListAsync();
        }

        public async Task<Paciente?> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _collection.GetAsync(id.ToString());
                return result.ContentAs<Paciente>();
            }
            catch (KeyValueException ex) when (ex.Message.Contains("Document not found"))
            {
                return null;
            }
        }

        public async Task AddAsync(Paciente paciente)
        {
            paciente.Id = Guid.NewGuid();
            await _collection.InsertAsync(paciente.Id.ToString(), paciente);
        }

        public async Task UpdateAsync(Guid id, Paciente paciente)
        {
            await _collection.ReplaceAsync(id.ToString(), paciente);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _collection.RemoveAsync(id.ToString());
        }

        public async Task<Paciente?> GetByEmailAsync(string email)
        {
            var query = $"SELECT p.* FROM `pacientes` p WHERE p.Correo = $email";
            var options = new Couchbase.Query.QueryOptions().Parameter("email", email);
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Paciente>(query, options);
            return await result.Rows.FirstOrDefaultAsync();
        }

        public async Task<Paciente?> GetByDniAsync(string dni)
        {
            var query = $"SELECT p.* FROM `pacientes` p WHERE p.Dni = $dni";
            var options = new QueryOptions().Parameter("dni", dni);
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Paciente>(query, options);
            return await result.Rows.FirstOrDefaultAsync();
        }
    }
}