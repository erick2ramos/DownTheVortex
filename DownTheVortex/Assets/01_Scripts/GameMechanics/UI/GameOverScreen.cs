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
        public void Retry()
        {
            ManagerHandler.Get<DataPersistanceManager>().Save();
            ManagerHandler.Get<SceneTransitionManager>().ReloadScene();
        }

        public void Quit()
        {
            ManagerHandler.Get<DataPersistanceManager>().Save();
        }
    }
}