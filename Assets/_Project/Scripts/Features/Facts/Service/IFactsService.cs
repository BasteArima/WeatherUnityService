using _Project.Scripts.Features.Facts.Domain;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Features.Facts.Service
{
    public interface IFactsService
    {
        UniTask<FactInfo[]> GetFactsAsync();
        UniTask<FactDetail> GetFactDetailAsync(string factId);
    }
}