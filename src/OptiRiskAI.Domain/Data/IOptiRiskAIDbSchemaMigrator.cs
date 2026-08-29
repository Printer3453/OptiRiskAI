using System.Threading.Tasks;

namespace OptiRiskAI.Data;

public interface IOptiRiskAIDbSchemaMigrator
{
    Task MigrateAsync();
}
