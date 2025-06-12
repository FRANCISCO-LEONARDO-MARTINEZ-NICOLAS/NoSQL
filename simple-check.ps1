Write-Host "Verificando estado de servicios..." -ForegroundColor Green
Write-Host ""

# Verificar .NET
try {
    $dotnetVersion = dotnet --version
    Write-Host "OK .NET: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "ERROR .NET: No encontrado" -ForegroundColor Red
}

# Verificar Node.js
try {
    $nodeVersion = node --version
    Write-Host "OK Node.js: $nodeVersion" -ForegroundColor Green
} catch {
    Write-Host "ERROR Node.js: No encontrado" -ForegroundColor Red
}

# Verificar npm
try {
    $npmVersion = npm --version
    Write-Host "OK npm: $npmVersion" -ForegroundColor Green
} catch {
    Write-Host "ERROR npm: No encontrado" -ForegroundColor Red
}

Write-Host ""
Write-Host "URLs de acceso:" -ForegroundColor Cyan
Write-Host "   Backend API: https://localhost:7001" -ForegroundColor White
Write-Host "   Frontend: http://localhost:5173" -ForegroundColor White
Write-Host "   Couchbase Web: http://localhost:8091" -ForegroundColor White
Write-Host "   Swagger: https://localhost:7001/swagger" -ForegroundColor White

Write-Host ""
Write-Host "Para iniciar el proyecto completo:" -ForegroundColor Yellow
Write-Host "   .\start-dev.ps1" -ForegroundColor Gray 