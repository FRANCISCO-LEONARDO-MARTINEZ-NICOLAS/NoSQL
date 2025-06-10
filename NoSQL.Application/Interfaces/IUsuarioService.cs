using NoSQL.Domain.Entities;

namespace NoSQL.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(string id);
        Task<Usuario?> GetByEmailAsync(string correo);
        Task<(bool Success, string Message)> CreateAsync(Usuario usuario);
        Task<(bool Success, string Message)> UpdateAsync(string id, Usuario usuario);
    }
} 