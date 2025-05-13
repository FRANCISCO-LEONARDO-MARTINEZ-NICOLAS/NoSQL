using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using NoSQL.Domain.Entities;

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
            var result = await _collection.GetAsync("consultas");
            return result.ContentAs<List<Consulta>>();
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
    }
}