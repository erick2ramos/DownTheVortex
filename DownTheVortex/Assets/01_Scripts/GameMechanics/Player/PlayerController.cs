using BaseSystems.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PlayerController : InputListener
    {
        public Transform Model;
        public Transform Pivot;
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
        }

        protected override void OnTouchStay(TouchInputEvent input)
        {
            if (GameManager.Instance.CurrentState != GameState.Playing)
                return;
            _currentAngle += input.touchDelta.x;
            Pivot.localRotation = Quaternion.Euler(0, 0, _currentAngle);
        }

        public Vector3 AngularPosition(float radius, float angleRad)
        {
            Vector3 newPos = new Vector3(
                Mathf.Cos(angleRad),
                Mathf.Sin(angleRad)) * radius;
            return newPos;
        }
    }
}