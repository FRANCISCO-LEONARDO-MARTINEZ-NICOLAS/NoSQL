# Script para ejecutar el proyecto completo en modo desarrollo
Write-Host "🚀 Iniciando Optica NoSQL - Modo Desarrollo" -ForegroundColor Green
Write-Host ""

# Verificar si .NET está instalado
try {
    $dotnetVersion = dotnet --version
    Write-Host "✅ .NET encontrado: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ .NET no está instalado. Por favor instala .NET 8.0 o superior." -ForegroundColor Red
    exit 1
}

# Verificar si Node.js está instalado
try {
    $nodeVersion = node --version
    Write-Host "✅ Node.js encontrado: $nodeVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Node.js no está instalado. Por favor instala Node.js 18 o superior." -ForegroundColor Red
    exit 1
}

# Verificar si npm está instalado
try {
    $npmVersion = npm --version
    Write-Host "✅ npm encontrado: $npmVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ npm no está instalado." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "📦 Instalando dependencias del frontend..." -ForegroundColor Yellow
Set-Location "Frontend"
npm install
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error al instalar dependencias del frontend" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "🔧 Restaurando paquetes del backend..." -ForegroundColor Yellow
Set-Location ".."
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error al restaurar paquetes del backend" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "🎯 Iniciando servicios..." -ForegroundColor Green
Write-Host ""

# Iniciar el backend en una nueva ventana
Write-Host "🔧 Iniciando Backend (.NET API)..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD'; dotnet run --project NoSQL.API"

# Esperar un momento para que el backend se inicie
Start-Sleep -Seconds 3

# Iniciar el frontend en una nueva ventana
Write-Host "🎨 Iniciando Frontend (React)..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD\Frontend'; npm run dev"

Write-Host ""
Write-Host "✅ Servicios iniciados correctamente!" -ForegroundColor Green
Write-Host ""
Write-Host "🌐 URLs de acceso:" -ForegroundColor White
Write-Host "   Backend API: https://localhost:7001" -ForegroundColor Cyan
Write-Host "   Frontend: http://localhost:5173" -ForegroundColor Cyan
Write-Host "   Swagger: https://localhost:7001/swagger" -ForegroundColor Cyan
Write-Host ""
Write-Host "📝 Para detener los servicios, cierra las ventanas de PowerShell correspondientes." -ForegroundColor Yellow 