using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public interface IFactsService
{
    UniTask<List<FactData>> FetchFactsAsync();
    UniTask<FactDetailData> FetchFactDetailAsync(string factId);
}