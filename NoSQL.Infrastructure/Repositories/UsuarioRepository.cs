using Couchbase;
using Couchbase.KeyValue;
using Microsoft.Extensions.Configuration;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;
using System.Text.Json;

namespace NoSQL.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly CouchbaseDbContext _context;
        private const string CollectionName = "usuarios";

        public UsuarioRepository(CouchbaseDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByIdAsync(Guid id)
        {
            var collection = _context.GetCollection(CollectionName);
            var result = await collection.GetAsync(id.ToString());
            return result.ContentAs<Usuario>();
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            var query = $"SELECT u.* FROM `{_context.BucketName}` u WHERE u.email = $email";
            var result = await _context.Cluster.QueryAsync<Usuario>(query, options => 
                options.Parameter("email", email));
            return await result.Rows.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var query = $"SELECT u.* FROM `{_context.BucketName}` u WHERE u.type = 'usuario'";
            var result = await _context.Cluster.QueryAsync<Usuario>(query);
            return await result.Rows.ToListAsync();
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            var collection = _context.GetCollection(CollectionName);
            usuario.Id = Guid.NewGuid();
            usuario.type = "usuario";
            await collection.InsertAsync(usuario.Id.ToString(), usuario);
            return usuario;
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            var collection = _context.GetCollection(CollectionName);
            usuario.type = "usuario";
            await collection.UpsertAsync(usuario.Id.ToString(), usuario);
        }

        public async Task DeleteAsync(Guid id)
        {
            var collection = _context.GetCollection(CollectionName);
            await collection.RemoveAsync(id.ToString());
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var usuario = await GetByEmailAsync(email);
            return usuario != null;
        }
    }
} 