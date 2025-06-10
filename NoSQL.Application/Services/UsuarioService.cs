using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task<Usuario?> GetByIdAsync(string id)
        {
            return await _usuarioRepository.GetByIdAsync(id);
        }

        public async Task<Usuario?> GetByEmailAsync(string correo)
        {
            return await _usuarioRepository.GetByEmailAsync(correo);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Usuario usuario)
        {
            try
            {
                usuario.Id = Guid.NewGuid().ToString();
                usuario.FechaCreacion = DateTime.UtcNow;
                usuario.UltimoAcceso = DateTime.UtcNow;
                usuario.Activo = true;
                usuario.type = "Usuario";
                usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash ?? "123456");
                await _usuarioRepository.AddAsync(usuario);
                return (true, "Usuario creado correctamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear usuario: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string id, Usuario usuario)
        {
            try
            {
                usuario.Id = id;
                await _usuarioRepository.UpdateAsync(id, usuario);
                return (true, "Usuario actualizado correctamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar usuario: {ex.Message}");
            }
        }
    }
}