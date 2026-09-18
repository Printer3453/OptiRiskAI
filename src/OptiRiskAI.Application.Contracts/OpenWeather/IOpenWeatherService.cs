using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OptiRiskAI.OpenWeather;

// IApplicationService'den kalıtım alarak ABP'nin Dependency Injection sistemine otomatik dahil oluyoruz.
public interface IOpenWeatherService : IApplicationService
{
    Task<double> GetFwiScoreAsync(double latitude, double longitude);
}