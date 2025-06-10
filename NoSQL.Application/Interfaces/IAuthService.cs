using NoSQL.Domain.Entities;

namespace NoSQL.Application.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, Usuario? User, string? Token)> LoginAsync(string email, string password, string rol);
        Task<(bool Success, string Message)> RegisterAsync(Usuario usuario, string password);
        Task<(bool Success, string Message)> ChangePasswordAsync(string email, string currentPassword, string newPassword);
        Task<(bool Success, string Message)> ResetPasswordAsync(string email);
        Task<(bool Success, string Message)> ValidateTokenAsync(string token);
    }
}