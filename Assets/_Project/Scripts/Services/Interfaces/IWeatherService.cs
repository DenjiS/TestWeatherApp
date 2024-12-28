using Cysharp.Threading.Tasks;

public interface IWeatherService
{
    UniTask<WeatherData> FetchWeatherAsync();
}