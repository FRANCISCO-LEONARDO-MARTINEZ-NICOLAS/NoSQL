using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Application.Services
{
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _repository;

        public CitaService(ICitaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Cita>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Cita?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Cita>> GetByPacienteIdAsync(string pacienteId)
        {
            return await _repository.GetByPacienteIdAsync(pacienteId);
        }

        public async Task<IEnumerable<Cita>> GetByOptometristaIdAsync(string optometristaId)
        {
            return await _repository.GetByOptometristaIdAsync(optometristaId);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Cita cita)
        {
            try
            {
                cita.Id = Guid.NewGuid().ToString();
                await _repository.AddAsync(cita);
                return (true, "Cita creada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear cita: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string id, Cita cita)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Cita no encontrada");
                cita.Id = id;
                await _repository.UpdateAsync(id, cita);
                return (true, "Cita actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar cita: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Cita no encontrada");
                await _repository.DeleteAsync(id);
                return (true, "Cita eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar cita: {ex.Message}");
            }
        }
    }
}