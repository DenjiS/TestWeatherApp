using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Networking;
using UnityEngine;

public class WeatherService : IWeatherService
{
    private const string WeatherApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

    public async UniTask<WeatherData> FetchWeatherAsync()
    {
        using UnityWebRequest request = UnityWebRequest.Get(WeatherApiUrl);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            WeatherJsonData data = JsonUtility.FromJson<WeatherJsonData>(json);
            WeatherPeriod currentPeriod = data.properties.periods[0];

            using UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(currentPeriod.icon);
            await textureRequest.SendWebRequest();

            if (textureRequest.result == UnityWebRequest.Result.Success)
            {
                DownloadHandlerTexture textureHandler = textureRequest.downloadHandler as DownloadHandlerTexture;
                Texture icon = textureHandler.texture;

                return new WeatherData { Temperature = currentPeriod.temperature, Icon = icon };
            }
            else
            {
                throw new Exception("Failed to fetch weather icon");
            }
        }
        else
        {
            throw new Exception("Failed to fetch weather data");
        }
    }
}