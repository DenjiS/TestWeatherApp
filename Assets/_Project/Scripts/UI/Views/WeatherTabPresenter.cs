using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WeatherTabPresenter : MonoBehaviour
{
    [Inject] private readonly IRequestQueue _requestQueue;
    [Inject] private readonly IWeatherService _weatherService;
    [Inject] private readonly IWeatherUpdateManager _weatherUpdateManager;

    [SerializeField] private TMP_Text _weatherText;
    [SerializeField] private RawImage _weatherIcon;

    private void OnEnable()
    {
        _requestQueue.AddRequest(UpdateWeather);
        _weatherUpdateManager.StartUpdating();
    }

    private void OnDisable() =>
        _weatherUpdateManager.StopUpdating();

    public async UniTask UpdateWeather()
    {
        try
        {
            WeatherData data = await _weatherService.FetchWeatherAsync();
            _weatherText.text = $"Today - {data.Temperature}F";
            _weatherIcon.texture = data.Icon;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to update weather: {ex.Message}");
        }
    }
}
