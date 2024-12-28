using Cysharp.Threading.Tasks;
using System;

public interface IRequestQueue
{
    void AddRequest(Func<UniTask> requestFunc);
    void CancelCurrentRequest();
}