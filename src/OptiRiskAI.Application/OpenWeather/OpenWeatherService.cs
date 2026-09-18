using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Application.Services;

OptiRiskAIApplicationModule
namespace OptiRiskAI.OpenWeather;

public class OpenWeatherService : ApplicationService, IOpenWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public OpenWeatherService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<double> GetFwiScoreAsync(double latitude, double longitude)
    {
        try
        {
            var apiKey = _configuration["OpenWeather:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new UserFriendlyException("OpenWeather API anahtarı appsettings.json içinde bulunamadı.");
            }

            // IHttpClientFactory kullanımı, testlerde Mock'lamayı ve port yönetimini kolaylaştırır.
            var client = _httpClientFactory.CreateClient("OpenWeatherClient");
            var url = $"https://api.openweathermap.org/data/2.5/fwi?lat={latitude}&lon={longitude}&appid={apiKey}";

            var response = await client.GetFromJsonAsync<OpenWeatherFwiResponseDto>(url);

            if (response != null && response.List.Count > 0)
            {
                return response.List[0].Main.Fwi;
            }

            Logger.LogWarning($"OpenWeather FWI verisi boş döndü. Koordinatlar: Lat={latitude}, Lon={longitude}");
            return 0; // Veri gelmezse riski 0 kabul et (Güvenli Fallback)
        }
        catch (HttpRequestException ex)
        {
            // İnternet koparsa veya API Rate Limit yersek uygulama çökmesin, log atıp devam etsin.
            Logger.LogError(ex, "OpenWeather API'sine erişilirken ağ hatası oluştu.");
            return 0;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "OpenWeather FWI skoru hesaplanırken beklenmeyen bir hata oluştu.");
            return 0;
        }
    }
}