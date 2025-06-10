using NoSQL.Domain.Entities;

namespace NoSQL.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(string id);
        Task<Usuario?> GetByEmailAsync(string correo);
        Task<bool> ExistsByEmailAsync(string correo);
        Task AddAsync(Usuario usuario);
        Task UpdateAsync(string id, Usuario usuario);
        Task DeleteAsync(string id);
    }
} 