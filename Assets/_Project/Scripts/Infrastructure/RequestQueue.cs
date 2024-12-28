using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading;

public class RequestQueue : IRequestQueue
{
    private readonly Queue<Func<CancellationToken, UniTask>> _requestQueue = new();

    private CancellationTokenSource _currentTokenSource;
    private bool _isProcessing;

    public void AddRequest(Func<CancellationToken, UniTask> requestFunc)
    {
        _requestQueue.Enqueue(requestFunc);

        if (!_isProcessing)
            ProcessQueue().Forget();
    }

    public void CancelCurrentRequest()
    {
        _currentTokenSource?.Cancel();
        _currentTokenSource = null;
    }

    private async UniTaskVoid ProcessQueue()
    {
        _isProcessing = true;

        while (_requestQueue.Count > 0)
        {
            Func<CancellationToken, UniTask> requestFunc = _requestQueue.Dequeue();

            _currentTokenSource = new CancellationTokenSource();

            try
            {
                if (requestFunc != null)
                    await requestFunc.Invoke(_currentTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Request was canceled.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Request failed: {ex.Message}");
            }
            finally
            {
                _currentTokenSource = null;
            }
        }

        _isProcessing = false;
    }
}