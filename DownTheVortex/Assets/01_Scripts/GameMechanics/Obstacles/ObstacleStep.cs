using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Gameplay.Obstacles {
    public class ObstacleStep : MonoBehaviour
    {
        public Transform Pivot;
        List<GameObject> _obstacles;
        Animator _animator;
        bool _active = false;
        float _speed = 0;
        Vector3 _direction;

        public void Init()
        {
            _active = false;
            _obstacles = new List<GameObject>();
            _animator = GetComponent<Animator>();
            _speed = GameManager.Instance.GameConfig.OverallSpeed;
            _direction = new Vector3(0, 0, -1);
        }

        public void Activate()
        {
            Pivot.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            _active = true;
            gameObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            if (!_active)
                return;
            transform.Translate(_direction * _speed);
            if(transform.position.z < -10)
            {
                GameManager.Instance.AddScore();
                Deactivate();
            }
        }

        public void Deactivate()
        {
            _active = false;
            _animator.SetTrigger("Deactivate");
            Destroy(gameObject, 1);
        }
    }
}