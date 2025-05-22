using NoSQL.Domain.Entities;

namespace NoSQL.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> LoginAsync(string email, string password, string role);
        Task<(bool Success, string Message)> RegisterAsync(string email, string password, string role);
    }
} 