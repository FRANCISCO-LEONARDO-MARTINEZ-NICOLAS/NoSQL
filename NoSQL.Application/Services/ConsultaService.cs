using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Infrastructure.Repositories;
using NoSQL.Application.Services.Interfaces;

namespace NoSQL.Application.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly ConsultaRepository _repository;

        public ConsultaService(ConsultaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Consulta>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Consulta?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Consulta>> GetByPacienteEmailAsync(string email)
        {
            return await _repository.GetByPacienteEmailAsync(email);
        }

        public async Task<IEnumerable<Consulta>> GetByOptometristaEmailAsync(string email)
        {
            return await _repository.GetByOptometristaEmailAsync(email);
        }

        public async Task<IEnumerable<Consulta>> GetByPacienteIdAsync(Guid id)
        {
            return await _repository.GetByPacienteIdAsync(id);
        }

        public async Task<IEnumerable<Consulta>> GetByOptometristaIdAsync(Guid id)
        {
            return await _repository.GetByOptometristaIdAsync(id);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Consulta consulta)
        {
            try
            {
                consulta.Id = Guid.NewGuid();
                consulta.Fecha = DateTime.UtcNow;
                await _repository.AddAsync(consulta);
                return (true, "Consulta creada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear consulta: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(Guid id, Consulta consulta)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return (false, "Consulta no encontrada");
            consulta.Id = id;
            await _repository.UpdateAsync(id, consulta);
            return (true, "Consulta actualizada exitosamente");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return (false, "Consulta no encontrada");
            await _repository.DeleteAsync(id);
            return (true, "Consulta eliminada exitosamente");
        }
    }
}