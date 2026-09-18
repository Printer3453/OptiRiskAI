using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OptiRiskAI.OpenWeather;

// burada DTO sınıfı, OpenWeather API'sinden gelen FWI (Fire Weather Index) verilerini temsil eder.
// Bu sınıf, JSON yanıtını C# nesnelerine dönüştürmek için kullanılır.
public class OpenWeatherFwiResponseDto 
{
    [JsonPropertyName("list")]
    public List<OpenWeatherFwiDataDto> List { get; set; } = new();
}

// burada, OpenWeather API'sinden gelen her bir FWI veri noktasını temsil eden DTO sınıfı tanımlanır.
// Bu sınıf, JSON yanıtındaki "main" alanını içerir ve FWI ile ilgili bilgileri taşır.
public class OpenWeatherFwiDataDto
{
    [JsonPropertyName("main")]
    public OpenWeatherFwiMainDto Main { get; set; } = new();
}

// burada, OpenWeather API'sinden gelen FWI ana verilerini temsil eden DTO sınıfı tanımlanır.
public class OpenWeatherFwiMainDto
{
    [JsonPropertyName("fwi")]
    public double Fwi { get; set; }

    [JsonPropertyName("danger_rating")]
    public string DangerRating { get; set; } = string.Empty;
}