using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using NoSQL.Domain.Entities;
using Couchbase.Query;

namespace NoSQL.Infrastructure.Repositories
{
    public class ConsultaRepository
    {
        private readonly ICouchbaseCollection _collection;

        public ConsultaRepository(CouchbaseDbContext context)
        {
            _collection = context.GetCollection("consultas");
        }

        public async Task<List<Consulta>> GetAllAsync()
        {
            var query = "SELECT c.* FROM `consultas` c";
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Consulta>(query);
            return await result.Rows.ToListAsync();
        }

        public async Task<Consulta?> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _collection.GetAsync(id.ToString());
                return result.ContentAs<Consulta>();
            }
            catch (KeyValueException ex) when (ex.Message.Contains("Document not found"))
            {
                return null;
            }
        }

        public async Task AddAsync(Consulta consulta)
        {
            consulta.Id = Guid.NewGuid();
            await _collection.InsertAsync(consulta.Id.ToString(), consulta);
        }

        public async Task UpdateAsync(Guid id, Consulta consulta)
        {
            await _collection.ReplaceAsync(id.ToString(), consulta);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _collection.RemoveAsync(id.ToString());
        }

        public async Task<IEnumerable<Consulta>> GetByPacienteEmailAsync(string pacienteEmail)
        {
            var query = $"SELECT c.* FROM `consultas` c WHERE c.PacienteEmail = $pacienteEmail";
            var options = new QueryOptions().Parameter("pacienteEmail", pacienteEmail);
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Consulta>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Consulta>> GetByOptometristaEmailAsync(string optometristaEmail)
        {
            var query = $"SELECT c.* FROM `consultas` c WHERE c.OptometristaEmail = $optometristaEmail";
            var options = new QueryOptions().Parameter("optometristaEmail", optometristaEmail);
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Consulta>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Consulta>> GetByPacienteIdAsync(Guid pacienteId)
        {
            var query = $"SELECT c.* FROM `consultas` c WHERE c.PacienteId = $pacienteId";
            var options = new QueryOptions().Parameter("pacienteId", pacienteId.ToString());
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Consulta>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Consulta>> GetByOptometristaIdAsync(Guid optometristaId)
        {
            var query = $"SELECT c.* FROM `consultas` c WHERE c.OptometristaId = $optometristaId";
            var options = new QueryOptions().Parameter("optometristaId", optometristaId.ToString());
            var result = await _collection.Scope.Bucket.Cluster.QueryAsync<Consulta>(query, options);
            return await result.Rows.ToListAsync();
        }
    }
}