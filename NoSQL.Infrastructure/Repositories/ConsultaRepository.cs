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
        private readonly string _bucketName;

        public ConsultaRepository(CouchbaseDbContext context)
        {
            _collection = context.Bucket.DefaultCollection();
            _cluster = context.Cluster;
            _bucketName = context.BucketName;
        }

        public async Task<IEnumerable<Consulta>> GetAllAsync()
        {
            var query = $"SELECT c.* FROM `{_bucketName}` c WHERE c.type = 'consulta'";
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

        public async Task<IEnumerable<Consulta>> GetByPacientecorreoAsync(string correo)
        {
            var query = $"SELECT c.* FROM `{_bucketName}` c WHERE c.type = 'consulta' AND c.Pacientecorreo = $correo";
            var options = new QueryOptions().Parameter("correo", correo);
            var result = await _cluster.QueryAsync<Consulta>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Consulta>> GetByOptometristacorreoAsync(string correo)
        {
            var query = $"SELECT c.* FROM `{_bucketName}` c WHERE c.type = 'consulta' AND c.Optometristacorreo = $correo";
            var options = new QueryOptions().Parameter("correo", correo);
            var result = await _cluster.QueryAsync<Consulta>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Consulta>> GetByPacienteIdAsync(string id)
        {
            var query = $"SELECT c.* FROM `{_bucketName}` c WHERE c.type = 'consulta' AND c.PacienteId = $id";
            var options = new QueryOptions().Parameter("id", id);
            var result = await _cluster.QueryAsync<Consulta>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task<IEnumerable<Consulta>> GetByOptometristaIdAsync(string id)
        {
            var query = $"SELECT c.* FROM `{_bucketName}` c WHERE c.type = 'consulta' AND c.OptometristaId = $id";
            var options = new QueryOptions().Parameter("id", id);
            var result = await _cluster.QueryAsync<Consulta>(query, options);
            return await result.Rows.ToListAsync();
        }

        public async Task AddAsync(Consulta consulta)
        {
            consulta.type = "consulta";
            await _collection.InsertAsync(consulta.Id, consulta);
        }

        public async Task UpdateAsync(string id, Consulta consulta)
        {
            consulta.type = "consulta";
            await _collection.ReplaceAsync(id, consulta);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.RemoveAsync(id);
        }
    }
}