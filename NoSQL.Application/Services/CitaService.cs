using NoSQL.Domain.Entities;
using NoSQL.Infrastructure.Repositories;

namespace NoSQL.Application.Services
{
    public class CitaService
    {
        private readonly CitaRepository _repository;

        public CitaService(CitaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Cita>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Cita?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Cita cita)
        {
            await _repository.AddAsync(cita);
        }

        public async Task UpdateAsync(Guid id, Cita cita)
        {
            await _repository.UpdateAsync(id, cita);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}