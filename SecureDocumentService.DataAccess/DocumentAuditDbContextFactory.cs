using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SecureDocumentService.DataAccess;

public class DocumentAuditDbContextFactory:IDesignTimeDbContextFactory<DocumentAuditDbContext>
{
     public DocumentAuditDbContext CreateDbContext(string[] args)
        {
            
            var basePath = Directory.GetCurrentDirectory();

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var provider = config["DatabaseProvider"];
            var optsBuilder = new DbContextOptionsBuilder<DocumentAuditDbContext>();

            switch (provider)
            {
                case "SqlServer":
                    optsBuilder.UseSqlServer(
                      config.GetConnectionString("SqlServerConnection"));
                    break;
                case "Oracle":
                    optsBuilder.UseOracle(
                      config.GetConnectionString("OracleConnection"));
                    break;
                default:
                    throw new InvalidOperationException(
                      $"Unsupported DatabaseProvider: {provider}");
            }

            return new DocumentAuditDbContext(optsBuilder.Options);
        }
}
