using NoSQL.Domain.Entities;

namespace NoSQL.Application.Interfaces
{
    public interface IVentaService
    {
        Task<IEnumerable<Venta>> GetAllAsync();
        Task<Venta?> GetByIdAsync(Guid id);
        Task<IEnumerable<Venta>> GetByPacienteIdAsync(Guid pacienteId);
        Task<IEnumerable<Venta>> GetByOptometristaIdAsync(Guid optometristaId);
        Task<Venta> CreateAsync(Venta venta);
        Task UpdateAsync(Venta venta);
        Task DeleteAsync(Guid id);
        
        // Métodos específicos de negocio
        Task<decimal> CalcularTotalVentaAsync(Guid ventaId);
        Task<bool> AgregarProductoAsync(Guid ventaId, ProductoVenta producto);
        Task<bool> ActualizarEstadoAsync(Guid ventaId, string nuevoEstado);
    }
} 