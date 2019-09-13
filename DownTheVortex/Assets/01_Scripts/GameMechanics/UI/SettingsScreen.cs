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
        [SerializeField]
        Toggle _vibrateToggle;
        bool _isMuted;
        int _cheatCounter;
        float _cheatTimer;

        public override void Init()
        {
            _isMuted = DataPersistanceManager.PlayerData.SoundPreferences.IsMuted;
            _muteSFXToggle.isOn = !_isMuted;
            _vibrateToggle.isOn = DataPersistanceManager.PlayerData.CanVibrate;
            base.Init();
        }

        public void ToggleSound(bool toggle)
        {
            AudioManager SoundManager = ManagerHandler.Get<AudioManager>();
            _isMuted = !toggle;
            DataPersistanceManager.PlayerData.SoundPreferences.IsMuted = _isMuted;
            SoundManager.MuteSFX(_isMuted);
        }

        public void ToggleVibration(bool toggle)
        {
            DataPersistanceManager.PlayerData.CanVibrate = toggle;
        }

        public void Close()
        {
            ManagerHandler.Get<DataPersistanceManager>().Save();
            // Back to last screen
            GameManager.Instance.UIManager.ShowScreen("MainMenu");
        }

        private void Update()
        {
            if (_cheatCounter > 0)
            {
                _cheatTimer += Time.deltaTime;
                if (_cheatTimer > 0.5f)
                {
                    _cheatCounter = 0;
                    _cheatTimer = 0;
                }
            }
        }

        /// <summary>
        /// To use the cheat you have to tap 5 times on the top right of the screen when in the settings screen,
        /// gives 100 currency/collectables
        /// </summary>
        public void Cheat()
        {
            _cheatCounter++;
            _cheatTimer = 0;
            if (_cheatCounter >= 5)
            {
                _cheatCounter = 0;
                DataPersistanceManager.PlayerData.CurrentCurrency += 100;
            }
        }
    }
}
