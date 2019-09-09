using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Obstacles;

namespace Gameplay
{
    public class SpawnFactory
    {
        GameConfig _config;
        Queue<ObstacleStep> _pool;
        Queue<ObstacleStep> _collectablePool;
        Transform _obstaclesParent;

        public SpawnFactory(GameConfig config, Transform obstaclesParent)
        {
            _config = config;
            _pool = new Queue<ObstacleStep>();
            _collectablePool = new Queue<ObstacleStep>();
            _obstaclesParent = obstaclesParent;
            FillPool();
        }

        public void FillPool()
        {
            while (_pool.Count < _config.MaxPoolSize)
            {
                int patternIndex = Random.Range(0, _config.ObstaclesPatterns.Count);
                ObstacleStep step = GameObject.Instantiate(_config.ObstaclesPatterns[patternIndex], _obstaclesParent, false);
                step.gameObject.SetActive(false);
                step.Init();
                step.OnDestroyEvent += ResetObstacle;
                _pool.Enqueue(step);
            }

            // We don't need more than 4 instances of collectables
            while (_collectablePool.Count < 4)
            {
                int patternIndex = Random.Range(0, _config.CollectablesPatterns.Count);
                ObstacleStep step = GameObject.Instantiate(_config.CollectablesPatterns[patternIndex], _obstaclesParent, false);
                step.gameObject.SetActive(false);
                step.Init();
                step.OnDestroyEvent += ResetCollectable;
                _collectablePool.Enqueue(step);
            }
        }

        public ObstacleStep Next()
        {
            ObstacleStep step = _pool.Dequeue();
            step.Activate();
            return step;
        }

        public CollectableStep NextCollectable()
        {
            ObstacleStep step = _collectablePool.Dequeue();
            step.Activate();
            return (CollectableStep)step;
        }

        private void ResetObstacle(ObstacleStep obstacle)
        {
            obstacle.transform.localPosition = Vector3.zero;
            obstacle.gameObject.SetActive(false);
            obstacle.Init();
            _pool.Enqueue(obstacle);
        }

        private void ResetCollectable(ObstacleStep collectable)
        {
            collectable.transform.localPosition = Vector3.zero;
            collectable.gameObject.SetActive(false);

            collectable.Init();
            _collectablePool.Enqueue(collectable);
        }
    }
}