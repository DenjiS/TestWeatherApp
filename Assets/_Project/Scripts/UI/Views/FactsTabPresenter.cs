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
        _requestQueue.AddRequest(async (cancellationToken) =>
            await UpdateFacts().AttachExternalCancellation(cancellationToken));
    }

    private async UniTask UpdateFacts()
    {
        foreach (Transform child in _factsContainer)
            Destroy(child.gameObject);

        try
        {
            List<FactData> facts = await _factsService.FetchFactsAsync();

            foreach (FactData factData in facts)
            {
                FactView factView = Instantiate(_factTemplate, _factsContainer);
                factView.Text.text = $"{factData.Id} - {factData.Name}";

                factView.Button.onClick.AddListener(() =>
                {
                    _requestQueue.CancelCurrentRequest();
                    _requestQueue.AddRequest(async (cancellationToken) =>
                        await OnFactClicked(factData.WebId, factView.LoadingIcon).AttachExternalCancellation(cancellationToken));

                    _factsContainer.gameObject.SetActive(false);
                });
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to update facts: {ex.Message}");
        }
    }

    private async UniTask OnFactClicked(string factId, Image loadingIcon)
    {
        try
        {
            loadingIcon.gameObject.SetActive(true);
            FactDetailData detail = await _factsService.FetchFactDetailAsync(factId);

            ShowPopup(detail);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to fetch fact details: {ex.Message}");
        }
        finally
        {
            loadingIcon.gameObject.SetActive(false);
        }
    }

    private void ShowPopup(FactDetailData detail)
    {
        _popupPanel.gameObject.SetActive(true);
        _popupPanel.Setup(detail.Name, detail.Description);
    }
}