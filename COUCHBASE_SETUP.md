# 🗄️ Configuración de Couchbase

## Requisitos Previos

1. **Instalar Couchbase Server**
   - Descargar desde: https://www.couchbase.com/downloads/
   - Versión recomendada: 7.x o superior
   - Instalar con configuración por defecto

## Configuración Inicial

### 1. Acceder a la Consola Web
- Abrir navegador y ir a: http://localhost:8091
- Usuario inicial: `Administrator`
- Contraseña inicial: `password`

### 2. Crear Bucket
1. Ir a **Buckets** en el menú lateral
2. Hacer clic en **Add Bucket**
3. Configurar:
   - **Name**: `OpticaNoSQL`
   - **Memory Quota**: 100 MB (mínimo)
   - **Replicas**: 0 (para desarrollo)
4. Hacer clic en **Add Bucket**

### 3. Crear Usuario
1. Ir a **Security** > **Users** en el menú lateral
2. Hacer clic en **Add User**
3. Configurar:
   - **Username**: `OpticaNoSQL`
   - **Full Name**: `Optica NoSQL User`
   - **Password**: `Leo000426`
   - **Roles**: 
     - `Bucket Full Access` para `OpticaNoSQL`
     - `Query System Catalog` (opcional)
4. Hacer clic en **Add User**

## Verificación de Conexión

### 1. Verificar desde el Backend
El backend intentará conectarse automáticamente al iniciar. Verificar en los logs:

```
[INFO] Iniciando conexión a Couchbase...
[INFO] Intentando conectar a Couchbase en couchbase://localhost con usuario OpticaNoSQL
[INFO] Iniciando conexión directa...
[INFO] Conexión directa exitosa
[INFO] Intentando abrir el bucket OpticaNoSQL...
[INFO] Bucket OpticaNoSQL abierto exitosamente
[INFO] Verificando operaciones básicas...
[INFO] Ping exitoso. Servicios disponibles: kv, query, index, search, analytics, eventing
[INFO] Realizando prueba de operación básica...
[INFO] Operación de escritura exitosa
[INFO] Operación de lectura exitosa
[INFO] Operación de eliminación exitosa
```

### 2. Verificar desde la Consola Web
1. Ir a **Query** en el menú lateral
2. Ejecutar: `SELECT * FROM OpticaNoSQL LIMIT 1;`
3. Debería ejecutarse sin errores

## Solución de Problemas

### Error: "Connection refused"
- Verificar que Couchbase Server esté ejecutándose
- Verificar que el puerto 8091 esté disponible
- Reiniciar el servicio de Couchbase

### Error: "Authentication failed"
- Verificar credenciales en `appsettings.json`
- Verificar que el usuario tenga permisos correctos
- Crear el usuario nuevamente si es necesario

### Error: "Bucket not found"
- Verificar que el bucket `OpticaNoSQL` exista
- Crear el bucket si no existe
- Verificar que el usuario tenga acceso al bucket

### Error: "Timeout"
- Aumentar los timeouts en `CouchbaseDbContext.cs`
- Verificar la conectividad de red
- Verificar que Couchbase tenga recursos suficientes

## Configuración de Desarrollo

Para desarrollo local, la configuración mínima es:

```json
{
  "CouchbaseSettings": {
    "ConnectionString": "couchbase://localhost",
    "Username": "OpticaNoSQL",
    "Password": "Leo000426",
    "BucketName": "OpticaNoSQL"
  }
}
```

## Comandos Útiles

### Reiniciar Couchbase Server (Windows)
```powershell
# Como administrador
net stop CouchbaseServer
net start CouchbaseServer
```

### Verificar estado del servicio
```powershell
sc query CouchbaseServer
```

### Acceder a logs de Couchbase
- Windows: `C:\Program Files\Couchbase\Server\var\lib\couchbase\logs\`
- Linux: `/opt/couchbase/var/lib/couchbase/logs/` 