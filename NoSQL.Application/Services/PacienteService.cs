using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Infrastructure.Repositories;
using NoSQL.Application.Services.Interfaces;

namespace NoSQL.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly PacienteRepository _repository;

        public PacienteService(PacienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Paciente?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Paciente?> GetByEmailAsync(string email)
        {
            return await _repository.GetByEmailAsync(email);
        }

        public async Task<Paciente?> GetByDniAsync(string dni)
        {
            return await _repository.GetByDniAsync(dni);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Paciente paciente)
        {
            try
            {
                paciente.Id = Guid.NewGuid();
                await _repository.AddAsync(paciente);
                return (true, "Paciente creado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear paciente: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string email, Paciente paciente)
        {
            var existing = await _repository.GetByEmailAsync(email);
            if (existing == null)
                return (false, "Paciente no encontrado");
            paciente.Id = existing.Id;
            await _repository.UpdateAsync(existing.Id, paciente);
            return (true, "Paciente actualizado exitosamente");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string email)
        {
            var existing = await _repository.GetByEmailAsync(email);
            if (existing == null)
                return (false, "Paciente no encontrado");
            await _repository.DeleteAsync(existing.Id);
            return (true, "Paciente eliminado exitosamente");
        }
    }
}