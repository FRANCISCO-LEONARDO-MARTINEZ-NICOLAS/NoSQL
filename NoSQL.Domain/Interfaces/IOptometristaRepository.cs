using NoSQL.Domain.Entities;

namespace NoSQL.Domain.Interfaces
{
    public interface IOptometristaRepository
    {
        Task<IEnumerable<Optometrista>> GetAllAsync();
        Task<Optometrista?> GetByIdAsync(string id);
        Task AddAsync(Optometrista optometrista);
        Task UpdateAsync(string id, Optometrista optometrista);
        Task DeleteAsync(string id);
        Task<Optometrista?> GetByEmailAsync(string correo);
    }
}