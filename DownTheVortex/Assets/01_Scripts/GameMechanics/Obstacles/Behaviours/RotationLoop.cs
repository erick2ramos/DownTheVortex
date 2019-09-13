using UnityEngine;
using System.Collections;

namespace Gameplay.Obstacles
{
    /// <summary>
    /// Rotates the obstacle in a random direction indefinetly
    /// </summary>
    public class RotationLoop : ObstacleBehaviour
    {
        public float RotationAnglePerSecond;
        public float AxisSign;

        public override void Setup()
        {
            AxisSign = Random.value >= 0.5f ? 1 : -1;
        }

        public override void HandleStep()
        {
            _step.Pivot.transform.localRotation *= Quaternion.Euler(0, 0, AxisSign * RotationAnglePerSecond * Time.deltaTime);
        }
    }
}