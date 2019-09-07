using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Obstacles
{
    public class ObstacleController : MonoBehaviour
    {
        float _depth = 0;

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