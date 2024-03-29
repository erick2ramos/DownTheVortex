﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Gameplay.Obstacles {
    /// <summary>
    /// Handles the logic of an obstacle which behaviour should be using and how it moves
    /// towards the camera
    /// </summary>
    public class ObstacleStep : MonoBehaviour
    {
        public event System.Action<ObstacleStep> OnDestroyEvent;

        public Transform Pivot;
        protected Animator _animator;
        protected bool _active = false;
        protected float _speed = 0;
        protected Vector3 _direction;
        protected ObstacleBehaviour[] _behaviours;
        protected MeshRenderer[] _obstacleRenderers;

        protected virtual void Awake()
        {
            _behaviours = GetComponents<ObstacleBehaviour>();
            _obstacleRenderers = GetComponentsInChildren<MeshRenderer>();
        }

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
            foreach (var behaviour in _behaviours)
            {
                behaviour.IsActive = false;
                behaviour.Setup();
            }
            if(_behaviours.Length > 0)
                _behaviours[Random.Range(0, _behaviours.Length)].IsActive = true;
            gameObject.SetActive(true);
        }

        protected virtual void FixedUpdate()
        {
            if (!_active)
                return;
            // Moves the obstacle towards the camera
            transform.Translate(_direction * _speed * Time.deltaTime);

            foreach (var behaviour in _behaviours)
            {
                if(behaviour.IsActive)
                    behaviour.HandleStep();
            }

            // If the obstacle pass a fixed plane it should be deactivated
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

        public virtual void SetColor(Color newColor)
        {
            foreach (MeshRenderer renderer in _obstacleRenderers)
            {
                renderer.material.SetColor("_BaseColor", newColor);
            }
        }
    }
}