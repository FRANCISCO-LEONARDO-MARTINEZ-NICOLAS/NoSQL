using Couchbase;
using Couchbase.KeyValue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace NoSQL.Infrastructure
{
    public class CouchbaseDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CouchbaseDbContext> _logger;
        private readonly ICluster _cluster;
        private readonly IBucket _bucket;

        public string BucketName { get; }
        public IBucket Bucket => _bucket;
        public ICluster Cluster => _cluster;

        public CouchbaseDbContext(IConfiguration configuration, ILogger<CouchbaseDbContext> logger)
        {
            _configuration = configuration;
            _logger = logger;
            
            try
            {
                _logger.LogInformation("Iniciando conexión a Couchbase...");
                
                var settings = _configuration.GetSection("CouchbaseSettings");
                BucketName = settings["BucketName"] ?? "OpticaNoSQL";
                
                // Asegurarnos de usar el puerto correcto
var connectionString = settings["ConnectionString"] ?? "couchbase://localhost";                var username = settings["Username"] ?? "OpticaNoSQL";
                var password = settings["Password"] ?? "Leo000426";

                _logger.LogInformation($"Intentando conectar a Couchbase en {connectionString} con usuario {username}");

                var options = new ClusterOptions
                {
                    UserName = username,
                    Password = password,
                    KvConnectTimeout = TimeSpan.FromSeconds(10),
                    KvTimeout = TimeSpan.FromSeconds(10),
                    QueryTimeout = TimeSpan.FromSeconds(10),
                    ManagementTimeout = TimeSpan.FromSeconds(10)
                };

                // Intentar conexión directa
                _logger.LogInformation("Iniciando conexión directa...");
                _cluster = Couchbase.Cluster.ConnectAsync(connectionString, options).GetAwaiter().GetResult();
                _logger.LogInformation("Conexión directa exitosa");

                _logger.LogInformation($"Intentando abrir el bucket {BucketName}...");
                _bucket = _cluster.BucketAsync(BucketName).GetAwaiter().GetResult();
                _logger.LogInformation($"Bucket {BucketName} abierto exitosamente");

                // Verificar que podemos realizar operaciones básicas
                _logger.LogInformation("Verificando operaciones básicas...");
                var pingResult = _cluster.PingAsync().GetAwaiter().GetResult();
                _logger.LogInformation($"Ping exitoso. Servicios disponibles: {string.Join(", ", pingResult.Services.Keys)}");

                // Intentar una operación básica de lectura/escritura
                TestConnectionAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error detallado al conectar con Couchbase: {ex.GetType().Name} - {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError(ex.InnerException, $"Inner Exception: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}");
                }
                throw new Exception($"Error al conectar con Couchbase: {ex.Message}", ex);
            }
        }

        private async Task TestConnectionAsync()
        {
            _logger.LogInformation("Realizando prueba de operación básica...");
            var collection = _bucket.DefaultCollection();
            var testKey = "test_connection";
            var testDoc = new { test = "value", timestamp = DateTime.UtcNow };
            
            try
            {
                await collection.UpsertAsync(testKey, testDoc);
                _logger.LogInformation("Operación de escritura exitosa");
                
                var result = await collection.GetAsync(testKey);
                _logger.LogInformation("Operación de lectura exitosa");
                
                await collection.RemoveAsync(testKey);
                _logger.LogInformation("Operación de eliminación exitosa");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en operación de prueba");
                throw;
            }
        }

        // Obtener una colección del bucket
        public ICouchbaseCollection GetCollection(string collectionName)
        {
            try
            {
                _logger.LogInformation($"Obteniendo colección {collectionName} del bucket {BucketName}");
                return _bucket.DefaultCollection();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la colección {collectionName}");
                throw new Exception($"Error al obtener la colección {collectionName}: {ex.Message}", ex);
            }
        }
    }
}