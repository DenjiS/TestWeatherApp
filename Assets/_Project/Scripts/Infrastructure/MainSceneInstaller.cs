using UnityEngine;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [Header("Facts presenter")]
    [SerializeField] private FactView _factView;
    [SerializeField] private int _factViewsPoolSize = 10;

    public override void InstallBindings()
    {
        Container.Bind<ITabController>().To<TabController>().AsSingle();
        Container.Bind<TabButton>().FromComponentInHierarchy().AsTransient();

        Container.Bind<WeatherTabPresenter>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<IWeatherUpdateManager>().To<WeatherUpdateManager>().AsSingle();

        Container.Bind<FactsTabPresenter>().FromComponentsInHierarchy().AsSingle();
        Container.BindMemoryPool<FactView, FactView.Pool>()
            .WithInitialSize(_factViewsPoolSize)
            .FromComponentInNewPrefab(_factView)
            .UnderTransform(transform);
    }
}
