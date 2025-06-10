using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Application.Services
{
    public class OptometristaService : IOptometristaService
    {
        private readonly IOptometristaRepository _repository;

        public OptometristaService(IOptometristaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Optometrista>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Optometrista?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Optometrista optometrista)
        {
            try
            {
                optometrista.Id = Guid.NewGuid().ToString();
                await _repository.AddAsync(optometrista);
                return (true, "Optometrista creado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear optometrista: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string id, Optometrista optometrista)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Optometrista no encontrado");
                optometrista.Id = id;
                await _repository.UpdateAsync(id, optometrista);
                return (true, "Optometrista actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar optometrista: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Optometrista no encontrado");
                await _repository.DeleteAsync(id);
                return (true, "Optometrista eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar optometrista: {ex.Message}");
            }
        }
    }
}