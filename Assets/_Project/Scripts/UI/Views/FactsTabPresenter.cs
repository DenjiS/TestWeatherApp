using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class FactsTabPresenter : MonoBehaviour
{
    [Inject] private readonly IRequestQueue _requestQueue;
    [Inject] private readonly IFactsService _factsService;

    [SerializeField] private FactView _factTemplate;
    [SerializeField] private PopupPresenter _popupPanel;
    [SerializeField] private Transform _factsContainer;

    private void Awake()
    {
        _popupPanel.CloseButtonClicked.AddListener(() => _factsContainer.gameObject.SetActive(true));
    }

    private void OnEnable()
    {
        _requestQueue.CancelCurrentRequest();
        _requestQueue.AddRequest(new RequestCommand<List<FactData>>(_factsService.FetchFactsAsync, UpdateFacts));
    }

    private void UpdateFacts(List<FactData> facts)
    {
        foreach (Transform child in _factsContainer)
            Destroy(child.gameObject);

        foreach (FactData factData in facts)
        {
            FactView factView = Instantiate(_factTemplate, _factsContainer);
            factView.Text.text = $"{factData.Id} - {factData.Name}";

            factView.Button.onClick.AddListener(() =>
            {
                _requestQueue.CancelCurrentRequest();
                _requestQueue.AddRequest(new RequestCommand<FactDetailData>(() => OnFactClicked(factData.WebId, factView.LoadingIcon), ShowPopup));

                _factsContainer.gameObject.SetActive(false);
            });
        }

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
        _popupPanel.gameObject.SetActive(true);
        _popupPanel.Setup(detail.Name, detail.Description);
    }
}