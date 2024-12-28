using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class FactsService : IFactsService
{
    private const string FactsApiUrl = "https://dogapi.dog/api/v2/breeds";
    private const int FactsAmount = 10;

    [Inject] private readonly IRequestQueue _requestQueue;

    private List<FactData> _factsData;

    public async UniTask<List<FactData>> FetchFactsAsync()
    {
        using UnityWebRequest request = UnityWebRequest.Get(FactsApiUrl);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log(json);

            FactsJsonData factsJsonData = JsonUtility.FromJson<FactsJsonData>(json);
            List<FactItem> factsItems = factsJsonData.data.Take(FactsAmount).ToList();
            _factsData = new List<FactData>();

            for (int i = 0; i < factsItems.Count; i++)
                _factsData.Add(new FactData { Id = i + 1, WebId = factsItems[i].id, Name = factsItems[i].attributes.name });

            return _factsData;
        }
        else
        {
            Debug.LogError($"Facts API request failed: {request.error}");
            throw new Exception("Failed to fetch facts");
        }
    }

    public async UniTask<FactDetailData> FetchFactDetailAsync(string factId)
    {
        using UnityWebRequest request = UnityWebRequest.Get(FactsApiUrl);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            FactsJsonData factsJsonData = JsonUtility.FromJson<FactsJsonData>(json);
            FactItem factItem = factsJsonData.data.Where(data => data.id == factId).FirstOrDefault();

            return new FactDetailData { Name = factItem.attributes.name, Description = factItem.attributes.description };
        }
        else
        {
            Debug.LogError($"Fact detail API request failed: {request.error}");
            throw new Exception("Failed to fetch fact details");
        }
    }
}
