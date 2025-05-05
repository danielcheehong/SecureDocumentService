using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using SecureDocumentService.DataAccess;
using SecureDocumentService.DataAccess.Interfaces;
using SecureDocumentService.DataAccess.Repositories;
using SecureDocumentService.Infrastructure.Implementation;
using SecureDocumentService.Infrastructure.Interface;
using SecureDocumentService.Workers;

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
// Keep track of running worker tasks and its cancellation tokens.
var runningWorkerTasks = new ConcurrentDictionary<Guid, (CancellationTokenSource cts, Task worker)>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton(runningWorkerTasks); // Register the running tasks dictionary as a singleton service
builder.Services.AddSingleton<IDocumentAuditChannel, DocumentAuditChannel>(); // Register the DocumentAuditChannel as a singleton service

// Register the DocumentAuditRepository as a scoped service
builder.Services.AddScoped<IDocumentAuditRepository, DocumentAuditRepository>();

// Register the FileProtectionWorker as a hosted service
builder.Services.AddHostedService<FileProtectionWorker>();

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
