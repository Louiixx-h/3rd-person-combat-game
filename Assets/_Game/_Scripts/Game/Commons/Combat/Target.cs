using System;
using UnityEngine;

namespace CombatGame.Commons.Combat
{
    public class Target : MonoBehaviour
    {
        public Action<Target> DestroyEvent;

        private void OnDestroy()
        {
            DestroyEvent?.Invoke(this);
        }
    }
}
