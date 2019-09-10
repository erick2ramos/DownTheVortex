using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay.Obstacles
{
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
                child.localPosition = 
                    new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * GameManager.Instance.GameConfig.VortexRadius;
                child.rotation = Quaternion.LookRotation(_step.Pivot.forward, _step.Pivot.position - child.position);
            }
        }
    }
}