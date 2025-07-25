using NoSQL.Domain.Entities;

namespace NoSQL.Application.Interfaces
{
    public interface IConsultaService
    {
        Task<IEnumerable<Consulta>> GetAllAsync();
        Task<Consulta?> GetByIdAsync(string id);
        Task<IEnumerable<Consulta>> GetByPacientecorreoAsync(string correo);
        Task<IEnumerable<Consulta>> GetByOptometristacorreoAsync(string correo);
        Task<IEnumerable<Consulta>> GetByPacienteIdAsync(string id);
        Task<IEnumerable<Consulta>> GetByOptometristaIdAsync(string id);
        Task<IEnumerable<Consulta>> GetByFechaAsync(DateTime fecha);
        Task<(bool Success, string Message)> CreateAsync(Consulta consulta);
        Task<(bool Success, string Message)> UpdateAsync(string id, Consulta consulta);
        Task<(bool Success, string Message)> DeleteAsync(string id);
    }
} 