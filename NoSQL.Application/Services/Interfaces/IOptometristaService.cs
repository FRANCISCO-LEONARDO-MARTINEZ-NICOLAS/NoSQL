using NoSQL.Domain.Entities;

namespace NoSQL.Application.Services.Interfaces
{
    public interface IOptometristaService
    {
        Task<IEnumerable<Optometrista>> GetAllAsync();
        Task<Optometrista?> GetByEmailAsync(string email);
        Task<(bool Success, string Message)> CreateAsync(Optometrista optometrista);
        Task<(bool Success, string Message)> UpdateAsync(string email, Optometrista optometrista);
        Task<(bool Success, string Message)> DeleteAsync(string email);
    }
} 