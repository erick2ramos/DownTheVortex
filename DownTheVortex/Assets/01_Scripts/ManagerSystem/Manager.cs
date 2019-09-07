using UnityEngine;
using System.Collections;

namespace BaseSystems.Managers
{
    public class Manager : MonoBehaviour
    {
        public int InitializationPriority = 0;

        public virtual void Initialize() { }

        public void RegisterAsManager()
        {
            ManagerHandler.Register(GetType(), this);
        }
    }
}