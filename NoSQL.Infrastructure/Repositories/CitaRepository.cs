using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using NoSQL.Domain.Entities;

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
            var result = await _collection.GetAsync("citas");
            return result.ContentAs<List<Cita>>();
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
    }
}