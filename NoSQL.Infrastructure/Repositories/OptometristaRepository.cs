using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using NoSQL.Domain.Entities;

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
            // Aquí puedes implementar una consulta para obtener todos los optometristas
            // Si estás usando Couchbase, necesitarás una consulta N1QL o similar.
            throw new NotImplementedException("Consulta para obtener todos los optometristas no implementada.");
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
    }
}