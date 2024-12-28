using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RequestQueue : IRequestQueue
{
    private readonly Queue<Func<UniTask>> _requestQueue = new();

    private Func<UniTask> _currentRequest;
    private bool _isProcessing;

    public void AddRequest(Func<UniTask> requestFunc)
    {
        _requestQueue.Enqueue(requestFunc);

        if (!_isProcessing)
            ProcessQueue().Forget();
    }

    public void CancelCurrentRequest() =>
        _currentRequest = null;

    private async UniTaskVoid ProcessQueue()
    {
        _isProcessing = true;

        while (_requestQueue.Count > 0)
        {
            _currentRequest = _requestQueue.Dequeue();

            try
            {
                if (_currentRequest != null)
                    await _currentRequest.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Request failed: {ex.Message}");
            }

            _currentRequest = null;
        }

        _isProcessing = false;
    }
}