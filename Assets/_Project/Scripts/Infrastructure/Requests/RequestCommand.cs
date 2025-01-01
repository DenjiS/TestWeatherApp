using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class RequestCommand<T> : IRequestCommand
{
    private readonly Func<UniTask<T>> _onRequest;
    private readonly Action<T> _onComplete;

    private readonly CancellationTokenSource _cancellationToken = new();

    public RequestCommand(Func<UniTask<T>> onRequest, Action<T> onComplete)
    {
        _onRequest = onRequest;
        _onComplete = onComplete;
    }

    public async UniTask Execute()
    {
        try
        {
            T requestData = await _onRequest.Invoke().AttachExternalCancellation(_cancellationToken.Token);

#if ENABLE_REQUEST_DELAY_TEST
            await UniTask.Delay(TimeSpan.FromSeconds(1d));
#endif
            _onComplete.Invoke(requestData);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Request was canceled.");
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }

    }

    public void Cancel() =>
        _cancellationToken.Cancel();
}
