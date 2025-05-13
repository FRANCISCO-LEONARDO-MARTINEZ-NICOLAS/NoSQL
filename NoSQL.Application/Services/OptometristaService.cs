using Couchbase.KeyValue;
using NoSQL.Domain.Entities;
using NoSQL.Infrastructure.Repositories;
using NoSQL.Infrastructure;

namespace NoSQL.Application.Services
{
    public class OptometristaService
    {
        private readonly ICouchbaseCollection _collection;
        private readonly OptometristaRepository _repository;

        public OptometristaService(CouchbaseDbContext context, OptometristaRepository repository)
        {
            _collection = context.GetCollection("optometristas");
            _repository = repository;
        }

        public async Task<List<Optometrista>> GetAllAsync()
        {
            var result = await _collection.GetAsync("optometristas");
            return result.ContentAs<List<Optometrista>>();
        }

        public async Task<Optometrista?> GetByIdAsync(Guid id)
        {
            var optometrista = await _repository.GetByIdAsync(id);
            return optometrista ?? null; // Manejo explícito de nulos
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