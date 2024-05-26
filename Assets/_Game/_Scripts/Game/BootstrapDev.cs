using LuisLabs.CoreLevel;
using UnityEngine;

namespace CombatGame.Commons.Utils
{
    class BootstrapDev : MonoBehaviour
    {
        private void Start()
        {
            var sceneController = FindFirstObjectByType<SceneController>();
            sceneController.AddCurrentScene(gameObject.scene.name);
        }
    }
}
