using UnityEngine;
using Zenject;
using _Project.Scripts.Core.Network;
using _Project.Scripts.Features.Weather.Service;

namespace _Project.Scripts.Features.Weather.Presentation
{
    public class WeatherPresenter : MonoBehaviour
    {
        private const float POLL_INTERVAL = 5f;

        [SerializeField] private WeatherView _weatherView;

        [Inject] private IWeatherService _weatherService;
        [Inject] private IRequestQueue _requestQueue;

        private WeatherRequest _currentRequest;
        private float _timer;

        private void OnEnable()
        {
            RequestWeather();
            _timer = 0f;
        }

        private void OnDisable()
        {
            CancelCurrentWeatherRequest();
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= POLL_INTERVAL)
            {
                _timer = 0f;
                RequestWeather();
            }
        }

        private void RequestWeather()
        {
            CancelCurrentWeatherRequest();
            _weatherView.SetLoading(true);

            _currentRequest = new WeatherRequest(_weatherService,
                (weatherInfo) => { _weatherView.SetWeather(weatherInfo); },
                (ex) =>
                {
                    _weatherView.SetLoading(false);
                    Debug.LogError($"Weather error: {ex.Message}");
                }
            );
            _requestQueue.Enqueue(_currentRequest);
        }

        private void CancelCurrentWeatherRequest()
        {
            if (_currentRequest != null)
            {
                _requestQueue.Remove(_currentRequest);
                _requestQueue.CancelCurrent();
                _currentRequest = null;
            }
        }
    }
}