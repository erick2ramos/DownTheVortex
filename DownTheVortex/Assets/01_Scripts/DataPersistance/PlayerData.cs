using UnityEngine;
using System.Collections;
using BaseSystems.Audio;

namespace BaseSystems.DataPersistance
{
    [System.Serializable]
    public class PlayerData
    {
        public int CurrentHighScore;
        public int CurrentCurrency;
        public SoundPreferences SoundPreferences;

        public PlayerData()
        {
            SoundPreferences = new SoundPreferences();
        }
    }
}