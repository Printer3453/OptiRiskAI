using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OptiRiskAI.Data;
using Volo.Abp.DependencyInjection;

namespace OptiRiskAI.EntityFrameworkCore;

public class EntityFrameworkCoreOptiRiskAIDbSchemaMigrator
    : IOptiRiskAIDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreOptiRiskAIDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the OptiRiskAIDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<OptiRiskAIDbContext>()
            .Database
            .MigrateAsync();
    }
}
