using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BaseSystems.DataPersistance;

namespace Gameplay.UI
{
    public class GameHUDController : GameUIScreen
    {
        [SerializeField]
        Text _scoreAmount, _collectablesAmount;
        [SerializeField]
        Image _activeAbilityIcon;

        public override IEnumerator Activate()
        {
            // Show ability icon if an ability has been equiped
            int activeAbility = DataPersistanceManager.PlayerData.ActiveAbility;
            if(activeAbility != 0)
                _activeAbilityIcon.sprite = GameManager.Instance.GameConfig.AbilitiesConfig.GetByID(activeAbility).AbilityHelperImage;
            _activeAbilityIcon.gameObject.SetActive(activeAbility != 0);
            _collectablesAmount.text = DataPersistanceManager.PlayerData.CurrentCurrency.ToString();
            _scoreAmount.text = GameManager.Instance.CurrentScore.ToString();

            GameManager.Instance.OnScoreUpdated -= UpdateScore;
            GameManager.Instance.OnScoreUpdated += UpdateScore;
            GameManager.Instance.OnCollectableUpdated -= UpdateCollectables;
            GameManager.Instance.OnCollectableUpdated += UpdateCollectables;

            return base.Activate();
        }

        public override IEnumerator Deactivate()
        {
            GameManager.Instance.OnScoreUpdated -= UpdateScore;
            GameManager.Instance.OnCollectableUpdated -= UpdateCollectables;
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