using BaseSystems.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PlayerController : InputListener
    { 
        float _currentAngle = -90;
        float _radius = 8.5f;
        float _depth;

        public void Init(Vector3 startingPos)
        {
            Vector3 projectedPos = startingPos;
            _depth = projectedPos.z;
            projectedPos.z = 0;
            _radius = projectedPos.magnitude;
            transform.position = startingPos;
        }

        protected override void OnTouchStay(TouchInputEvent input)
        {
            _currentAngle += input.touchDelta.x;
            transform.position = AngularPosition(_radius, _currentAngle * Mathf.Deg2Rad);
        }

        public Vector3 AngularPosition(float radius, float angleRad)
        {
            Vector3 newPos = new Vector3(
                Mathf.Cos(angleRad),
                Mathf.Sin(angleRad)) * radius;
            newPos.z = _depth;
            return newPos;
        }
    }
}