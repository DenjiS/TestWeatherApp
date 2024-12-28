using ModestTree.Util;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [Preserve]
    public override void InstallBindings()
    {
        Container.Bind<IWeatherService>().To<WeatherService>().AsSingle();
        Container.Bind<IFactsService>().To<FactsService>().AsSingle();
        Container.Bind<IRequestQueue>().To<RequestQueue>().AsSingle();
    }
}
