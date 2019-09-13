using UnityEngine;
using System.Collections.Generic;
using Gameplay.Obstacles;
using Gameplay.Ability;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Config")]
    public class GameConfig : ScriptableObject
    {
        public int MinPoolSize = 10;
        public int MaxPoolSize = 20;
        public int ScorePerStep = 1;
        public int CollectableValue = 1;
        public float StepSpawnTime = 1;
        public float OverallSpeed = 1;
        public float VortexRadius = 8;
        public List<ObstacleStep> ObstaclesPatterns;
        public List<ObstacleStep> CollectablesPatterns;
        public Character PlayerPrefab;
        public Vector3 PlayerStartingPosition;
        public List<Color> ValidColors;
        public AbilitiesConfig AbilitiesConfig;
    }
}