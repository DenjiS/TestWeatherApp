using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class FactsTabPresenter : MonoBehaviour
{
    [Inject] private readonly IRequestQueue _requestQueue;
    [Inject] private readonly IFactsService _factsService;

    [SerializeField] private FactView _factTemplate;
    [SerializeField] private PopupView _popupTemplate;
    [SerializeField] private Transform _factsContainer;

    private void OnEnable() =>
        _requestQueue.AddRequest(UpdateFacts);

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
                factView.Button.onClick.AddListener(() => OnFactClicked(factData.WebId));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to update facts: {ex.Message}");
        }
    }

    private async void OnFactClicked(string factId)
    {
        try
        {
            FactDetailData detail = await _factsService.FetchFactDetailAsync(factId);
            Debug.Log($"Fact: {detail.Name}, Description: {detail.Description}");
            ShowPopup(detail);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to fetch fact details: {ex.Message}");
        }
    }

    private void ShowPopup(FactDetailData detail)
    {
        PopupView popup = Instantiate(_popupTemplate, transform);
        popup.Setup(detail.Name, detail.Description);
    }
}