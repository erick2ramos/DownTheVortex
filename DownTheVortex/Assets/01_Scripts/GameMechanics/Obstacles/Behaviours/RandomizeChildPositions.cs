using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay.Obstacles
{
    /// <summary>
    /// Behaviour that calculates where all the obstacle pieces should
    /// be inside the vortex using circle coordinates
    /// </summary>
    public class RandomizeChildPositions : ObstacleBehaviour
    {
        public int AnglePartition;

        public override void Setup()
        {
            int childCount = _step.Pivot.childCount;
            Queue<int> piePieces = new Queue<int>();
            for (int i = 0; i < 360; i += AnglePartition)
                piePieces.Enqueue(i);
            piePieces = new Queue<int>(piePieces.OrderBy(x => Random.value));
            foreach (Transform child in _step.Pivot)
            {
                float angle = piePieces.Dequeue() * Mathf.Deg2Rad;
                // The position of the piece in circle coordinates
                child.localPosition = 
                    new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * GameManager.Instance.GameConfig.VortexRadius;
                // Rotate the piece so it's facing the pivot/center point
                child.rotation = Quaternion.LookRotation(_step.Pivot.forward, _step.Pivot.position - child.position);
            }
        }
    }
}