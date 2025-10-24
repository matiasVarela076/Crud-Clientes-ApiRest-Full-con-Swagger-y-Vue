using ClientesAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API de Gestión de Clientes - Intuit Challenge",
        Description = @"API REST completa para gestionar clientes con operaciones CRUD.
        
**Características principales:**
- Validación completa de campos obligatorios (Nombre, Apellido, CUIT, Teléfono, Email)
- Validación de formatos (Email, CUIT de 11 dígitos, Teléfono, Fecha de Nacimiento)
- Unicidad garantizada para ID y CUIT
- Borrado lógico de clientes (campo Activo)
- Logging estructurado de todas las operaciones
- Manejo robusto de errores con códigos HTTP apropiados

**Validaciones implementadas:**
- Nombre, Apellido, CUIT, Teléfono, Email: Obligatorios
- CUIT: Formato de 11 dígitos, único por cliente activo
- Email: Formato válido de email
- Teléfono: Mínimo 7 dígitos
- Fecha de Nacimiento: No futura, edad mínima 18 años, máximo 150 años
- ID: Único, auto-generado por la base de datos"
    });

    // Incluir comentarios XML para documentación detallada
    string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    
    // Configurar mapeo de tipos para Swagger
    options.MapType<DateTime>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date-time"
    });
    
    options.MapType<DateTime?>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date-time",
        Nullable = true
    });
});

string connectionString = "Server=localhost;Database=ClientesDB;Integrated Security=true;TrustServerCertificate=true;";
builder.Services.AddDbContext<ClientesContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.MapControllers();

app.Run();
