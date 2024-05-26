using UnityEngine;

namespace CombatGame.Commons.Utils
{
    public class LookTarget : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_camera == null) return;
            transform.LookAt(_camera.transform);
        }
    }
}