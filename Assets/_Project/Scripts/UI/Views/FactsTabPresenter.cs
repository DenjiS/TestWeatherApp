using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UniRx;

public class FactsTabPresenter : MonoBehaviour
{
    [Inject] private readonly IRequestQueue _requestQueue;
    [Inject] private readonly IFactsService _factsService;
    [Inject] private readonly FactView.Pool _factViewsPool;

    private readonly List<FactView> _activeViews = new();

    [SerializeField] private Transform _factsContainer;
    [SerializeField] private PopupView _popupPanel;

    private void Awake() =>
        _popupPanel.CloseButton.onClick.AddListener(() => SwitchPopupPanel(false));

    private void OnEnable()
    {
        SwitchPopupPanel(false);

        _requestQueue.CancelCurrentRequest();
        _requestQueue.AddRequest(new RequestCommand<FactData[]>(_factsService.FetchFactsAsync, UpdateFacts));
    }

    private void OnDisable() =>
        ClearFactsPanel();

    private void UpdateFacts(FactData[] facts)
    {
        ClearFactsPanel();

        foreach (FactData factData in facts)
        {
            FactView factView = _factViewsPool.Spawn(factData, _factsContainer);

            factView.Button.onClick.AddListener(() =>
            {
                _requestQueue.CancelCurrentRequest();
                _requestQueue.AddRequest(new RequestCommand<FactDetailData>(() => OnFactClicked(factData.WebId, factView.LoadingIcon), ShowPopup));
            });

            _activeViews.Add(factView);
        }
    }

    private void ClearFactsPanel()
    {
        foreach (FactView view in _activeViews)
            _factViewsPool.Despawn(view);

        _activeViews.Clear();
    }

    private async UniTask<FactDetailData> OnFactClicked(string factId, Image loadingIcon)
    {
        FactDetailData detail;

        try
        {
            loadingIcon.gameObject.SetActive(true);

            detail = await _factsService.FetchFactDetailAsync(factId);
        }
        catch (Exception exception)
        {
            detail = default;
            Debug.LogError($"Failed to fetch fact details: {exception.Message}");
        }
        finally
        {
            loadingIcon.gameObject.SetActive(false);
        }

        return detail;
    }

    private void ShowPopup(FactDetailData detail)
    {
        SwitchPopupPanel(true);
        _popupPanel.Setup(detail.Name, detail.Description);
    }

    private void SwitchPopupPanel(bool isPopupActive)
    {
        _factsContainer.gameObject.SetActive(!isPopupActive);
        _popupPanel.gameObject.SetActive(isPopupActive);
    }
}