using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BaseSystems.DataPersistance;
using BaseSystems.Managers;
using BaseSystems.Audio;

namespace Gameplay.UI
{
    public class SettingsScreen : GameUIScreen
    {
        [SerializeField]
        Toggle _muteSFXToggle;
        bool _isMuted;

        public override void Init()
        {
            _isMuted = DataPersistanceManager.PlayerData.SoundPreferences.IsMuted;
            _muteSFXToggle.isOn = !_isMuted;
            base.Init();
        }

        public void ToggleSound(bool toggle)
        {
            AudioManager SoundManager = ManagerHandler.Get<AudioManager>();
            _isMuted = !toggle;
            DataPersistanceManager.PlayerData.SoundPreferences.IsMuted = _isMuted;
            SoundManager.MuteSFX(_isMuted);
        }

        public void Close()
        {
            ManagerHandler.Get<DataPersistanceManager>().Save();
            // Back to last screen
            GameManager.Instance.UIManager.ShowScreen("MainMenu");
        }
    }
}
