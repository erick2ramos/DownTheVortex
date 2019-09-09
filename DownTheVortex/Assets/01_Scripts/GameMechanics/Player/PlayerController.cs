﻿using BaseSystems.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PlayerController : InputListener
    {
        public Transform Model;
        public Transform Pivot;
        public LayerMask ObstaclesLayer;
        public LayerMask CollectableLayer;
        public ParticleSystem PlayFeedback;
        public ParticleSystem DeathFeedback;

        float _currentAngle = -90;
        float _radius = 8.5f;
        float _depth;
        float _speed;
        float _angularSpeed = 1;

        public void Init(float radius, float depth)
        {
            _speed = GameManager.Instance.GameConfig.OverallSpeed;
            _angularSpeed = 2 * Mathf.PI * _speed * Mathf.Rad2Deg;
            _radius = radius;
            _currentAngle = transform.localRotation.eulerAngles.z;
            Model.position = new Vector3(0, -radius, depth);
        }

        public void Activate()
        {
            StartCoroutine(RotateModel());
            PlayFeedback.transform.position = Model.transform.position;
            PlayFeedback.Play();
        }

        private IEnumerator RotateModel()
        {
            while (true)
            {
                yield return null;
                Model.localRotation *= Quaternion.Euler(_angularSpeed * Time.deltaTime, 0, 0);
            }
        }

        public void Deactivate()
        {
            StopAllCoroutines();
            PlayFeedback.Pause();
        }

        public IEnumerator Kill()
        {
            Handheld.Vibrate();
            Model.gameObject.SetActive(false);
            PlayFeedback.gameObject.SetActive(false);
            DeathFeedback.transform.position = Model.transform.position;
            DeathFeedback.Play();
            yield return null;
            Deactivate();
        }

        protected override void OnTouchStay(TouchInputEvent input)
        {
            if (GameManager.Instance.CurrentState != GameState.Playing)
                return;
            _currentAngle += input.touchDelta.x;
            Pivot.localRotation = Quaternion.Euler(0, 0, _currentAngle);
            PlayFeedback.transform.position = Model.transform.position;
        }

        private void OnScoreUpdate(int newScore)
        {
            // Show score feedback
        }

        private void OnTriggerEnter(Collider other)
        {
            if((ObstaclesLayer.value & (1 << other.gameObject.layer)) > 0)
            {
                // Player collided with an obstacle start the gameover flow
                Debug.Log(LayerMask.LayerToName(other.gameObject.layer));
                StartCoroutine(Kill());
                GameManager.Instance.GameOver();
            } else if((CollectableLayer.value & (1 << other.gameObject.layer)) > 0)
            {
                // Player collected
                var collectable = other.GetComponentInParent<CollectableStep>();
                collectable.OnCollect();
                GameManager.Instance.AddCollectable();
            }
        }
    }
}