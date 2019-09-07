using UnityEngine;
using System.Collections;

namespace BaseSystems.Patterns
{
    public class Singleton<SingletonType> : MonoBehaviour
        where SingletonType : MonoBehaviour
    {
        static SingletonType _instance;

        public static SingletonType Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Try to find the instance in the scene
                    _instance = GameObject.FindObjectOfType<SingletonType>();
                    if(_instance == null)
                    {
                        // No instance in the scene, then create a game object and attach the singleton component to it
                        GameObject go = new GameObject(typeof(SingletonType).ToString() + "_Singleton");
                        _instance = go.AddComponent<SingletonType>();
                    }
                }
                return _instance;
            }
        }
    }
}