using UnityEngine;
using System.Collections;

namespace BaseSystems.SceneHandling
{
    public class LoadedScene
    {
        public ISceneController Controller { get; private set; }
        public SceneModel Model { get; private set; }
        public GameObject Scene { get; private set; }
        public SceneIndex SceneIdentifier { get; private set; }

        public LoadedScene(ISceneController controller, SceneModel model, GameObject sceneRoot, SceneIndex id)
        {
            Controller = controller;
            Model = model;
            SceneIdentifier = id;
            Scene = sceneRoot;
        }
    }
}