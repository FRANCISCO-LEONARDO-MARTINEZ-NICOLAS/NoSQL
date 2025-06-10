using NoSQL.Domain.Entities;

namespace NoSQL.Domain.Interfaces
{
    public interface ICitaRepository
    {
        Task<IEnumerable<Cita>> GetAllAsync();
        Task<Cita?> GetByIdAsync(string id);
        Task<IEnumerable<Cita>> GetByPacienteIdAsync(string id);
        Task<IEnumerable<Cita>> GetByOptometristaIdAsync(string id);
        Task<IEnumerable<Cita>> GetByPacienteEmailAsync(string email);
        Task<IEnumerable<Cita>> GetByOptometristaEmailAsync(string email);
        Task AddAsync(Cita cita);
        Task UpdateAsync(string id, Cita cita);
        Task DeleteAsync(string id);
    }
} 