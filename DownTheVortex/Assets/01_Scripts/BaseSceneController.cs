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
            var DPManager = ManagerHandler.Get<DataPersistanceManager>();

            DataPersistanceManager.PlayerData.CurrentHighScore++;
            yield return new WaitForSeconds(1);
            DPManager.Save();

            GameLog.Log(DataPersistanceManager.PlayerData.CurrentHighScore);
        }
    }
}