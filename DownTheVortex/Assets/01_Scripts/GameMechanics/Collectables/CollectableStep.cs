using UnityEngine;
using System.Collections;
using BaseSystems.Audio;
using BaseSystems.Managers;
using BaseSystems.Feedback;

namespace Gameplay
{
    /// <summary>
    /// Handler for the collectables / currency
    /// </summary>
    public class CollectableStep : Obstacles.ObstacleStep
    {
        [SerializeField]
        Feedbacks _collectFeedback;
        [SerializeField]
        Transform _model;
        [SerializeField]
        float _rotationDegree;

        public override void Init()
        {
            base.Init();
            _model.gameObject.SetActive(true);
            _collectFeedback.Initialize(gameObject);
        }

        /// <summary>
        /// Called when the player collides with a collectable
        /// </summary>
        public void OnCollect()
        {
            _model.gameObject.SetActive(false);
            _collectFeedback.PlayAll();
        }

        protected override void FixedUpdate()
        {
            // Rotate the model just for eyecandy
            _model.transform.rotation *= Quaternion.Euler(0, _rotationDegree * Time.deltaTime, 0);

            if (!_active)
                return;

            // Moves the collectable towards the screen
            transform.Translate(_direction * _speed * Time.deltaTime);
            // After a fixed plane in space the collectable should deactivate as the
            // player didn't collect it
            if (transform.position.z < -10)
            {
                Deactivate();
            }
        }
    }
}