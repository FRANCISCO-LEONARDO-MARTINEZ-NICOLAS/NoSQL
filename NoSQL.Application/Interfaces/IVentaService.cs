using NoSQL.Domain.Entities;

namespace NoSQL.Application.Interfaces
{
    public interface IVentaService
    {
        Task<IEnumerable<Venta>> GetAllAsync();
        Task<Venta?> GetByIdAsync(string id);
        Task<IEnumerable<Venta>> GetByPacienteIdAsync(string pacienteId);
        Task<IEnumerable<Venta>> GetByOptometristaIdAsync(string optometristaId);
        Task<(bool Success, string Message)> CreateAsync(Venta venta);
        Task<(bool Success, string Message)> UpdateAsync(string id, Venta venta);
        Task<(bool Success, string Message)> DeleteAsync(string id);
        
        // Métodos específicos de negocio
        Task<(bool Success, string Message, decimal Total)> CalcularTotalVentaAsync(string ventaId);
        Task<(bool Success, string Message)> AgregarProductoAsync(string ventaId, ProductoVenta producto);
        Task<(bool Success, string Message)> ActualizarEstadoAsync(string ventaId, string nuevoEstado);
    }
} 