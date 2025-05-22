using NoSQL.Domain.Entities;

namespace NoSQL.Application.Services.Interfaces
{
    public interface IPacienteService
    {
        Task<IEnumerable<Paciente>> GetAllAsync();
        Task<Paciente?> GetByEmailAsync(string email);
        Task<(bool Success, string Message)> CreateAsync(Paciente paciente);
        Task<(bool Success, string Message)> UpdateAsync(string email, Paciente paciente);
        Task<(bool Success, string Message)> DeleteAsync(string email);
    }
} 