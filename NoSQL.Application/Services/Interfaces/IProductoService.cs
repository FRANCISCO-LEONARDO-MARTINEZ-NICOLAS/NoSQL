using NoSQL.Domain.Entities;

namespace NoSQL.Application.Services.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> GetAllAsync();
        Task<IEnumerable<Producto>> GetByPacienteEmailAsync(string pacienteEmail);
        Task<IEnumerable<Producto>> GetByOptometristaEmailAsync(string optometristaEmail);
        Task<(bool Success, string Message)> CreateAsync(Producto producto);
        Task<(bool Success, string Message)> UpdateAsync(Guid id, Producto producto);
        Task<(bool Success, string Message)> UpdateStatusAsync(string id, string nuevoEstado);
        Task<(bool Success, string Message)> DeleteAsync(Guid id);
    }
} 