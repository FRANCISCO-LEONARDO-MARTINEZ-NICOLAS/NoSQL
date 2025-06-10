using NoSQL.Domain.Entities;

namespace NoSQL.Application.Interfaces
{
    public interface IOptometristaService
    {
        Task<IEnumerable<Optometrista>> GetAllAsync();
        Task<Optometrista?> GetByIdAsync(string id);
        Task<(bool Success, string Message)> CreateAsync(Optometrista optometrista);
        Task<(bool Success, string Message)> UpdateAsync(string id, Optometrista optometrista);
        Task<(bool Success, string Message)> DeleteAsync(string id);
    }
}