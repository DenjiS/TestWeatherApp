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
            IRequestCommand request = _requestQueue.Dequeue();

            try
            {
                await request.Execute();
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Request was canceled.");
            }
            catch (Exception exception)
            {
                Debug.LogError($"Request failed: {exception.Message}");
            }
        }

        _isProcessing = false;
    }
}