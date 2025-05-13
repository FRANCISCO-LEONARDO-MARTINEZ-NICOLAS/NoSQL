using NoSQL.Domain.Entities;
using NoSQL.Infrastructure.Repositories;

namespace NoSQL.Application.Services
{
    public class ProductoService
    {
        private readonly ProductoRepository _repository;

        public ProductoService(ProductoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Producto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Producto?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Producto producto)
        {
            await _repository.AddAsync(producto);
        }

        public async Task UpdateAsync(Guid id, Producto producto)
        {
            await _repository.UpdateAsync(id, producto);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}