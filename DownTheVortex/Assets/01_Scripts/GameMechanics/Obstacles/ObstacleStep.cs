using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Gameplay.Obstacles {
    public class ObstacleStep : MonoBehaviour
    {
        ObstacleController[] _obstacles;
        Animator _animator;
        bool _active = false;

        public void Init()
        {
            _active = false;
            _obstacles = GetComponentsInChildren<ObstacleController>(true);
            _animator = GetComponent<Animator>();
        }

        public void Activate()
        {
            _active = true;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            
        }

        public void Deactivate()
        {
            _animator.SetTrigger("Deactivate");
            Destroy(gameObject, 1);
        }
    }
}