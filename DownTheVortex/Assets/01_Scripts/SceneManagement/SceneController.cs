using UnityEngine;
using System.Collections;

namespace BaseSystems.SceneHandling
{
    public abstract class SceneController<SceneModelType> : MonoBehaviour, ISceneController
        where SceneModelType : SceneModel
    {
        public SceneModel BaseModel { get { return _model; } set { _model = value as SceneModelType; } }
        private SceneModelType _model;
        public SceneModelType Model { get { return _model; } }

        public virtual void BeforeDestroy()
        {

        }

        public abstract IEnumerator Initialization();

        public virtual void AfterInitialization()
        {

        }

        public virtual void OnFocus()
        {
            
        }
    }
}