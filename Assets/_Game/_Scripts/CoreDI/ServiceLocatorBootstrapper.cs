using UnityEngine;

namespace LuisLabs.CoreDI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ServiceLocator))]
    public abstract class ServiceLocatorBootstrapper : MonoBehaviour
    {
        private ServiceLocator container;
        internal ServiceLocator Container => container ??= GetComponent<ServiceLocator>();

        private bool hasBeenBoostrapped;

        private void Awake() {
            BootstrapOnDemand();
        }

        public void BootstrapOnDemand()
        {
            if (hasBeenBoostrapped) return;
            hasBeenBoostrapped = true;
            Bootstrap();
        }

        protected abstract void Bootstrap();
    }
}