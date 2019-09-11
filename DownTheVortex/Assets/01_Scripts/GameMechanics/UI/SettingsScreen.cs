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
        InputField _inputField;
        bool _isMuted;

        public override void Init()
        {
            _isMuted = DataPersistanceManager.PlayerData.SoundPreferences.IsMuted;
            _muteSFXToggle.isOn = !_isMuted;
            _inputField.text = GameManager.Instance.MovementMultiplier.ToString();
            base.Init();
        }

        private void OnValueChanged (string input)
        {
            float newValue = (float)System.Convert.ToDouble(input);
            GameManager.Instance.MovementMultiplier = newValue;
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
            OnValueChanged(_inputField.text);
            ManagerHandler.Get<DataPersistanceManager>().Save();
            // Back to last screen
            GameManager.Instance.UIManager.ShowScreen("MainMenu");
        }
    }
}
