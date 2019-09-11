using BaseSystems.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Obstacles;
using Gameplay.UI;
using BaseSystems.Managers;
using BaseSystems.DataPersistance;

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
        public int CollectableValue = 1;
        public float StepSpawnTime = 1;
        public float OverallSpeed = 1;
        public float VortexRadius = 8;
        public List<ObstacleStep> ObstaclesPatterns;
        public List<ObstacleStep> CollectablesPatterns;
        public Character PlayerPrefab;
        public Vector3 PlayerStartingPosition;
        public List<Color> ValidColors;
    }

    public class GameManager : Singleton<GameManager>
    {
        public event System.Action OnGameOver;
        public event System.Action<bool> OnPause;
        public event System.Action<int> OnScoreUpdated;
        public event System.Action<int> OnCollectableUpdated;

        public GameConfig GameConfig;
        public int CurrentScore { get; set; }
        public float MovementMultiplier = 1;
        public Transform ObstaclesHolder;
        public Transform WorldHolder;
        public MeshRenderer EnvironmentRenderer;
        public GameUIManager UIManager;
        public GameState CurrentState { get; private set; }
        public Character CurrentPlayer { get; private set; }
        public int CurrentCollectable { get; private set; }
        public int PreviousHighScore { get; private set; }

        SpawnFactory _factory;

        float _timer;
        private float _collectableTimer;

        public void Init()
        {
            CurrentScore = 0;
            _timer = 0;
            _factory = new SpawnFactory(GameConfig, ObstaclesHolder);

            CurrentPlayer = Instantiate(GameConfig.PlayerPrefab, WorldHolder, false);
            CurrentPlayer.Init(GameConfig.VortexRadius, 
                GameConfig.PlayerStartingPosition.z);

            UIManager.Init();
            UIManager.ShowScreen("MainMenu", () => 
            {
                CurrentState = GameState.Ready;
            });
        }

        public void Update()
        {
            switch (CurrentState)
            {
                case GameState.Playing:
                    _timer += Time.deltaTime;
                    _collectableTimer += Time.deltaTime;
                    float collectableSpawnTime = GameConfig.StepSpawnTime * 0.5f;

                    if (_collectableTimer > collectableSpawnTime && _timer < GameConfig.StepSpawnTime)
                    {
                        _collectableTimer = 0;
                        if (Random.value > 0.8f)
                        {
                            _factory.NextCollectable();
                        }
                    }

                    if (_timer > GameConfig.StepSpawnTime)
                    {
                        _factory.Next();
                        _timer = 0;
                    }
                    break;
            }
        }

        public void Play()
        {
            if(CurrentState == GameState.Ready)
            {
                CurrentPlayer.Activate();
                EnvironmentRenderer.material.SetVector("_Velocity", new Vector4(0, -1));
                EnvironmentRenderer.material.SetFloat("_Speed", GameConfig.OverallSpeed * 0.2f * Time.deltaTime);
                CurrentState = GameState.Playing;
            }
        }

        public void AddScore()
        {
            CurrentScore += GameConfig.ScorePerStep;
            // Show score feedback
            // Update ui
            OnScoreUpdated?.Invoke(CurrentScore);
        }

        public void AddCollectable()
        {
            DataPersistanceManager.PlayerData.CurrentCurrency += GameConfig.CollectableValue;
            CurrentCollectable = DataPersistanceManager.PlayerData.CurrentCurrency;
            OnCollectableUpdated?.Invoke(CurrentCollectable);
        }

        public void GameOver()
        {
            PreviousHighScore = DataPersistanceManager.PlayerData.CurrentHighScore;
            DataPersistanceManager.PlayerData.CurrentHighScore = Mathf.Max(DataPersistanceManager.PlayerData.CurrentHighScore, CurrentScore);
            CurrentState = GameState.Gameover;
            EnvironmentRenderer.material.SetFloat("_Speed", 0);
            OnGameOver?.Invoke();
            UIManager.ShowScreen("GameOverMenu");
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