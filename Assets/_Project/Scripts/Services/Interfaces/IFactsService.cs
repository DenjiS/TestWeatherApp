using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public interface IFactsService
{
    UniTask<FactData[]> FetchFactsAsync();
    UniTask<FactDetailData> FetchFactDetailAsync(string factId);
}