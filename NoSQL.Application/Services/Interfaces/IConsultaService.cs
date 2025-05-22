using NoSQL.Domain.Entities;

namespace NoSQL.Application.Services.Interfaces
{
    public interface IConsultaService
    {
        Task<IEnumerable<Consulta>> GetAllAsync();
        Task<IEnumerable<Consulta>> GetByPacienteEmailAsync(string pacienteEmail);
        Task<IEnumerable<Consulta>> GetByOptometristaEmailAsync(string optometristaEmail);
        Task<(bool Success, string Message)> CreateAsync(Consulta consulta);
        Task<(bool Success, string Message)> UpdateAsync(Guid id, Consulta consulta);
        Task<(bool Success, string Message)> DeleteAsync(Guid id);
    }
} 