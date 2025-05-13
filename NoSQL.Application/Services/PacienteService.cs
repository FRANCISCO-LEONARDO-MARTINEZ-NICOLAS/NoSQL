using NoSQL.Domain.Entities;
using NoSQL.Infrastructure.Repositories;

namespace NoSQL.Application.Services
{
    public class PacienteService
    {
        private readonly PacienteRepository _repository;

        public PacienteService(PacienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Paciente>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Paciente?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Paciente paciente)
        {
            await _repository.AddAsync(paciente);
        }

        public async Task UpdateAsync(Guid id, Paciente paciente)
        {
            await _repository.UpdateAsync(id, paciente);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}