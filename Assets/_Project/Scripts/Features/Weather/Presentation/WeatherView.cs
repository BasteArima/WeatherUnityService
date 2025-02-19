using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using _Project.Scripts.Features.Weather.Domain;
using TMPro;

namespace _Project.Scripts.Features.Weather.Presentation
{
    public class WeatherView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _weatherText;
        [SerializeField] private GameObject _loadingIndicator;

        public void SetLoading(bool loading)
        {
            _loadingIndicator.SetActive(loading);
        }

        public void SetWeather(WeatherInfo info)
        {
            SetLoading(false);
            if (!string.IsNullOrEmpty(info.IconUrl))
                StartCoroutine(LoadIcon(info.IconUrl));
            _weatherText.text = $"Сегодня - {info.TemperatureF}";
        }

        private IEnumerator LoadIcon(string url)
        {
            using (var request = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(url))
            {
                yield return request.SendWebRequest();
                if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
                {
                    var tex = UnityEngine.Networking.DownloadHandlerTexture.GetContent(request);
                    if (_icon != null && tex != null)
                    {
                        _icon.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
                    }
                }
            }
        }
    }
}