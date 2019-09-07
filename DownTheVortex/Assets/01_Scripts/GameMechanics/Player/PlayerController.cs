using BaseSystems.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PlayerController : InputListener
    {
        public float AngularSpeed = 1;
        public Transform Model;
        public Transform Pivot;
        float _currentAngle = -90;
        float _radius = 8.5f;
        float _depth;

        public void Init(Vector3 startingPos)
        {
            Vector3 projectedPos = startingPos;
            _depth = projectedPos.z;
            projectedPos.z = 0;
            _radius = projectedPos.magnitude;
            _currentAngle = transform.localRotation.eulerAngles.z;
            Pivot.position = startingPos;
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
                Model.localRotation *= Quaternion.Euler(AngularSpeed * Time.deltaTime, 0, 0);
            }
        }

        public void Deactivate()
        {
            StopAllCoroutines();
        }

        protected override void OnTouchStay(TouchInputEvent input)
        {
            _currentAngle += input.touchDelta.x;
            transform.localRotation = Quaternion.Euler(0, 0, _currentAngle);
        }
    }
}