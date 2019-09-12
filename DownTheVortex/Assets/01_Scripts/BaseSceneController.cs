using BaseSystems.DataPersistance;
using BaseSystems.Managers;
using Store;
using System.Collections;
using UnityEngine;

namespace BaseSystems.SceneHandling
{
    public class BaseSceneModel : SceneModel
    {

    }

    public class BaseSceneController : SceneController<BaseSceneModel>
    {
        public override IEnumerator Initialization()
        {
            Gameplay.GameManager.Instance.Init();

            yield return null;

        }

        public void OpenStore()
        {
            StoreSceneModel model = new StoreSceneModel();
            ManagerHandler.Get<SceneTransitionManager>().LoadScene(SceneIndex.StoreScene, model);
        }
    }
}