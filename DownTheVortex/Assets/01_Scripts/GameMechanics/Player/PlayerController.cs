using BaseSystems.Input;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class PlayerController : InputListener
    {
        public Transform Model;
        public Transform Pivot;
        public LayerMask ObstaclesLayer;
        public LayerMask CollectableLayer;
        public LayerMask ScoreLayer;
        public ParticleSystem PlayFeedback;
        public ParticleSystem DeathFeedback;
        public TextMeshPro NotificationLabel;

        Vector3 OverallVelocity;
        float _currentAngle = -90;
        float _radius = 8.5f;
        float _depth;
        float _speed;
        float _angularSpeed = 1;
        bool _showScoreNotification;

        public void Init(float radius, float depth)
        {
            _speed = GameManager.Instance.GameConfig.OverallSpeed;
            _angularSpeed = Mathf.PI * _speed * Mathf.Rad2Deg * Time.deltaTime;
            _radius = radius;
            _currentAngle = transform.localRotation.eulerAngles.z;
            Model.position = new Vector3(0, -radius, depth);
            OverallVelocity = new Vector3(0, 0, -1) * _speed;
            GameManager.Instance.OnScoreUpdated -= OnScoreUpdate;
            GameManager.Instance.OnScoreUpdated += OnScoreUpdate;
        }

        public void Activate()
        {
            StartCoroutine(RotateModel());
            PlayFeedback.transform.position = Model.transform.position;
            PlayFeedback.Play();
            GameManager.Instance.OnScoreUpdated -= OnScoreUpdate;
            GameManager.Instance.OnScoreUpdated += OnScoreUpdate;
        }

        private IEnumerator RotateModel()
        {
            while (true)
            {
                yield return null;
                Model.localRotation *= Quaternion.Euler(_angularSpeed * Time.deltaTime, 0, 0);
            }
        }

        private void FixedUpdate()
        {
            if (_showScoreNotification)
            {
                NotificationLabel.transform.Translate(OverallVelocity * Time.deltaTime);
                // Stop notification when out of screen
                if (NotificationLabel.transform.position.z < -10)
                {
                    NotificationLabel.gameObject.SetActive(false);
                    _showScoreNotification = false;
                }
            }
        }

        public void Deactivate()
        {
            StopAllCoroutines();
            PlayFeedback.Pause();
            GameManager.Instance.OnScoreUpdated -= OnScoreUpdate;
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
            NotificationLabel.transform.position = Model.transform.position;
            NotificationLabel.text = string.Format("+{0}", GameManager.Instance.GameConfig.ScorePerStep);
            NotificationLabel.gameObject.SetActive(true);
            _showScoreNotification = true;
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
                CollectableStep collectable = other.GetComponentInParent<CollectableStep>();
                collectable.OnCollect();
                GameManager.Instance.AddCollectable();
            } else if ((ScoreLayer.value & (1 << other.gameObject.layer)) > 0)
            {
                GameManager.Instance.AddScore();
            }
        }
    }
}