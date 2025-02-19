using System;
using _Project.Scripts.Features.Weather.Domain;
using _Project.Scripts.Features.Weather.Service;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Core.Network
{
    public class WeatherRequest : IRequest
    {
        private readonly Action<WeatherInfo> _onSuccess;
        private readonly Action<Exception> _onError;
        
        private readonly IWeatherService _weatherService;
        private bool _cancelRequested;

        public WeatherRequest(IWeatherService weatherService,
            Action<WeatherInfo> onSuccess, 
            Action<Exception> onError)
        {
            _weatherService = weatherService;
            _onSuccess = onSuccess;
            _onError = onError;
        }

        public async UniTask ExecuteAsync()
        {
            if (_cancelRequested) return;
            try
            {
                var info = await _weatherService.GetWeatherAsync();
                if (!_cancelRequested)
                {
                    _onSuccess?.Invoke(info);
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