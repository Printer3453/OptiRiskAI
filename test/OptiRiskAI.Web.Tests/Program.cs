using Microsoft.AspNetCore.Builder;
using OptiRiskAI;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("OptiRiskAI.Web.csproj"); 
await builder.RunAbpModuleAsync<OptiRiskAIWebTestModule>(applicationName: "OptiRiskAI.Web");

public partial class Program
{
}
