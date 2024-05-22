using UnityEngine;

namespace AlienWaves.CoreDI
{
    [AddComponentMenu("SeviceLocator/Service Locator Global")]
    public class ServiceLocatorGlobalBootstrapper : ServiceLocatorBootstrapper
    {
        [SerializeField] private bool dontDestroyOnLoad = true;

        protected override void Bootstrap()
        {
            Container.ConfigureAsGlobal(dontDestroyOnLoad);
        }
    }
}