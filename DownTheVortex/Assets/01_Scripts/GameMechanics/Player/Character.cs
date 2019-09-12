using BaseSystems.Audio;
using BaseSystems.DataPersistance;
using BaseSystems.Feedback;
using BaseSystems.Input;
using BaseSystems.Managers;
using Gameplay.Ability;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public enum CharacterState
    {
        Normal,
        UsingAbility,
        Death
    }

    public class Character : InputListener
    {
        public CharacterState CurrentState { get; set; }
        public Transform Model;
        public Transform Pivot;
        public LayerMask ObstaclesLayer;
        public LayerMask CollectableLayer;
        public LayerMask ScoreLayer;
        public ParticleSystem PlayFeedback;
        public Feedbacks DeathFeedback;
        public TextMeshPro NotificationLabel;
        public float MovementModifier = 0.38f;

        #region Abilities
        CharacterAbility[] _characterAbilities;
        CharacterAbility _activeAbility;
        bool _validTap;
        float _tapTime, _tapMaxTime = 0.15f;
        #endregion

        #region Character circular movement
        [System.NonSerialized]
        public float CurrentAngle = -90;
        Vector3 OverallVelocity;
        float _radius = 8.5f, _depth, _speed, _angularSpeed = 1;
        bool _showScoreNotification;
        #endregion

        public void Init(float radius, float depth)
        {
            _speed = GameManager.Instance.GameConfig.OverallSpeed;
            _angularSpeed = Mathf.PI * _speed * Mathf.Rad2Deg * Time.deltaTime;
            _radius = radius;
            CurrentAngle = transform.localRotation.eulerAngles.z;
            Model.position = new Vector3(0, -radius, depth);
            OverallVelocity = new Vector3(0, 0, -1) * _speed;
            GameManager.Instance.OnScoreUpdated -= OnScoreUpdate;
            GameManager.Instance.OnScoreUpdated += OnScoreUpdate;

            // Get all abilities, initialize and activate only the one that was
            // purchased at the store
            _characterAbilities = GetComponents<CharacterAbility>();
            foreach(var ability in _characterAbilities)
            {
                ability.enabled = false;
                ability.IsPermitted = false;
                if(ability.AbilityID == DataPersistanceManager.PlayerData.ActiveAbility)
                {
                    _activeAbility = ability;
                    _activeAbility.IsPermitted = true;
                    _activeAbility.Initialize(this);
                }
            }
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

        private void Update()
        {
            if(_characterAbilities != null)
            {
                foreach(CharacterAbility ability in _characterAbilities)
                {
                    if(ability.enabled && ability.IsPermitted)
                       ability.ProcessAbility();
                }
            }
        }

        private void FixedUpdate()
        {
            PlayFeedback.transform.position = Model.transform.position;

            if (_showScoreNotification)
            {
                NotificationLabel.transform.Translate(OverallVelocity * Time.deltaTime * 0.5f);
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
            ManagerHandler.Get<VibrationManager>().Vibrate();
            Model.gameObject.SetActive(false);
            PlayFeedback.gameObject.SetActive(false);
            DeathFeedback.transform.position = Model.transform.position;
            DeathFeedback.PlayAll();
            yield return null;
            Deactivate();
        }

        protected override void OnTouchStart(TouchInputEvent input)
        {
            if (GameManager.Instance.CurrentState != GameState.Playing)
                return;
            _tapTime = Time.time;
        }

        protected override void OnTouchStay(TouchInputEvent input)
        {
            if (GameManager.Instance.CurrentState != GameState.Playing || (_activeAbility != null && _activeAbility.enabled))
                return;

            CurrentAngle += Mathf.Clamp(input.touchDelta.x, -45, 45) * MovementModifier;
            Pivot.localRotation = Quaternion.Euler(0, 0, CurrentAngle);
        }

        protected override void OnTouchRelease(TouchInputEvent input)
        {
            if (GameManager.Instance.CurrentState != GameState.Playing)
                return;

            if(_activeAbility != null)
                _activeAbility.enabled = (Time.time - _tapTime < _tapMaxTime);
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
                ManagerHandler.Get<VibrationManager>().Vibrate();
                GameManager.Instance.AddCollectable();
            } else if ((ScoreLayer.value & (1 << other.gameObject.layer)) > 0)
            {
                ManagerHandler.Get<VibrationManager>().Vibrate();
                GameManager.Instance.AddScore();
            }
        }
    }
}