using UnityEngine;

namespace LuisLabs.CoreDI
{
    [AddComponentMenu("SeviceLocator/Service Locator Scene")]
    public class ServiceLocatorSceneBootstrapper : ServiceLocatorBootstrapper
    {
        protected override void Bootstrap()
        {
            Container.ConfigureAsScene();
        }
    }
}