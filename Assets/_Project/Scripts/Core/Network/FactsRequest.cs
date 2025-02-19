using System;
using _Project.Scripts.Features.Facts.Domain;
using _Project.Scripts.Features.Facts.Service;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Core.Network
{
    public class FactsRequest : IRequest
    {
        private readonly Action<FactInfo[]> _onSuccess;
        private readonly Action<Exception> _onError;
        
        private readonly IFactsService _factsService;
        private bool _cancelRequested;

        public FactsRequest(IFactsService factsService,
            Action<FactInfo[]> onSuccess,
            Action<Exception> onError)
        {
            _factsService = factsService;
            _onSuccess = onSuccess;
            _onError = onError;
        }

        public async UniTask ExecuteAsync()
        {
            if (_cancelRequested) return;
            try
            {
                var facts = await _factsService.GetFactsAsync();
                if (!_cancelRequested)
                {
                    _onSuccess?.Invoke(facts);
                }
            }
            catch (Exception ex)
            {
                if (!_cancelRequested)
                {
                    _onError?.Invoke(ex);
                }
            }
        }

        public void Cancel()
        {
            _cancelRequested = true;
        }
    }
}