using NoSQL.Application.Interfaces;
using NoSQL.Application.Services;
using NoSQL.Domain.Interfaces;
using NoSQL.Infrastructure;
using NoSQL.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Couchbase
builder.Services.AddSingleton<CouchbaseDbContext>();

// Configure Repositories
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<INotificacionRepository, NotificacionRepository>();

// Configure Application Services
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<INotificacionService>(provider =>
{
    var configuration = builder.Configuration;
    var notificacionRepository = provider.GetRequiredService<INotificacionRepository>();
    
    return new NotificacionService(
        notificacionRepository,
        configuration["SmtpSettings:Server"] ?? "smtp.gmail.com",
        int.Parse(configuration["SmtpSettings:Port"] ?? "587"),
        configuration["SmtpSettings:Username"] ?? "",
        configuration["SmtpSettings:Password"] ?? ""
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
