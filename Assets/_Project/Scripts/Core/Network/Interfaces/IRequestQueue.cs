namespace _Project.Scripts.Core.Network
{
    public interface IRequestQueue
    {
        void Enqueue(IRequest request);
        void Remove(IRequest request);
        void CancelCurrent();
    }
}