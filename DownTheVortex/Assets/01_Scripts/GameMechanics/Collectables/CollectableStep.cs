using UnityEngine;
using System.Collections;
using BaseSystems.Audio;
using BaseSystems.Managers;
using BaseSystems.Feedback;

namespace Gameplay
{
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

        public void OnCollect()
        {
            _model.gameObject.SetActive(false);
            _collectFeedback.PlayAll();
        }

        protected override void FixedUpdate()
        {
            _model.transform.rotation *= Quaternion.Euler(0, _rotationDegree * Time.deltaTime, 0);

            if (!_active)
                return;

            transform.Translate(_direction * _speed * Time.deltaTime);
            if (transform.position.z < -10)
            {
                Deactivate();
            }
        }
    }
}