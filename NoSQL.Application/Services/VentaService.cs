using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Application.Services
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly INotificacionService _notificacionService;

        public VentaService(IVentaRepository ventaRepository, INotificacionService notificacionService)
        {
            _ventaRepository = ventaRepository;
            _notificacionService = notificacionService;
        }

        public async Task<IEnumerable<Venta>> GetAllAsync()
        {
            return await _ventaRepository.GetAllAsync();
        }

        public async Task<Venta?> GetByIdAsync(string id)
        {
            return await _ventaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Venta>> GetByPacienteIdAsync(string pacienteId)
        {
            return await _ventaRepository.GetByPacienteIdAsync(pacienteId);
        }

        public async Task<IEnumerable<Venta>> GetByOptometristaIdAsync(string optometristaId)
        {
            return await _ventaRepository.GetByOptometristaIdAsync(optometristaId);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Venta venta)
        {
            try
            {
                venta.Id = Guid.NewGuid().ToString();
                venta.Fecha = DateTime.UtcNow;
                venta.CalcularMontoTotal();
                await _ventaRepository.CreateAsync(venta);
                return (true, "Venta creada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear venta: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string id, Venta venta)
        {
            try
            {
                var existing = await _ventaRepository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Venta no encontrada");
                venta.Id = id;
                venta.CalcularMontoTotal();
                await _ventaRepository.UpdateAsync(venta);
                return (true, "Venta actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar venta: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            try
            {
                var existing = await _ventaRepository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Venta no encontrada");
                await _ventaRepository.DeleteAsync(id);
                return (true, "Venta eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar venta: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message, decimal Total)> CalcularTotalVentaAsync(string ventaId)
        {
            var venta = await GetByIdAsync(ventaId);
            if (venta == null)
                return (false, $"Venta con ID {ventaId} no encontrada", 0);
            venta.CalcularMontoTotal();
            return (true, "Total calculado exitosamente", venta.MontoTotal);
        }

        public async Task<(bool Success, string Message)> AgregarProductoAsync(string ventaId, ProductoVenta producto)
        {
            var venta = await GetByIdAsync(ventaId);
            if (venta == null)
                return (false, "Venta no encontrada");
            venta.Productos.Add(producto);
            venta.CalcularMontoTotal();
            await _ventaRepository.UpdateAsync(venta);
            return (true, "Producto agregado exitosamente");
        }

        public async Task<(bool Success, string Message)> ActualizarEstadoAsync(string ventaId, string nuevoEstado)
        {
            var venta = await GetByIdAsync(ventaId);
            if (venta == null)
                return (false, "Venta no encontrada");
            venta.Estado = nuevoEstado;
            await _ventaRepository.UpdateAsync(venta);
            if (nuevoEstado.Equals("Listo", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var producto in venta.Productos)
                {
                    await _notificacionService.CrearNotificacionProductoListoAsync(venta.PacienteId, producto.Nombre);
                }
            }
            return (true, "Estado actualizado exitosamente");
        }
    }
} 