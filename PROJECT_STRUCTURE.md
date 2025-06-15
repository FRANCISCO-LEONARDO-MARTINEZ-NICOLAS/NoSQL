# Estructura del Proyecto Optica NoSQL

## 📁 Estructura General

```
NoSQL/
├── Frontend/                    # Aplicación React + TypeScript
├── NoSQL.API/                   # API REST (ASP.NET Core)
├── NoSQL.Application/           # Capa de Aplicación (Clean Architecture)
├── NoSQL.Domain/                # Capa de Dominio (Clean Architecture)
├── NoSQL.Infrastructure/        # Capa de Infraestructura (Clean Architecture)
├── NoSQL.CLI/                   # Herramientas de línea de comandos
├── GeneradorHash/               # Utilidades adicionales
└── NoSQL.sln                    # Archivo de solución
```

## 🏗️ Arquitectura Clean Architecture

### **NoSQL.Domain** - Capa de Dominio
- **Entidades**: Modelos de datos del negocio
  - `Optometrista.cs`
  - `Paciente.cs`
  - `Cita.cs`
  - `Consulta.cs`
  - `Producto.cs`
  - `Venta.cs`
  - `Usuario.cs`
  - `Notificacion.cs`
  - `HistorialClinico.cs`
  - `ProductoVenta.cs`

- **Interfaces**: Contratos para repositorios y servicios

### **NoSQL.Application** - Capa de Aplicación
- **Servicios**: Lógica de negocio
  - `AuthService.cs`
  - `PacienteService.cs`
  - `OptometristaService.cs`
  - `ConsultaService.cs`
  - `ProductoService.cs`
  - `CitaService.cs`
  - `VentaService.cs`
  - `UsuarioService.cs`
  - `NotificacionService.cs`

- **Interfaces**: Contratos para servicios
- **Settings**: Configuraciones de la aplicación

### **NoSQL.Infrastructure** - Capa de Infraestructura
- **Repositorios**: Implementación de acceso a datos
  - `PacienteRepository.cs`
  - `OptometristaRepository.cs`
  - `ConsultaRepository.cs`
  - `ProductoRepository.cs`
  - `CitaRepository.cs`
  - `VentaRepository.cs`
  - `UsuarioRepository.cs`
  - `NotificacionRepository.cs`

- **DbContext**: `CouchbaseDbContext.cs` - Configuración de Couchbase

### **NoSQL.API** - Capa de Presentación
- **Controllers**: Endpoints de la API REST
  - `AuthController.cs` - Autenticación
  - `DashboardController.cs` - Métricas del dashboard
  - `PacientesController.cs` - Gestión de pacientes
  - `OptometristasController.cs` - Gestión de optometristas
  - `CitasController.cs` - Gestión de citas
  - `ConsultasController.cs` - Gestión de consultas
  - `ProductosController.cs` - Gestión de productos
  - `VentasController.cs` - Gestión de ventas
  - `NotificacionesController.cs` - Sistema de notificaciones
  - `SystemConfigController.cs` - Configuración del sistema
  - `SystemMonitoringController.cs` - Monitoreo del sistema

- **DTOs**: Objetos de transferencia de datos
- **Configuración**: `Program.cs`, `appsettings.json`

## 🎨 Frontend (React + TypeScript)

### **Estructura del Frontend**
```
Frontend/
├── src/
│   ├── components/          # Componentes reutilizables
│   ├── pages/              # Páginas de la aplicación
│   ├── services/           # Servicios para llamadas a la API
│   ├── contexts/           # Contextos de React
│   ├── hooks/              # Hooks personalizados
│   ├── config/             # Configuración (API, environment)
│   ├── types/              # Definiciones de tipos TypeScript
│   ├── App.tsx             # Componente principal
│   └── main.tsx            # Punto de entrada
├── package.json
├── vite.config.ts
└── tailwind.config.js
```

### **Servicios del Frontend**
- `authService.ts` - Autenticación
- `dashboardService.ts` - Métricas del dashboard
- `patientService.ts` - Gestión de pacientes
- `optometristService.ts` - Gestión de optometristas
- `appointmentService.ts` - Gestión de citas
- `consultationService.ts` - Gestión de consultas
- `productService.ts` - Gestión de productos
- `saleService.ts` - Gestión de ventas
- `systemMonitoringService.ts` - Monitoreo del sistema
- `systemConfigService.ts` - Configuración del sistema

## 🔗 Configuración de Conexiones

### **API Endpoints**
- **Base URL**: `http://localhost:5271`
- **Prefijo**: `/api/[controller]`
- **Ejemplos**:
  - `GET /api/auth/login`
  - `GET /api/dashboard/metrics`
  - `GET /api/pacientes`
  - `POST /api/citas`

### **Base de Datos**
- **Couchbase**: `couchbase://localhost`
- **Bucket**: `OpticaNoSQL`
- **Usuario**: `OpticaNoSQL`

## 🚀 Cómo Ejecutar

### **Backend**
```bash
cd NoSQL.API
dotnet run
```

### **Frontend**
```bash
cd Frontend
npm install
npm run dev
```

## 📋 Cambios Realizados

1. ✅ **Eliminada carpeta `Backend/` redundante**
2. ✅ **Consolidados todos los controladores en `NoSQL.API/Controllers/`**
3. ✅ **Verificadas todas las referencias y rutas**
4. ✅ **Confirmada la compilación exitosa del proyecto**
5. ✅ **Mantenida la arquitectura Clean Architecture**
6. ✅ **Organizada la estructura del Frontend**

## 🔧 Dependencias y Referencias

### **Backend (.NET)**
- **NoSQL.API** → **NoSQL.Application** → **NoSQL.Domain**
- **NoSQL.Application** → **NoSQL.Domain**
- **NoSQL.Infrastructure** → **NoSQL.Domain**

### **Frontend (React)**
- **Services** → **API Backend** (puerto 5271)
- **Components** → **Services**
- **Pages** → **Components**

## ✅ Estado del Proyecto

- **Compilación**: ✅ Exitosa
- **Arquitectura**: ✅ Clean Architecture implementada
- **Organización**: ✅ Estructura clara y consistente
- **Referencias**: ✅ Todas las rutas y dependencias correctas
- **Frontend**: ✅ React + TypeScript bien estructurado
- **Backend**: ✅ ASP.NET Core con Couchbase 