using NoSQL.Domain.Entities;

namespace NoSQL.Application.Services.Interfaces
{
    public interface ICitaService
    {
        Task<IEnumerable<Cita>> GetAllAsync();
        Task<IEnumerable<Cita>> GetByPacienteEmailAsync(string pacienteEmail);
        Task<IEnumerable<Cita>> GetByOptometristaEmailAsync(string optometristaEmail);
        Task<(bool Success, string Message)> CreateAsync(Cita cita);
        Task<(bool Success, string Message)> UpdateAsync(Guid id, Cita cita);
        Task<(bool Success, string Message)> DeleteAsync(Guid id);
    }
} 