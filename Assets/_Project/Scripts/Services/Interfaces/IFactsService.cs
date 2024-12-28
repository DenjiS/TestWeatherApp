using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.UI;

public interface IFactsService
{
    UniTask<List<FactData>> FetchFactsAsync();
    UniTask<FactDetailData> FetchFactDetailAsync(string factId);
}