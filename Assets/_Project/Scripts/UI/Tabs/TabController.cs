using UnityEngine;
using Zenject;

public class TabController : ITabController
{
    [Inject] private readonly WeatherTabPresenter _weatherTabView;
    [Inject] private readonly FactsTabPresenter _factsTabView;
    [Inject] private readonly IRequestQueue _requestQueue;

    private TabType _currentTab;

    public void SwitchTab(TabType tabType)
    {
        if (_currentTab == tabType)
            return;

        _requestQueue.CancelCurrentRequest();

        _currentTab = tabType;

        _weatherTabView.gameObject.SetActive(tabType == TabType.Weather);
        _factsTabView.gameObject.SetActive(tabType == TabType.Facts);
    }
}