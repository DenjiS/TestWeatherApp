using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public interface IRequestQueue
{
    void AddRequest(Func<CancellationToken, UniTask> requestFunc);
    void CancelCurrentRequest();
}