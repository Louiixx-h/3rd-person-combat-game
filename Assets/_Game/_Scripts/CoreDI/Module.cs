using UnityEngine;

namespace AlienWaves.CoreDI
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