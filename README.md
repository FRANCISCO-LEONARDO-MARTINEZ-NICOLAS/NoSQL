# 🏥 Optica NoSQL - Sistema de Gestión Óptica

Sistema completo de gestión para clínicas ópticas desarrollado con .NET Core (Backend) y React (Frontend), utilizando Couchbase como base de datos NoSQL.

## 🚀 Características

- **Gestión de Pacientes**: Registro y seguimiento completo de pacientes
- **Citas Médicas**: Programación y gestión de citas con optometristas
- **Consultas Médicas**: Registro de exámenes y diagnósticos
- **Inventario**: Control de productos ópticos (lentes, monturas, etc.)
- **Ventas**: Sistema de ventas y facturación
- **Reportes**: Generación de reportes y estadísticas
- **Autenticación**: Sistema de login con JWT
- **Interfaz Moderna**: Diseño responsive con Tailwind CSS

## 📋 Prerrequisitos

### Backend (.NET Core)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) o superior
- [Couchbase Server](https://www.couchbase.com/downloads/) (versión Community o Enterprise)

### Frontend (React)
- [Node.js](https://nodejs.org/) (versión 18 o superior)
- [npm](https://www.npmjs.com/) (incluido con Node.js)

## 🛠️ Instalación

### 1. Clonar el repositorio
```bash
git clone <url-del-repositorio>
cd Optica-NoSQL
```

### 2. Configurar Couchbase
1. Instalar Couchbase Server
2. Crear un bucket llamado `OpticaNoSQL`
3. Crear un usuario con las siguientes credenciales:
   - Usuario: `OpticaNoSQL`
   - Contraseña: `Leo000426`
   - Permisos: Administrador del bucket

### 3. Configurar el Backend
1. Navegar al directorio del proyecto:
```bash
cd NoSQL.API
```

2. Restaurar paquetes NuGet:
```bash
dotnet restore
```

3. Verificar la configuración en `appsettings.json`:
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

### 4. Configurar el Frontend
1. Navegar al directorio del frontend:
```bash
cd Frontend
```

2. Instalar dependencias:
```bash
npm install
```

## 🚀 Ejecución

### Opción 1: Script Automático (Recomendado)
Ejecutar el script de PowerShell que inicia ambos servicios:

```powershell
.\start-dev.ps1
```

### Opción 2: Manual

#### Backend
```bash
cd NoSQL.API
dotnet run
```

#### Frontend (en otra terminal)
```bash
cd Frontend
npm run dev
```

## 🌐 URLs de Acceso

- **Frontend**: http://localhost:5173
- **Backend API**: https://localhost:7001
- **Swagger Documentation**: https://localhost:7001/swagger

## 📁 Estructura del Proyecto

```
Optica-NoSQL/
├── Frontend/                 # Aplicación React
│   ├── src/
│   │   ├── components/       # Componentes reutilizables
│   │   ├── pages/           # Páginas de la aplicación
│   │   ├── services/        # Servicios de API
│   │   ├── contexts/        # Contextos de React
│   │   ├── hooks/           # Hooks personalizados
│   │   └── types/           # Definiciones de TypeScript
│   ├── package.json
│   └── vite.config.ts
├── NoSQL.API/               # API REST (.NET Core)
│   ├── Controllers/         # Controladores de la API
│   ├── DTOs/               # Objetos de transferencia de datos
│   └── Program.cs          # Configuración de la aplicación
├── NoSQL.Application/       # Lógica de aplicación
├── NoSQL.Domain/           # Entidades y reglas de negocio
├── NoSQL.Infrastructure/   # Implementación de repositorios
└── NoSQL.CLI/              # Aplicación de consola
```

## 🔧 Configuración de Desarrollo

### Variables de Entorno
El proyecto utiliza las siguientes configuraciones por defecto:

- **Couchbase**: localhost:8091
- **API Port**: 7001 (HTTPS)
- **Frontend Port**: 5173 (HTTP)

### CORS
El backend está configurado para aceptar peticiones desde:
- http://localhost:5173 (Vite dev server)
- http://localhost:3000 (alternativo)

## 📚 API Endpoints

### Autenticación
- `POST /api/auth/login` - Iniciar sesión
- `POST /api/auth/register` - Registro de usuarios

### Pacientes
- `GET /api/patients` - Obtener todos los pacientes
- `POST /api/patients` - Crear nuevo paciente
- `GET /api/patients/{id}` - Obtener paciente por ID
- `PUT /api/patients/{id}` - Actualizar paciente
- `DELETE /api/patients/{id}` - Eliminar paciente

### Citas
- `GET /api/appointments` - Obtener todas las citas
- `POST /api/appointments` - Crear nueva cita
- `GET /api/appointments/{id}` - Obtener cita por ID
- `PUT /api/appointments/{id}` - Actualizar cita
- `DELETE /api/appointments/{id}` - Eliminar cita
- `PATCH /api/appointments/{id}/status` - Actualizar estado de cita

## 🐛 Solución de Problemas

### Error de CORS
Si encuentras errores de CORS, verifica que:
1. El backend esté ejecutándose en el puerto correcto
2. La configuración de CORS en `Program.cs` incluya la URL del frontend

### Error de Conexión a Couchbase
1. Verifica que Couchbase Server esté ejecutándose
2. Confirma las credenciales en `appsettings.json`
3. Asegúrate de que el bucket `OpticaNoSQL` exista

### Error de Dependencias del Frontend
```bash
cd Frontend
rm -rf node_modules package-lock.json
npm install
```

## 📝 Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.

## 👥 Autores

- **Leonardo Martínez** - *Desarrollo inicial* - [TuUsuario](https://github.com/TuUsuario)

## 🙏 Agradecimientos

- Couchbase por proporcionar una excelente base de datos NoSQL
- Microsoft por .NET Core
- React Team por el framework de frontend
- Tailwind CSS por el framework de estilos 