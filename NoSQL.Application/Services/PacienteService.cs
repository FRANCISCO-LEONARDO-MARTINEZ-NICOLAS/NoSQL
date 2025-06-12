using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Paciente?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Paciente?> GetBycorreoAsync(string correo)
        {
            return await _repository.GetBycorreoAsync(correo);
        }

        public async Task<Paciente?> GetByDniAsync(string dni)
        {
            return await _repository.GetByDniAsync(dni);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Paciente paciente)
        {
            try
            {
                paciente.Id = Guid.NewGuid().ToString();
                await _repository.AddAsync(paciente);
                return (true, "Paciente creado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear paciente: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string id, Paciente paciente)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Paciente no encontrado");

                paciente.Id = id;
                await _repository.UpdateAsync(id, paciente);
                return (true, "Paciente actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar paciente: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Paciente no encontrado");

                await _repository.DeleteAsync(id);
                return (true, "Paciente eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar paciente: {ex.Message}");
            }
        }
    }
}