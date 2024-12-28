using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ITabController>().To<TabController>().AsSingle();
        Container.Bind<IWeatherUpdateManager>().To<WeatherUpdateManager>().AsSingle();
        Container.Bind<WeatherTabPresenter>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<FactsTabPresenter>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<TabButton>().FromComponentInHierarchy().AsTransient();
    }
}
