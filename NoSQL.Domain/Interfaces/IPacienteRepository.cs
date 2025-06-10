using NoSQL.Domain.Entities;

namespace NoSQL.Domain.Interfaces
{
    public interface IPacienteRepository
    {
        Task<IEnumerable<Paciente>> GetAllAsync();
        Task<Paciente?> GetByIdAsync(string id);
        Task<Paciente?> GetByEmailAsync(string correo);
        Task<Paciente?> GetByDniAsync(string dni);
        Task AddAsync(Paciente paciente);
        Task UpdateAsync(string id, Paciente paciente);
        Task DeleteAsync(string id);
    }
} 