using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace OptiRiskAI.Data;

/* This is used if database provider does't define
 * IOptiRiskAIDbSchemaMigrator implementation.
 */
public class NullOptiRiskAIDbSchemaMigrator : IOptiRiskAIDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
