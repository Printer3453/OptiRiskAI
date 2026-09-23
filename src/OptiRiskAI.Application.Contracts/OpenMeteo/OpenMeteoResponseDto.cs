using System.Text.Json.Serialization;

namespace OptiRiskAI.OpenMeteo;

public class OpenMeteoResponseDto
{
    [JsonPropertyName("current")]
    public CurrentWeatherDto Current { get; set; } = new();
    // CurrentWeatherDto nesnesi oluşturuluyor ve Current özelliğine atanıyor.
    // Bu, OpenMeteo API'sinden gelen yanıtın "current" bölümünü temsil eder ve bu bölümdeki hava durumu verilerini tutar.
}

// CurrentWeatherDto sınıfı, OpenMeteo API'sinden gelen yanıtın "current" bölümündeki hava durumu verilerini temsil eder.
public class CurrentWeatherDto
{
    [JsonPropertyName("temperature_2m")]
    public float Temperature { get; set; }// Bu özellik, 2 metre yükseklikteki sıcaklık değerini temsil eder.

    [JsonPropertyName("relative_humidity_2m")]
    public float RelativeHumidity { get; set; }// Bu özellik, 2 metre yükseklikteki bağıl nem değerini temsil eder.

    [JsonPropertyName("wind_speed_10m")]
    public float WindSpeed { get; set; }// Bu özellik, 10 metre yükseklikteki rüzgar hızını temsil eder.

    [JsonPropertyName("precipitation")]
    public float Precipitation { get; set; }// Bu özellik, yağış miktarını temsil eder.
}