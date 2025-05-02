using NoSQL.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configurar Couchbase
builder.Services.AddSingleton<CouchbaseDbContext>();

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

// Mapear los controladores
app.MapControllers();

app.Run();
