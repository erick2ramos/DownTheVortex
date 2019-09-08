using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class GameHUDController : GameUIScreen
    {
        [SerializeField]
        Text _scoreAmount, _collectablesAmount;

        [SerializeField]
        Button _pauseButton;

        public override IEnumerator Activate()
        {
            _pauseButton.onClick.AddListener(TogglePause);
            GameManager.Instance.OnScoreUpdated -= UpdateScore;
            GameManager.Instance.OnScoreUpdated += UpdateScore;
            return base.Activate();
        }

        public override IEnumerator Deactivate()
        {
            GameManager.Instance.OnScoreUpdated -= UpdateScore;
            _pauseButton.onClick.RemoveAllListeners();
            return base.Deactivate();
        }

        public void TogglePause()
        {
            GameManager.Instance.Pause();
        }

        public void UpdateScore(int amount)
        {
            _scoreAmount.text = amount.ToString();
        }

        public void UpdateCollectables(int amount)
        {
            _collectablesAmount.text = amount.ToString();
        }
    }
}