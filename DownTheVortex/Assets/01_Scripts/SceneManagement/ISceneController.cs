using System.Collections;

namespace BaseSystems.SceneHandling
{
    public class SceneModel { }

    public interface ISceneController
    {
        SceneModel BaseModel { get; set; }

        IEnumerator Initialization();
        void AfterInitialization();
        void BeforeDestroy();
        void OnFocus();
    }
}