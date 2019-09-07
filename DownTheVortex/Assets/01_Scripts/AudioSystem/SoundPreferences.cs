using UnityEngine;
using System.Collections;

namespace BaseSystems.Audio
{
    [System.Serializable]
    public class SoundPreferences
    {
        public float MasterVolume = 1;
        public float SFXVolume = 1;
        public bool IsMuted = false;
    }
}