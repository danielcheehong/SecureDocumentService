
using Microsoft.EntityFrameworkCore;
using SecureDocumentService.DataAccess;
using SecureDocumentService.Infrastructure.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core services.
var dbProvider = builder.Configuration.GetValue<string>("DatabaseProvider");

switch (dbProvider)
{
    case "SqlServer":
        builder.Services.AddDbContext<DocumentAuditDbContext>(opts =>
            opts.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));
        break;
    case "Oracle":
        builder.Services.AddDbContext<DocumentAuditDbContext>(opts =>
            opts.UseOracle(
                builder.Configuration.GetConnectionString("OracleConnection")));
        break;
    default:
        throw new InvalidOperationException($"Unsupported database provider: {dbProvider}");
}


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IDocumentAuditChannel>(); // Register the DocumentAuditChannel as a singleton service


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
