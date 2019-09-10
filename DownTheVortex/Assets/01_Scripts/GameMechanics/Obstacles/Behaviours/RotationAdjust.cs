using UnityEngine;
using System.Collections;

namespace Gameplay.Obstacles
{
    public class RotationAdjust : ObstacleBehaviour
    {
        public Quaternion TargetAngle;
        public float RotationAnglePerSecond;

        public override void Setup()
        {
            TargetAngle = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }

        public override void HandleStep()
        {
            _step.Pivot.transform.localRotation = Quaternion.Slerp(_step.Pivot.transform.localRotation, TargetAngle, 3 * Time.deltaTime);
        }
    }
}