using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BaseSystems.DataPersistance;

namespace Gameplay.UI
{
    public class MainMenuScreen : GameUIScreen
    {
        [SerializeField]
        Button _settingsButton;

        [SerializeField]
        Animator _ftueAnimator;

        bool _alreadyStarted;
        private bool _blocked;

        public override void Init()
        {
            base.Init();
            _settingsButton.onClick.AddListener(Settings);
        }

        public override IEnumerator Activate()
        {
            _alreadyStarted = false;
            _blocked = true;
            yield return base.Activate();

            _blocked = false;
            int activeAbility = DataPersistanceManager.PlayerData.ActiveAbility;
            _ftueAnimator.SetInteger("Ability", activeAbility);
        }

        public override IEnumerator Deactivate()
        {
            return base.Deactivate();
        }

        public void Play()
        {
            if (_alreadyStarted || _blocked)
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
