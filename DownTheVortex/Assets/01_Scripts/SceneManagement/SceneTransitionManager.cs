using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BaseSystems.Managers;


namespace BaseSystems.SceneHandling
{
    public class SceneTransitionManager : Manager
    {
        private List<LoadedScene> _loadedScenes;
        private LoadedScene _activeScene;

        public event Action OnLoadingStart;
        public event Action<float> OnLoadingUpdate;

        public event Action OnLoadingFinished;

        public bool IsLoading { get; private set; }

        public override void Initialize()
        {
            _loadedScenes = new List<LoadedScene>();
        }

        public void LoadScene(SceneIndex scene, SceneModel model)
        {
            StartCoroutine(LoadMainScene(scene, model));
        }

        public void AddScene(SceneIndex scene, SceneModel model)
        {
            StartCoroutine(LoadAdditiveScene(scene, model));
        }
        public void ReloadScene()
        {
            StartCoroutine(LoadMainScene(_activeScene.SceneIdentifier, _activeScene.Model));
        }

        private IEnumerator LoadMainScene(SceneIndex scene, SceneModel model)
        {
            OnLoadingStart?.Invoke();
            IsLoading = true;

            // Make sure to clean up the current active scene before destroying it
            if (_activeScene != null)
                _activeScene.Controller.BeforeDestroy();

            // Start the scene loading asyncronous operation
            AsyncOperation loadSceneJob = SceneManager.LoadSceneAsync(scene.ToString());
            while (!loadSceneJob.isDone)
            {
                yield return null;
                // Callback to update based on the async task progress
                OnLoadingUpdate?.Invoke(loadSceneJob.progress);
            }

            _loadedScenes.Clear();

            GC.Collect();

            Scene loadedScene = SceneManager.GetSceneByName(scene.ToString());
            if (loadedScene.isLoaded)
            {
                bool controllerFound = false;
                // Get the scene object to initialize the scene using the ISceneController interface
                GameObject[] rootObjects = loadedScene.GetRootGameObjects();
                foreach (GameObject rootObject in rootObjects)
                {
                    // Try to get the scene controller so we can initialize the scene
                    ISceneController sceneController = rootObject.GetComponent<ISceneController>();

                    // This object didn't have the scene controller
                    if (sceneController == null)
                    {
                        continue;
                    }

                    // Create a loaded scene handle
                    LoadedScene managedLoadedScene = new LoadedScene(sceneController, model, rootObject, scene);
                    _loadedScenes.Add(managedLoadedScene);

                    _activeScene = managedLoadedScene;
                    sceneController.BaseModel = model;
                    yield return sceneController.Initialization();

                    sceneController.AfterInitialization();
                    controllerFound = true;
                }

                if (!controllerFound)
                {
                    Debug.LogError("[SceneTransitionManager] Could not find any object with component SceneController in scene: "
                                    + loadedScene.name);
                }
            }

            yield return new WaitForSeconds(1);
    
            OnLoadingFinished?.Invoke();
            IsLoading = false;
        }

        private IEnumerator LoadAdditiveScene(SceneIndex scene, SceneModel model)
        {
            OnLoadingStart?.Invoke();
            IsLoading = true;

            AsyncOperation loadSceneJob = SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive);
            while (!loadSceneJob.isDone)
            {
                yield return null;
                // Callback to update based on the async task progress
                OnLoadingUpdate?.Invoke(loadSceneJob.progress);
            }

            // Scene was done loading
            Scene loadedScene = SceneManager.GetSceneByName(scene.ToString());

            if (loadedScene.isLoaded)
            {
                SceneManager.MergeScenes(loadedScene, SceneManager.GetActiveScene());

                bool controllerFound = false;
                // Get the scene object to initialize the scene using the ISceneController interface
                GameObject[] rootObjects = loadedScene.GetRootGameObjects();
                foreach (GameObject rootObject in rootObjects)
                {
                    // Try to get the scene controller so we can initialize the scene
                    SceneController<SceneModel> sceneController = rootObject.GetComponent<SceneController<SceneModel>>();

                    // This object didn't have the scene controller
                    if (sceneController == null)
                    {
                        continue;
                    }

                    sceneController.BaseModel = model;

                    LoadedScene managedLoadedScene = new LoadedScene(sceneController, model, rootObject, scene);
                    _loadedScenes.Add(managedLoadedScene);

                    yield return sceneController.Initialization();

                    sceneController.AfterInitialization();

                    controllerFound = true;
                }

                if (!controllerFound)
                {
                    Debug.LogError("[SceneTransitionManager] Could not find any object with component ISceneController in scene: "
                                    + loadedScene.name);
                }
            }

            OnLoadingFinished?.Invoke();
            IsLoading = false;
        }

        public void CloseAdditiveScene(ISceneController controller)
        {
            // Search a loaded scene by it's controller reference and destroy the root gameobject
            LoadedScene loadedScene = _loadedScenes.Find(x => x.Controller == controller);

            if(loadedScene != null)
            {
                // Callback before destroying the loaded scene
                loadedScene.Controller.BeforeDestroy();
                DestroyImmediate(loadedScene.Scene);
                _loadedScenes.Remove(loadedScene);
                loadedScene = null;
                GC.Collect();

                // Try to focus the last scene loaded
                // Assert(_loadedScenes.Count should always be greater than 0)
                if (_loadedScenes.Count > 0)
                {
                    _loadedScenes[_loadedScenes.Count - 1].Controller.OnFocus();
                }
            }
        }
    }
}