using NoSQL.Domain.Entities;

namespace NoSQL.Domain.Interfaces
{
    public interface IConsultaRepository
    {
        Task<IEnumerable<Consulta>> GetAllAsync();
        Task<Consulta?> GetByIdAsync(string id);
        Task<IEnumerable<Consulta>> GetByPacientecorreoAsync(string correo);
        Task<IEnumerable<Consulta>> GetByOptometristacorreoAsync(string correo);
        Task<IEnumerable<Consulta>> GetByPacienteIdAsync(string id);
        Task<IEnumerable<Consulta>> GetByOptometristaIdAsync(string id);
        Task AddAsync(Consulta consulta);
        Task UpdateAsync(string id, Consulta consulta);
        Task DeleteAsync(string id);
    }
} 