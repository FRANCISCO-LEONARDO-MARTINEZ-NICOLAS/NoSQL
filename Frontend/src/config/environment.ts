// Configuración de entorno para la aplicación
export const environment = {
  production: false,
  apiBaseUrl: 'http://localhost:5271',
  appName: 'Optica NoSQL',
  appVersion: '1.0.0',
  // Configuración de Couchbase (solo para referencia)
  couchbase: {
    connectionString: 'couchbase://localhost',
    username: 'OpticaNoSQL',
    password: 'Leo000426',
    bucketName: 'OpticaNoSQL'
  }
}; 