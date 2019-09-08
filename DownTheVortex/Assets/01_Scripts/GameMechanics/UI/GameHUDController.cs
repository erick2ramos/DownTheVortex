using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class GameHUDController : GameUIScreen
    {
        [SerializeField]
        Text _scoreAmount, _collectablesAmount;
        public override IEnumerator Activate()
        {
            GameManager.Instance.OnScoreUpdated -= UpdateScore;
            GameManager.Instance.OnScoreUpdated += UpdateScore;
            return base.Activate();
        }

        public override IEnumerator Deactivate()
        {
            GameManager.Instance.OnScoreUpdated -= UpdateScore;
            return base.Deactivate();
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