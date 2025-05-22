using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Infrastructure.Repositories;
using NoSQL.Application.Services.Interfaces;

namespace NoSQL.Application.Services
{
    public class OptometristaService : IOptometristaService
    {
        private readonly OptometristaRepository _repository;

        public OptometristaService(OptometristaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Optometrista>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Optometrista?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Optometrista?> GetByEmailAsync(string email)
        {
            return await _repository.GetByEmailAsync(email);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Optometrista optometrista)
        {
            try
            {
                optometrista.Id = Guid.NewGuid();
                await _repository.AddAsync(optometrista);
                return (true, "Optometrista creado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear optometrista: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string email, Optometrista optometrista)
        {
            try
            {
                var existing = await _repository.GetByEmailAsync(email);
                if (existing == null)
                {
                    return (false, "Optometrista no encontrado");
                }

                optometrista.Id = existing.Id;
                await _repository.UpdateAsync(existing.Id, optometrista);
                return (true, "Optometrista actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar optometrista: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string email)
        {
            try
            {
                var existing = await _repository.GetByEmailAsync(email);
                if (existing == null)
                {
                    return (false, "Optometrista no encontrado");
                }

                await _repository.DeleteAsync(existing.Id);
                return (true, "Optometrista eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar optometrista: {ex.Message}");
            }
        }
    }
}