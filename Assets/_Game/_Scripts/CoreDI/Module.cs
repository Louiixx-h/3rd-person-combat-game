using UnityEngine;

namespace LuisLabs.CoreDI
{
    public abstract class Module : MonoBehaviour
    {
        public abstract void Register();
        public abstract void Unregister();

        private void OnDestroy()
        {
            Unregister();
        }
    }
}