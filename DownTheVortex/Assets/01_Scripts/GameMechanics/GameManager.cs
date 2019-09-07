using BaseSystems.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Obstacles;

namespace Gameplay {
    public enum GameState
    {
        Initializing,
        Ready,
        Playing,
        Paused,
        Gameover
    }

    [System.Serializable]
    public class GameConfig
    {
        public int MinPoolSize = 10;
        public int MaxPoolSize = 20;
        public int ScorePerStep = 1;
        public float StepSpawnTime = 1;
        public float OverallSpeed = 1;
        public List<ObstacleStep> ObstaclesPatterns;
        public PlayerController PlayerPrefab;
        public Vector3 PlayerStartingPosition;
    }

    public class GameManager : Singleton<GameManager>
    {
        public GameConfig GameConfig;
        public int CurrentScore { get; set; }
        public Transform ObstaclesHolder;
        public Transform WorldHolder;
        public MeshRenderer EnvironmentRenderer;
        public GameState CurrentState { get; private set; }
        public PlayerController CurrentPlayer { get; private set; }

        Queue<ObstacleStep> _obstaclesPool;
        Queue<ObstacleStep> _activeObstacles;
        float _timer;

        public void Init()
        {
            CurrentScore = 0;
            _timer = 0;
            _obstaclesPool = new Queue<ObstacleStep>();
            _activeObstacles = new Queue<ObstacleStep>();
            FillPool();
            CurrentPlayer = Instantiate(GameConfig.PlayerPrefab, WorldHolder, false);
            CurrentPlayer.Init(GameConfig.PlayerStartingPosition);
        }

        public void FillPool()
        {
            while (_obstaclesPool.Count < GameConfig.MaxPoolSize)
            {
                int patternIndex = Random.Range(0, GameConfig.ObstaclesPatterns.Count);
                ObstacleStep step = Instantiate(GameConfig.ObstaclesPatterns[patternIndex], ObstaclesHolder, false);
                step.gameObject.SetActive(false);
                step.Init();
                _obstaclesPool.Enqueue(step);
            }
        }

        public void Update()
        {
            switch (CurrentState)
            {
                case GameState.Playing:
                    _timer += Time.deltaTime;
                    if (_timer > GameConfig.StepSpawnTime)
                    {
                        ObstacleStep obstacle = _obstaclesPool.Dequeue();
                        obstacle.Activate();
                        _activeObstacles.Enqueue(obstacle);
                        if (_obstaclesPool.Count < GameConfig.MinPoolSize)
                        {
                            FillPool();
                        }
                        _timer = 0;
                    }
                    break;
            }
        }

        public void AddScore()
        {
            CurrentScore += GameConfig.ScorePerStep;
        }

        public void Pause()
        {
            if(CurrentState == GameState.Paused)
            {
                // Set playing
            } else if (CurrentState == GameState.Playing)
            {
                //Set pause
            }
        }

        public void Quit()
        {

        }
    }
}