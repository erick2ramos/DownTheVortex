using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class MainMenuScreen : GameUIScreen
    {
        [SerializeField]
        Button _settingsButton;
        bool _alreadyStarted;

        public override void Init()
        {
            base.Init();
            _settingsButton.onClick.AddListener(Settings);
        }

        public override IEnumerator Activate()
        {
            _alreadyStarted = false;
            return base.Activate();
        }

        public override IEnumerator Deactivate()
        {
            return base.Deactivate();
        }

        public void Play()
        {
            if (_alreadyStarted)
                return;
            _alreadyStarted = true;
            GameManager.Instance.UIManager.ShowScreen("GameHud", GameManager.Instance.Play);
        }

        public void Settings()
        {
            // Open the settings screen
            _alreadyStarted = true;
            GameManager.Instance.UIManager.ShowScreen("SettingsMenu");
        }
    }
}
