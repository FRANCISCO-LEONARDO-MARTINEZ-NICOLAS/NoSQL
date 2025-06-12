# Script para verificar el estado de todos los servicios
Write-Host "🔍 Verificando estado de servicios..." -ForegroundColor Green
Write-Host ""

# Verificar .NET
try {
    $dotnetVersion = dotnet --version
    Write-Host "✅ .NET: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ .NET: No encontrado" -ForegroundColor Red
}

# Verificar Node.js
try {
    $nodeVersion = node --version
    Write-Host "✅ Node.js: $nodeVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Node.js: No encontrado" -ForegroundColor Red
}

# Verificar npm
try {
    $npmVersion = npm --version
    Write-Host "✅ npm: $npmVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ npm: No encontrado" -ForegroundColor Red
}

Write-Host ""

# Verificar Couchbase Server
try {
    $couchbaseService = Get-Service -Name "CouchbaseServer" -ErrorAction SilentlyContinue
    if ($couchbaseService) {
        if ($couchbaseService.Status -eq "Running") {
            Write-Host "✅ Couchbase Server: Ejecutándose" -ForegroundColor Green
        } else {
            Write-Host "⚠️  Couchbase Server: $($couchbaseService.Status)" -ForegroundColor Yellow
        }
    } else {
        Write-Host "❌ Couchbase Server: Servicio no encontrado" -ForegroundColor Red
    }
} catch {
    Write-Host "❌ Couchbase Server: Error al verificar" -ForegroundColor Red
}

Write-Host ""

# Verificar puertos
$ports = @(7001, 5173, 8091)
$portNames = @("Backend API", "Frontend Dev", "Couchbase Web")

for ($i = 0; $i -lt $ports.Length; $i++) {
    $port = $ports[$i]
    $name = $portNames[$i]
    
    try {
        $connection = Test-NetConnection -ComputerName localhost -Port $port -InformationLevel Quiet -WarningAction SilentlyContinue
        if ($connection) {
            Write-Host "✅ $name (puerto $port): Disponible" -ForegroundColor Green
        } else {
            Write-Host "❌ $name (puerto $port): No disponible" -ForegroundColor Red
        }
    } catch {
        Write-Host "❌ $name (puerto $port): Error al verificar" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "📋 Resumen de URLs:" -ForegroundColor Cyan
Write-Host "   Backend API: https://localhost:7001" -ForegroundColor White
Write-Host "   Frontend: http://localhost:5173" -ForegroundColor White
Write-Host "   Couchbase Web: http://localhost:8091" -ForegroundColor White
Write-Host "   Swagger: https://localhost:7001/swagger" -ForegroundColor White

Write-Host ""
Write-Host "💡 Consejos:" -ForegroundColor Yellow
Write-Host "   - Si Couchbase no está ejecutándose, instálalo desde: https://www.couchbase.com/downloads/" -ForegroundColor Gray
Write-Host "   - Si los puertos no están disponibles, verifica que los servicios estén ejecutándose" -ForegroundColor Gray
Write-Host "   - Para iniciar todo el proyecto, ejecuta: .\start-dev.ps1" -ForegroundColor Gray 