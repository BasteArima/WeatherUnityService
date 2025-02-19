using System;
using _Project.Scripts.Features.Facts.Domain;
using _Project.Scripts.Features.Facts.Service;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Core.Network
{
    public class FactDetailRequest : IRequest
    {
        private readonly Action<FactDetail> _onSuccess;
        private readonly Action<Exception> _onError;
        
        private readonly IFactsService _factsService;
        private readonly string _factId;
        private bool _cancelRequested;

        public FactDetailRequest(IFactsService factsService,
            string factId,
            Action<FactDetail> onSuccess, 
            Action<Exception> onError)
        {
            _factsService = factsService;
            _factId = factId;
            _onSuccess = onSuccess;
            _onError = onError;
        }

        public async UniTask ExecuteAsync()
        {
            if (_cancelRequested) return;
            try
            {
                var detail = await _factsService.GetFactDetailAsync(_factId);
                if (!_cancelRequested)
                {
                    _onSuccess?.Invoke(detail);
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