using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Infrastructure.Repositories;
using NoSQL.Application.Services.Interfaces;

namespace NoSQL.Application.Services
{
    public class CitaService : ICitaService
    {
        private readonly CitaRepository _repository;

        public CitaService(CitaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Cita>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Cita?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Cita>> GetByPacienteEmailAsync(string email)
        {
            return await _repository.GetByPacienteEmailAsync(email);
        }

        public async Task<IEnumerable<Cita>> GetByOptometristaEmailAsync(string email)
        {
            return await _repository.GetByOptometristaEmailAsync(email);
        }

        public async Task<IEnumerable<Cita>> GetByPacienteIdAsync(Guid pacienteId)
        {
            return await _repository.GetByPacienteIdAsync(pacienteId);
        }

        public async Task<IEnumerable<Cita>> GetByOptometristaIdAsync(Guid optometristaId)
        {
            return await _repository.GetByOptometristaIdAsync(optometristaId);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Cita cita)
        {
            try
            {
                cita.Id = Guid.NewGuid();
                await _repository.AddAsync(cita);
                return (true, "Cita creada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear cita: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(Guid id, Cita cita)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return (false, "Cita no encontrada");
            cita.Id = id;
            await _repository.UpdateAsync(id, cita);
            return (true, "Cita actualizada exitosamente");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return (false, "Cita no encontrada");
            await _repository.DeleteAsync(id);
            return (true, "Cita eliminada exitosamente");
        }
    }
}