using UnityEngine;

namespace Scripts.Arch
{
    public class GameManager<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<T>();
            }

            DontDestroyOnLoad(Instance);
        }
    }
}