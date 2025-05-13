using NoSQL.Domain.Entities;
using NoSQL.Infrastructure.Repositories;

namespace NoSQL.Application.Services
{
    public class ConsultaService
    {
        private readonly ConsultaRepository _repository;

        public ConsultaService(ConsultaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Consulta>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Consulta?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Consulta consulta)
        {
            await _repository.AddAsync(consulta);
        }

        public async Task UpdateAsync(Guid id, Consulta consulta)
        {
            await _repository.UpdateAsync(id, consulta);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}