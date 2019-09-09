using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Gameplay.Obstacles {
    public class ObstacleStep : MonoBehaviour
    {
        public event System.Action<ObstacleStep> OnDestroyEvent;

        public Transform Pivot;
        protected Animator _animator;
        protected bool _active = false;
        protected float _speed = 0;
        protected Vector3 _direction;

        public virtual void Init()
        {
            _active = false;
            _animator = GetComponent<Animator>();
            _speed = GameManager.Instance.GameConfig.OverallSpeed;
            _direction = new Vector3(0, 0, -1);
            GameManager.Instance.OnPause += ToggleActive;
            GameManager.Instance.OnGameOver += OnGameOver;
        }

        public virtual void OnGameOver()
        {
            _active = false;
        }

        public virtual void ToggleActive(bool isPaused)
        {
            _active = !isPaused;
        }

        public virtual void Activate()
        {
            Pivot.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            _active = true;
            gameObject.SetActive(true);
        }

        protected virtual void FixedUpdate()
        {
            if (!_active)
                return;
            transform.Translate(_direction * _speed * Time.deltaTime);
            if(transform.position.z < -10)
            {
                Deactivate();
            }
        }

        public virtual void Deactivate()
        {
            _active = false;
            _animator.SetTrigger("Deactivate");
            GameManager.Instance.OnGameOver -= OnGameOver;
            GameManager.Instance.OnPause -= ToggleActive;
            OnDestroyEvent?.Invoke(this);
        }
    }
}