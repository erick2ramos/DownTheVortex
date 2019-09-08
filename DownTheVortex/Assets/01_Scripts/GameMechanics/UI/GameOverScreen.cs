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
        Text _scoreText;

        public override void Init()
        {
            base.Init();
        }

        public override IEnumerator Activate()
        {
            _scoreText.text = GameManager.Instance.CurrentScore.ToString();
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