using Couchbase;
using Couchbase.Core.Exceptions.KeyValue;
using Couchbase.Query;
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

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var query = $"SELECT u.* FROM `{_context.BucketName}` u WHERE u.type = 'usuario'";
            var result = await _context.Bucket.Cluster.QueryAsync<Usuario>(query);
            return await result.ToListAsync();
        }

        public async Task<Usuario?> GetByIdAsync(string id)
        {
            try
            {
                var result = await _context.Bucket.DefaultCollection()
                    .GetAsync(id);
                return result.ContentAs<Usuario>();
            }
            catch (DocumentNotFoundException)
            {
                return null;
            }
        }

        public async Task<Usuario?> GetBycorreoAsync(string correo)
        {
            var query = $"SELECT u.* FROM `{_context.BucketName}` u WHERE u.type = 'usuario' AND u.correo = $correo LIMIT 1";
            var options = QueryOptions.Create().Parameter("correo", correo);
            var result = await _context.Bucket.Cluster.QueryAsync<Usuario>(query, options);
            return (await result.ToListAsync()).FirstOrDefault();
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            var collection = _context.GetCollection(CollectionName);
            usuario.Id = Guid.NewGuid().ToString();
            usuario.type = "usuario";
            await collection.InsertAsync(usuario.Id, usuario);
            return usuario;
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            var collection = _context.GetCollection(CollectionName);
            usuario.type = "usuario";
            await collection.UpsertAsync(usuario.Id, usuario);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.Bucket.DefaultCollection()
                .RemoveAsync(id);
        }

        public async Task<bool> ExistsBycorreoAsync(string correo)
        {
            var usuario = await GetBycorreoAsync(correo);
            return usuario != null;
        }

        public async Task AddAsync(Usuario usuario)
        {
            await _context.Bucket.DefaultCollection()
                .InsertAsync(usuario.Id, usuario);
        }

        public async Task UpdateAsync(string id, Usuario usuario)
        {
            await _context.Bucket.DefaultCollection()
                .ReplaceAsync(id, usuario);
        }
    }
}