using _Project.Scripts.Core.Network;
using _Project.Scripts.Features.Facts.Service;
using _Project.Scripts.Features.Weather.Service;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Core.Installers
{
    [CreateAssetMenu(fileName = "AppInstaller", menuName = "Installers/AppInstaller")]
    public class AppInstaller : ScriptableObjectInstaller<AppInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IRequestQueue>().To<RequestQueue>().AsSingle();
            Container.Bind<IWeatherService>().To<WeatherService>().AsSingle();
            Container.Bind<IFactsService>().To<FactsService>().AsSingle();
        }
    }
}