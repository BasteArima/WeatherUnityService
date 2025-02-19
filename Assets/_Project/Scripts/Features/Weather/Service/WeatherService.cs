using System;
using System.Threading.Tasks;
using _Project.Scripts.Features.Weather.Domain;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

namespace _Project.Scripts.Features.Weather.Service
{
public class WeatherService : IWeatherService
{
    private const string WEATHER_URL = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

    public async Task<WeatherInfo> GetWeatherAsync()
    {
        using (var request = UnityWebRequest.Get(WEATHER_URL))
        {
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string json = request.downloadHandler.text;
                    JObject root = JObject.Parse(json);
                    JToken periods = root["properties"]?["periods"];
                    if (periods == null || !periods.HasValues)
                    {
                        throw new Exception("No periods data in weather JSON");
                    }

                    JToken firstPeriod = periods[0];
                    int temperature = firstPeriod["temperature"]?.Value<int>() ?? 0;
                    string iconUrl = firstPeriod["icon"]?.Value<string>();

                    return new WeatherInfo
                    {
                        IconUrl = iconUrl,
                        TemperatureF = temperature + "F"
                    };
                }
                catch (Exception e)
                {
                    Debug.LogError($"JSON parse error (weather): {e.Message}");
                    throw;
                }
            }
            else
            {
                throw new Exception($"Weather request error: {request.error}");
            }
        }
    }
}

}