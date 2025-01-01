using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RequestQueue : IRequestQueue
{
    private readonly Queue<IRequestCommand> _requestQueue = new();

    private IRequestCommand _currentRequest;
    private bool _isProcessing;

    public void AddRequest(IRequestCommand request)
    {
        _requestQueue.Enqueue(request);

        if (!_isProcessing)
            ProcessQueue().Forget();
    }

    public void CancelCurrentRequest() =>
        _currentRequest?.Cancel();

    private async UniTaskVoid ProcessQueue()
    {
        _isProcessing = true;

        while (_requestQueue.Count > 0)
        {
            _currentRequest = _requestQueue.Dequeue();

            try
            {
                await _currentRequest.Execute();
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Request not completed: {exception.Message}");
            }

            _currentRequest = null;
        }

        _isProcessing = false;
    }
}