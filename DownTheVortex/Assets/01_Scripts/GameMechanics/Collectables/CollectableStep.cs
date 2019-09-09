using UnityEngine;
using System.Collections;
using BaseSystems.Audio;
using BaseSystems.Managers;

namespace Gameplay
{
    public class CollectableStep : Obstacles.ObstacleStep
    {
        [SerializeField]
        ParticleSystem _collectFeedback;
        [SerializeField]
        AudioID _sfxOnCollect;
        [SerializeField]
        Transform _model;
        [SerializeField]
        float _rotationDegree;

        public override void Init()
        {
            base.Init();
            _collectFeedback.Stop();
        }

        public void OnCollect()
        {
            AudioManager audioManager = ManagerHandler.Get<AudioManager>();
            _model.gameObject.SetActive(false);
            _collectFeedback.Play();
        }

        protected override void FixedUpdate()
        {
            _model.transform.rotation *= Quaternion.Euler(0, 0, _rotationDegree * Time.deltaTime);

            if (!_active)
                return;

            transform.Translate(_direction * _speed);
            if (transform.position.z < -10)
            {
                Deactivate();
            }
        }
    }
}