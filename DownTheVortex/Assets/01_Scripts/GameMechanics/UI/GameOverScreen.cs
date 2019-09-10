using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BaseSystems.Managers;
using BaseSystems.DataPersistance;
using BaseSystems.SceneHandling;

namespace Gameplay.UI
{
    public class GameOverScreen : GameUIScreen
    {
        [SerializeField]
        Text _scoreText, _bestValueText, _currencyAmountText;

        [SerializeField]
        GameObject _newBestIcon;

        public override void Init()
        {
            base.Init();
        }

        public override IEnumerator Activate()
        {
            _scoreText.text = GameManager.Instance.CurrentScore.ToString();
            _newBestIcon.SetActive(GameManager.Instance.CurrentScore > GameManager.Instance.PreviousHighScore);
            _currencyAmountText.text = DataPersistanceManager.PlayerData.CurrentCurrency.ToString();
            _bestValueText.text = DataPersistanceManager.PlayerData.CurrentHighScore.ToString();
            return base.Activate();
        }

        public void Retry()
        {
            ManagerHandler.Get<DataPersistanceManager>().Save();
            ManagerHandler.Get<SceneTransitionManager>().ReloadScene();
        }

        public void Quit()
        {
            ManagerHandler.Get<DataPersistanceManager>().Save();
            GameManager.Instance.Quit();
        }
    }
}