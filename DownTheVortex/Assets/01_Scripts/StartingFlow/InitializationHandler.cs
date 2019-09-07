using UnityEngine;
using System.Collections;
using BaseSystems.Managers;
using BaseSystems.SceneHandling;

public class InitializationHandler : MonoBehaviour
{
    ManagerHandler handler;

    IEnumerator Start()
    {
        Application.targetFrameRate = 500;
        handler = FindObjectOfType<ManagerHandler>();

        handler.Init();

        yield return new WaitForSeconds(0.25f);

        BaseSceneModel model = new BaseSceneModel();
        ManagerHandler.Get<SceneTransitionManager>().LoadScene(SceneIndex.GameScene, model);
    }
}
