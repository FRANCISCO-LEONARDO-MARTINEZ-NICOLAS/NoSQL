using NoSQL.Domain.Entities;

namespace NoSQL.Application.Interfaces
{
    public interface IPacienteService
    {
        Task<IEnumerable<Paciente>> GetAllAsync();
        Task<Paciente?> GetByIdAsync(string id);
        Task<Paciente?> GetByEmailAsync(string correo);
        Task<Paciente?> GetByDniAsync(string dni);
        Task<(bool Success, string Message)> CreateAsync(Paciente paciente);
        Task<(bool Success, string Message)> UpdateAsync(string id, Paciente paciente);
        Task<(bool Success, string Message)> DeleteAsync(string id);
    }
} 