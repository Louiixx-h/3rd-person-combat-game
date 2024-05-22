using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;

namespace AlienWaves.CoreDI
{
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator global;
        private static Dictionary<Scene, ServiceLocator> sceneContainers;
        private static List<GameObject> tmpSceneGameObjects;
        private readonly ServiceManager serviceManger = new();

        private const string GlobalServiceLocatorName = "ServiceLocator [Global]";
        private const string SceneServiceLocatorName = "ServiceLocator [Scene]";

        internal void ConfigureAsGlobal(bool dontDestroyOnLoad) {
            if (global == this) {
                Debug.LogWarning("ServiceLocator is already configured as global");
            } else if (global != null) {
                Debug.LogError("Another Service Locator is already configured as global");
            } else {
                global = this;
                if (dontDestroyOnLoad) {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }

        internal void ConfigureAsScene() {
            var scene = gameObject.scene;

            sceneContainers ??= new Dictionary<Scene, ServiceLocator>();

            if (sceneContainers.ContainsKey(scene)) {
                Debug.LogWarning("ServiceLocator is already configured for this scene");
            } else {
                sceneContainers.Add(scene, this);
            }
        }

        public static ServiceLocator Global
        {
            get
            {
                if (global != null)
                {
                    return global;
                }

                if (FindFirstObjectByType<ServiceLocatorGlobalBootstrapper>() is {  } found) {
                    found.BootstrapOnDemand();
                    return global;
                }

                var container = new GameObject(GlobalServiceLocatorName, typeof(ServiceLocator));
                container.AddComponent<ServiceLocatorGlobalBootstrapper>().BootstrapOnDemand();

                return global;
            }
        }

        public static ServiceLocator For(MonoBehaviour monoBehaviour) {
            return monoBehaviour.GetComponentInParent<ServiceLocator>() ?? throw new ArgumentException("ServiceLocator not found in hierarchy");
        }

        public static ServiceLocator ForSceneOf(MonoBehaviour monoBehaviour) {
            var scene = monoBehaviour.gameObject.scene;

            if (sceneContainers.TryGetValue(scene, out var container) && container != null) {
                return container;
            }

            tmpSceneGameObjects.Clear();
            scene.GetRootGameObjects(tmpSceneGameObjects);

            foreach (var item in tmpSceneGameObjects.Where(go => go.GetComponent<ServiceLocatorSceneBootstrapper>() != null)) {
                if (item.TryGetComponent(out ServiceLocatorSceneBootstrapper bootstrapper) && bootstrapper.Container != monoBehaviour) {
                    bootstrapper.BootstrapOnDemand();
                    return bootstrapper.Container;
                }
            }

            throw new ArgumentException("ServiceLocator not found in hierarchy");
        }

        public ServiceLocator Register<T>(T service) {
            serviceManger.Register(service);
            return this;
        }

        public ServiceLocator Register<T>(Type type, object service) {
            serviceManger.Register(type, service);
            return this;
        }

        public void Unregister<T>()
        {
            serviceManger.Unregister<T>();
        }

        public ServiceLocator Get<T>(out T service) where T : class {
            if (TryGet(out service)) return this;

            if (TryGetNextInHierarchy(out var container)) {
                container.Get(out service);
            }

            throw new ArgumentException($"Service of type {typeof(T).FullName} is not registered");
        }

        public bool TryGet<T>(out T service) where T : class {
            return serviceManger.TryGet(out service);
        }

        public bool TryGetNextInHierarchy(out ServiceLocator container) {
            if (this == global) {
                container = null;
                return false;
            }

            container = transform.parent.GetComponentInParent<ServiceLocator>() ?? ForSceneOf(this);
            return container != null;
        }

        private void OnDestroy() {
            if (this == global) {
                global = null;
            } else if (sceneContainers.ContainsValue(this)) {
                sceneContainers.Remove(gameObject.scene);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics() {
            global = null;
            sceneContainers = new Dictionary<Scene, ServiceLocator>();
            tmpSceneGameObjects = new List<GameObject>();
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Service Locator/Add Global")]
        private static void AddGlobal() {
            new GameObject(GlobalServiceLocatorName, typeof(ServiceLocator));
        }

        [MenuItem("GameObject/Service Locator/Add Scene")]
        private static void AddScene() {
            new GameObject(SceneServiceLocatorName, typeof(ServiceLocator));
        }
#endif
    }
}
