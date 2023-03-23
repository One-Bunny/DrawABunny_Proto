using UnityEngine;


namespace OneBunny
{
    public abstract class Singleton : MonoBehaviour
    {
        private static Singleton _instance = null;
        private static object _lock = new object();
        private static bool _applicationQuit = false;

        public static Singleton Instance
        {
            get
            {
                if (_applicationQuit)
                {
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<Singleton>();

                        if (_instance == null)
                        {
                            string componentName = typeof(Singleton).ToString();

                            GameObject findObject = GameObject.Find(componentName);

                            if (findObject == null)
                            {
                                findObject = new GameObject(componentName);
                            }

                            _instance = findObject.AddComponent<Singleton>();

                            DontDestroyOnLoad(_instance);
                        }
                    }

                    return _instance;
                }
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _applicationQuit = true;
        }

        public virtual void OnDestroy()
        {
            _applicationQuit = true;
        }
    }
}