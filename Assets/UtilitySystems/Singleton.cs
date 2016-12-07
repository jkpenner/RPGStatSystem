using UnityEngine;
using System.Collections;

namespace UtilitySystems {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        static private T _instance = null;
        static private object _lock = new object();
        private static bool _applicationIsQuitting = false;

        static public T Instance {
            get {
                if (_applicationIsQuitting) {
                    Debug.LogWarningFormat("[{0}]: Instance already destroyed on application " +
                        "quit. Won't create again - returning null.", typeof(T).Name);
                    return null;
                }

                lock(_lock) {
                    if (_instance == null) {
                        // Get all objects of type T
                        var instances = FindObjectsOfType<T>();
                        if (instances.Length > 0) {
                            // Assign first instance as the singleton
                            _instance = instances[0];

                            // If there are more then one instance of the singleton,
                            // destroy all extra instances. There can be only one.
                            if (instances.Length > 1) {
                                Debug.LogWarningFormat("[{0}]: More then one instance of the singleton " +
                                    "found in scene. Destroying extra instances.", typeof(T).Name);

                                for (int i = 1; i < instances.Length; i++) {
                                    Destroy(instances[i].gameObject);
                                }
                            }
                        }
                        // If there are no instances in the scene
                        else if (!_applicationIsQuitting) {
                            Debug.LogWarningFormat("[{0}]: No instance of singleton found in the scene. Creating " +
                                "temporary instance.", typeof(T).Name);

                            // Create Singleton GameObject
                            GameObject singleton = new GameObject();
                            singleton.name = "(singleton) " + typeof(T).Name;

                            // Add Singleton Component to GameObject, and
                            // assign component to the singleton instance
                            _instance = singleton.AddComponent<T>();
                        }
                    }
                    return _instance;
                }
            }
        }

        
        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        public virtual void OnDestroy() {
            if (_instance == this) {
                _applicationIsQuitting = true;
            } 
        }
    }
}