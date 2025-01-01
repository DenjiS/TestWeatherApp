using Cysharp.Threading.Tasks;
using System;
using UniRx;
using Zenject;

public class WeatherUpdateManager : IWeatherUpdateManager
{
    private const int UpdateIntervalSeconds = 5;

    [Inject] private readonly IRequestQueue _requestQueue;
    [Inject] private readonly IWeatherService _weatherService;
    [Inject] private readonly WeatherTabPresenter _weatherTabView;

    private readonly CompositeDisposable _disposables = new();

    public void StartUpdating()
    {
        StopUpdating();

        Observable.Interval(TimeSpan.FromSeconds(UpdateIntervalSeconds))
            .Subscribe(_ => _requestQueue.AddRequest(new RequestCommand<WeatherData>(_weatherService.FetchWeatherAsync, _weatherTabView.UpdateWeather)))
            .AddTo(_disposables);
    }

    public void StopUpdating()
    {
        _disposables.Clear();
    }
}