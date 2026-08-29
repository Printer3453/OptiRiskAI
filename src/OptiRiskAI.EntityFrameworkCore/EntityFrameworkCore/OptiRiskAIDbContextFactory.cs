using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace OptiRiskAI.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class OptiRiskAIDbContextFactory : IDesignTimeDbContextFactory<OptiRiskAIDbContext>
{
    public OptiRiskAIDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        OptiRiskAIEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<OptiRiskAIDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"));
        
        return new OptiRiskAIDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../OptiRiskAI.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
