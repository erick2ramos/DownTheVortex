﻿using BaseSystems.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Obstacles;
using Gameplay.UI;
using BaseSystems.Managers;
using BaseSystems.DataPersistance;
using Gameplay.Ability;

namespace Gameplay {
    public enum GameState
    {
        Initializing,
        Ready,
        Playing,
        Paused,
        Gameover
    }

    /// <summary>
    /// Handles the game flow
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        public event System.Action OnGameOver;
        public event System.Action<bool> OnPause;
        public event System.Action<int> OnScoreUpdated;
        public event System.Action<int> OnCollectableUpdated;

        public GameConfig GameConfig;
        public int CurrentScore { get; set; }
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
                    // Controls when an obstacle or a collectable has to be spawned
                    _timer += Time.deltaTime;
                    _collectableTimer += Time.deltaTime;
                    float collectableSpawnTime = GameConfig.StepSpawnTime * 0.5f;

                    if (_collectableTimer > collectableSpawnTime && _timer < GameConfig.StepSpawnTime)
                    {
                        _collectableTimer = 0;
                        if (Random.value > 0.8f)
                        {
                            // Activate a collectable
                            _factory.NextCollectable();
                        }
                    }

                    if (_timer > GameConfig.StepSpawnTime)
                    {
                        // Activate an obstacle
                        _factory.Next();
                        _timer = 0;
                    }
                    break;
            }
        }

        /// <summary>
        /// Sets the game into play mode
        /// </summary>
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

        /// <summary>
        /// Called every time the player manages to pass an obstacle
        /// </summary>
        public void AddScore()
        {
            CurrentScore += GameConfig.ScorePerStep;
            // Show score feedback
            // Update ui
            OnScoreUpdated?.Invoke(CurrentScore);
        }

        /// <summary>
        /// Called when the player collects a collectable
        /// </summary>
        public void AddCollectable()
        {
            DataPersistanceManager.PlayerData.CurrentCurrency += GameConfig.CollectableValue;
            CurrentCollectable = DataPersistanceManager.PlayerData.CurrentCurrency;
            OnCollectableUpdated?.Invoke(CurrentCollectable);
        }

        /// <summary>
        /// Show the gameover screen
        /// </summary>
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