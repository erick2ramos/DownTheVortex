using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace BaseSystems.Managers {
    public class ManagerHandler : MonoBehaviour
    {
        static Dictionary<Type, Manager> Managers;

        public void Init()
        {
            Managers = new Dictionary<Type, Manager>();
            SortedList<int, Manager> priorityList = new SortedList<int, Manager>();
            // All children gameobjects of the manager handler GO must
            // inherit from the manager base class
            foreach (Transform child in transform)
            {
                Manager instance = child.GetComponent<Manager>();
                priorityList.Add(instance.InitializationPriority, instance);
                instance.RegisterAsManager();
            }

            foreach(KeyValuePair<int, Manager> manager in priorityList)
            {
                manager.Value.Initialize();
            }
            priorityList.Clear();

            DontDestroyOnLoad(gameObject);
        }

        public static void Register(Type managerType, Manager instance)
        {
            Managers[managerType] = instance;
        }

        public static T Get<T>() where T : class
        {
            Manager manager = null;
            if (!Managers.TryGetValue(typeof(T), out manager))
            {
                throw new Exception("No manager of type \"{0}\" is registered");
            }
            return manager as T;
        }
    }
}