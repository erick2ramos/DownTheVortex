using BaseSystems.DataPersistance;
using BaseSystems.Managers;
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
    }
}