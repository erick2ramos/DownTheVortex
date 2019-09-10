using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Obstacles;

namespace Gameplay
{
    public class SpawnFactory
    {
        class ObstaclePoolID
        {
            public int PoolId;
            public ObstacleStep Step;
        }

        GameConfig _config;
        Queue<ObstaclePoolID> _pool;
        List<Queue<ObstaclePoolID>> _typedPool;
        Queue<ObstacleStep> _collectablePool;
        Transform _obstaclesParent;
        int _currentPool = 0;
        int _activeCount = 0;
        int _maxActivePerPool = 0;

        public SpawnFactory(GameConfig config, Transform obstaclesParent)
        {
            _config = config;
            _typedPool = new List<Queue<ObstaclePoolID>>();
            _pool = new Queue<ObstaclePoolID>();
            _collectablePool = new Queue<ObstacleStep>();
            _obstaclesParent = obstaclesParent;
            FillPool();
            _currentPool = Random.Range(0, _typedPool.Count);
            // 80% of a pool can be used before switching to another pool
            // just in case the next pool is the same pool
            _maxActivePerPool = (int)(_config.MaxPoolSize * 0.8f);
        }

        public void FillPool()
        {
            int poolIndex = 0;
            foreach(var pattern in _config.ObstaclesPatterns)
            {
                _typedPool.Add(new Queue<ObstaclePoolID>());
                while (_typedPool[poolIndex].Count < _config.MaxPoolSize)
                {
                    ObstacleStep step = GameObject.Instantiate(_config.ObstaclesPatterns[poolIndex], _obstaclesParent, false);
                    step.gameObject.SetActive(false);
                    step.Init();
                    step.OnDestroyEvent += ResetObstacle;
                    _typedPool[poolIndex].Enqueue(new ObstaclePoolID()
                    {
                        PoolId = poolIndex,
                        Step = step
                    });
                }

                poolIndex++;
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
            ObstaclePoolID fullStep = _typedPool[_currentPool].Dequeue();
            _activeCount++;
            if (_activeCount >= _maxActivePerPool)
            {
                _activeCount = 0;
                _currentPool = Random.Range(0, _typedPool.Count);
            }
            fullStep.Step.Activate();
            _pool.Enqueue(fullStep);
            return fullStep.Step;
        }

        public CollectableStep NextCollectable()
        {
            ObstacleStep step = _collectablePool.Dequeue();
            step.Activate();
            return (CollectableStep)step;
        }

        private void ResetObstacle(ObstacleStep obstacle)
        {
            // step should be the same as the obstacle param
            ObstaclePoolID FullStep = _pool.Dequeue();
            FullStep.Step.transform.localPosition = Vector3.zero;
            FullStep.Step.gameObject.SetActive(false);
            FullStep.Step.Init();
            _typedPool[FullStep.PoolId].Enqueue(FullStep);
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