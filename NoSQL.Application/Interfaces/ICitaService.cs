using NoSQL.Domain.Entities;

namespace NoSQL.Application.Interfaces
{
    public interface ICitaService
    {
        Task<IEnumerable<Cita>> GetAllAsync();
        Task<Cita?> GetByIdAsync(string id);
        Task<IEnumerable<Cita>> GetByPacienteIdAsync(string pacienteId);
        Task<IEnumerable<Cita>> GetByOptometristaIdAsync(string optometristaId);
        Task<(bool Success, string Message)> CreateAsync(Cita cita);
        Task<(bool Success, string Message)> UpdateAsync(string id, Cita cita);
        Task<(bool Success, string Message)> DeleteAsync(string id);
    }
} 