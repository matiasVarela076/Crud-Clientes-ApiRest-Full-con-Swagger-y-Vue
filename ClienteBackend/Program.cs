using ClientesAPI.Business.Services;
using ClientesAPI.Business.Validators;
using ClientesAPI.Core.Entities;
using ClientesAPI.Core.Interfaces;
using ClientesAPI.Core.Mappings;
using ClientesAPI.Data;
using ClientesAPI.Data.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue", corsBuilder =>
    {
        corsBuilder.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:5173")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
    });
});

// Agregar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ClientesContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)));

// Registrar AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Registrar FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<ClienteCreacionDtoValidator>();

// Registrar Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Registrar Services
builder.Services.AddScoped<IClienteService, ClienteService>();

var app = builder.Build();

// Configurar middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowVue");
app.UseAuthorization();
app.MapControllers();

app.Run();
