using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OptiRiskAI.OpenMeteo
{
    public class OpenMeteoService : ApplicationService, IOpenMeteoService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OpenMeteoService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        // Bu metod, verilen enlem ve boylam koordinatlarına göre OpenMeteo API'sinden mevcut hava durumu verilerini çeker.
        public async Task<WeatherTelemetryDto> GetCurrentWeatherAsync(double latitude, double longitude)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("OpenMeteoClient");

                // Küresel standartlar için nokta ve virgül uyuşmazlığını engelliyoruz
                var latStr = latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                var lonStr = longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);

                string url = $"https://api.open-meteo.com/v1/forecast?latitude={latStr}&longitude={lonStr}&current=temperature_2m,relative_humidity_2m,wind_speed_10m,precipitation";

                var response = await client.GetFromJsonAsync<OpenMeteoResponseDto>(url);// OpenMeteo API'sinden yanıt alınıyor ve OpenMeteoResponseDto nesnesine dönüştürülüyor.

                if (response?.Current != null)
                {
                    Logger.LogInformation($"[OPEN-METEO] Veri çekildi: {response.Current.Temperature}°C, Nem: %{response.Current.RelativeHumidity}");

                    return new WeatherTelemetryDto
                    {
                        Temperature = response.Current.Temperature,
                        Humidity = response.Current.RelativeHumidity,
                        WindSpeed = response.Current.WindSpeed,
                        Rain = response.Current.Precipitation
                    };
                }

                throw new Exception("API yanıtı boş döndü.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"[OPEN-METEO HATA] Koordinat: {latitude}, {longitude} için veri çekilemedi.");

                // Simülasyonun çökmemesi için acil durum (Fallback) verisi dönüyoruz
                return new WeatherTelemetryDto { Temperature = 25, Humidity = 50, WindSpeed = 15, Rain = 0 };
            }
        }

    }
}
