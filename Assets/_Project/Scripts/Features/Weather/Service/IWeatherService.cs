using System.Threading.Tasks;
using _Project.Scripts.Features.Weather.Domain;

namespace _Project.Scripts.Features.Weather.Service
{
    public interface IWeatherService
    {
        Task<WeatherInfo> GetWeatherAsync();
    }
}