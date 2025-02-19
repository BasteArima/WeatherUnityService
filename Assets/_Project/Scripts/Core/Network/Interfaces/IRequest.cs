using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Core.Network
{
    public interface IRequest
    {
        UniTask ExecuteAsync();
        void Cancel();
    }
}