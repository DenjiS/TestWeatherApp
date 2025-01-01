public interface IRequestQueue
{
    void AddRequest(IRequestCommand request);
    void CancelCurrentRequest();
}