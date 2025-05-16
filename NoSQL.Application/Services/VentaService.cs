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

        public async Task<Venta?> GetByIdAsync(Guid id)
        {
            return await _ventaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Venta>> GetByPacienteIdAsync(Guid pacienteId)
        {
            return await _ventaRepository.GetByPacienteIdAsync(pacienteId);
        }

        public async Task<IEnumerable<Venta>> GetByOptometristaIdAsync(Guid optometristaId)
        {
            return await _ventaRepository.GetByOptometristaIdAsync(optometristaId);
        }

        public async Task<Venta> CreateAsync(Venta venta)
        {
            venta.Id = Guid.NewGuid();
            venta.Fecha = DateTime.UtcNow;
            venta.CalcularMontoTotal();
            
            await _ventaRepository.CreateAsync(venta);
            return venta;
        }

        public async Task UpdateAsync(Venta venta)
        {
            venta.CalcularMontoTotal();
            await _ventaRepository.UpdateAsync(venta);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _ventaRepository.DeleteAsync(id);
        }

        public async Task<decimal> CalcularTotalVentaAsync(Guid ventaId)
        {
            var venta = await GetByIdAsync(ventaId);
            if (venta == null)
                throw new KeyNotFoundException($"Venta con ID {ventaId} no encontrada");

            venta.CalcularMontoTotal();
            return venta.MontoTotal;
        }

        public async Task<bool> AgregarProductoAsync(Guid ventaId, ProductoVenta producto)
        {
            var venta = await GetByIdAsync(ventaId);
            if (venta == null)
                return false;

            venta.Productos.Add(producto);
            venta.CalcularMontoTotal();
            await UpdateAsync(venta);
            return true;
        }

        public async Task<bool> ActualizarEstadoAsync(Guid ventaId, string nuevoEstado)
        {
            var venta = await GetByIdAsync(ventaId);
            if (venta == null)
                return false;

            venta.Estado = nuevoEstado;
            await UpdateAsync(venta);

            // Si el estado es "Listo", enviar notificación al paciente
            if (nuevoEstado.Equals("Listo", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var producto in venta.Productos)
                {
                    await _notificacionService.CrearNotificacionProductoListoAsync(venta.PacienteId, producto.Nombre);
                }
            }

            return true;
        }
    }
} 