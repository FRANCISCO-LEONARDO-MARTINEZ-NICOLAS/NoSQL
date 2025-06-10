using Couchbase;
using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using Couchbase.Query;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Infrastructure.Repositories
{
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly ICouchbaseCollection _collection;
        private readonly ICluster _cluster;

        public ConsultaRepository(CouchbaseDbContext context)
        {
            _collection = context.Bucket.DefaultCollection();
            _cluster = context.Cluster;
        }

        public async Task<IEnumerable<Consulta>> GetAllAsync()
        {
            var query = "SELECT c.* FROM `consultas` c";
            var result = await _cluster.QueryAsync<Consulta>(query);
            return await result.Rows.ToListAsync();
        }

        public async Task<Consulta?> GetByIdAsync(string id)
        {
            try
            {
                var result = await _collection.GetAsync(id);
                return result.ContentAs<Consulta>();
            }
            catch (DocumentNotFoundException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Consulta>> GetByPacienteEmailAsync(string email)
        {
            var query = "SELECT c.* FROM `consultas` c WHERE c.PacienteEmail = $email";
            var options = new QueryOptions().Parameter("email", email);
            var result = await _cluster.QueryAsync<Consulta>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Consulta>> GetByOptometristaEmailAsync(string email)
        {
            var query = "SELECT c.* FROM `consultas` c WHERE c.OptometristaEmail = $email";
            var options = new QueryOptions().Parameter("email", email);
            var result = await _cluster.QueryAsync<Consulta>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Consulta>> GetByPacienteIdAsync(string id)
        {
            var query = "SELECT c.* FROM `consultas` c WHERE c.PacienteId = $id";
            var options = new QueryOptions().Parameter("id", id);
            var result = await _cluster.QueryAsync<Consulta>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Consulta>> GetByOptometristaIdAsync(string id)
        {
            var query = "SELECT c.* FROM `consultas` c WHERE c.OptometristaId = $id";
            var options = new QueryOptions().Parameter("id", id);
            var result = await _cluster.QueryAsync<Consulta>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task AddAsync(Consulta consulta)
        {
            await _collection.InsertAsync(consulta.Id, consulta);
        }

        public async Task UpdateAsync(string id, Consulta consulta)
        {
            await _collection.ReplaceAsync(id, consulta);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.RemoveAsync(id);
        }
    }
}