using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Gameplay.UI
{
    public class GameUIManager : MonoBehaviour
    {
        Dictionary<string, GameUIScreen> _registeredScreens;
        string _activeScreen;

        public void Init()
        {
            _registeredScreens = new Dictionary<string, GameUIScreen>();
            foreach(Transform child in transform)
            {
                var screen = child.GetComponent<GameUIScreen>();
                if(screen != null)
                {
                    _registeredScreens[screen.name] = screen;
                    screen.Init();
                }
            }
        }

        public void ShowScreen(string screenName, Action OnDone = null)
        {
            GameUIScreen screen;
            if (_registeredScreens.TryGetValue(screenName, out screen))
            {
                StartCoroutine(ShowScreenInternal(screenName, screen, OnDone));
            }
        }

        IEnumerator ShowScreenInternal(string screenName, GameUIScreen screen, Action OnDone)
        {
            if(!string.IsNullOrEmpty(_activeScreen))
                yield return _registeredScreens[_activeScreen].Deactivate();
            _activeScreen = null;
            yield return screen.Activate();
            _activeScreen = screenName;

            OnDone?.Invoke();
        }
    }
}