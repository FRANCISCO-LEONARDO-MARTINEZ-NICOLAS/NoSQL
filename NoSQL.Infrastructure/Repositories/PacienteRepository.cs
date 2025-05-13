using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using NoSQL.Domain.Entities;

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
            // Implementación para obtener todos los pacientes
            var result = await _collection.GetAsync("pacientes");
            return result.ContentAs<List<Paciente>>();
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
    }
}