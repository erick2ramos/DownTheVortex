using UnityEngine;
using System.Collections;

namespace Gameplay.Obstacles
{
    [RequireComponent(typeof(ObstacleStep))]
    public class ObstacleBehaviour : MonoBehaviour
    {
        public bool IsActive { get; set; }
        protected ObstacleStep _step;
        protected virtual void Awake()
        {
            _step = GetComponent<ObstacleStep>();
        }

        public virtual void Setup() { }

        public virtual void HandleStep() { }
    }
}