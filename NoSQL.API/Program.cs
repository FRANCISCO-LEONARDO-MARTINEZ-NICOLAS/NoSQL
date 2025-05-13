using NoSQL.Application.Services;
using NoSQL.Infrastructure;
using NoSQL.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configurar Couchbase
builder.Services.AddSingleton<CouchbaseDbContext>();

// Registrar los servicios
builder.Services.AddScoped<OptometristaService>();

// Registrar el repositorio y servicio de Paciente
builder.Services.AddScoped<PacienteRepository>();
builder.Services.AddScoped<PacienteService>();

// Registrar el repositorio y servicio de Cita
builder.Services.AddScoped<CitaRepository>();
builder.Services.AddScoped<CitaService>();

// Registrar el repositorio y servicio de Consulta
builder.Services.AddScoped<ConsultaRepository>();
builder.Services.AddScoped<ConsultaService>();

// Registrar el repositorio de Optometrista
builder.Services.AddScoped<OptometristaRepository>();

// Registrar el repositorio y servicio de Producto
builder.Services.AddScoped<ProductoRepository>();
builder.Services.AddScoped<ProductoService>();

// Registrar los controladores
builder.Services.AddControllers();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
