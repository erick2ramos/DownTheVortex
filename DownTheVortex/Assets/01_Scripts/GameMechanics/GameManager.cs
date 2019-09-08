using BaseSystems.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Obstacles;
using Gameplay.UI;

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
        public float VortexRadius = 8;
        public List<ObstacleStep> ObstaclesPatterns;
        public PlayerController PlayerPrefab;
        public Vector3 PlayerStartingPosition;
    }

    public class GameManager : Singleton<GameManager>
    {
        public event System.Action OnGameOver;
        public event System.Action<bool> OnPause;
        public event System.Action<int> OnScoreUpdated;

        public GameConfig GameConfig;
        public int CurrentScore { get; set; }
        public Transform ObstaclesHolder;
        public Transform WorldHolder;
        public MeshRenderer EnvironmentRenderer;
        public GameUIManager UIManager;
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
            CurrentPlayer.Init(GameConfig.VortexRadius, 
                GameConfig.PlayerStartingPosition.z);
            UIManager.Init();
            UIManager.ShowScreen("MainMenu", () => 
            {
                CurrentState = GameState.Ready;
            });
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
                case GameState.Ready:
                    if (Input.GetMouseButtonDown(0))
                    {
                        UIManager.ShowScreen("GameHud", () =>
                        {
                            CurrentPlayer.Activate();
                            EnvironmentRenderer.material.SetVector("_Velocity", new Vector4(0, -1));
                            EnvironmentRenderer.material.SetFloat("_Speed", GameConfig.OverallSpeed * 0.5f);
                            CurrentState = GameState.Playing;
                        });
                    }
                    break;
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
            // Show score feedback
            // Update ui
            OnScoreUpdated?.Invoke(CurrentScore);
        }

        public void Pause()
        {
            if(CurrentState == GameState.Paused)
            {
                // Set playing
                UIManager.ShowScreen("GameHud", () =>
                {
                    EnvironmentRenderer.material.SetFloat("_Speed", GameConfig.OverallSpeed * 0.5f);
                    CurrentPlayer.Activate();
                    CurrentState = GameState.Playing;
                    OnPause?.Invoke(false);
                });
            } else if (CurrentState == GameState.Playing)
            {
                //Set pause
                CurrentState = GameState.Paused;
                EnvironmentRenderer.material.SetFloat("_Speed", 0);
                CurrentPlayer.Deactivate();
                OnPause?.Invoke(true);
                UIManager.ShowScreen("PauseMenu");
            }
        }

        public void GameOver()
        {
            EnvironmentRenderer.material.SetFloat("_Speed", 0);
            OnGameOver?.Invoke();
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
            Application.Quit();
#endif
        }
    }
}