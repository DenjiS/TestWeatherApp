using Cysharp.Threading.Tasks;

public interface IRequestCommand
{
    public UniTask Execute();
    public void Cancel();
}
