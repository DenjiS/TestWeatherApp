using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class FactsService : IFactsService
{
    private const string FactsApiUrl = "https://dogapi.dog/api/v2/breeds";
    private const int FactsAmount = 10;

    public async UniTask<FactData[]> FetchFactsAsync()
    {
        using UnityWebRequest request = UnityWebRequest.Get(FactsApiUrl);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            FactsJsonData factsJsonData = JsonUtility.FromJson<FactsJsonData>(json);

            FactItem[] factsItems = factsJsonData.data.Take(FactsAmount).ToArray();
            FactData[] factsData = new FactData[factsItems.Count()];

            for (int i = 0; i < factsItems.Count(); i++)
                factsData[i] = new FactData { Number = i + 1, WebId = factsItems[i].id, Name = factsItems[i].attributes.name };

            return factsData;
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
