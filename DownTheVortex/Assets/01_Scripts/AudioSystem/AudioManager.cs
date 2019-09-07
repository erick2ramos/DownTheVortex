using UnityEngine;
using System.Collections;
using BaseSystems.Managers;
using System;
using System.Collections.Generic;

namespace BaseSystems.Audio
{
    public enum AudioID
    {
        BackgroundMusic = 0,
        Button0 = 1,
        Button1 = 2,
        Death0 = 3,
        Death1 = 4,
        Clear0 = 5,
        Clear1 = 6,
        Win = 7,
        Lose = 8
    }

    public class AudioManager : Manager
    {
        [Serializable]
        class AudioTuple
        {
            public AudioID Id;
            public AudioClip Sound;
        }

        public AudioSource MusicSource;
        public AudioSource SFXSource;
        [SerializeField]
        List<AudioTuple> _soundMap;

        public void PlayBackgroundMusic(AudioID audioId)
        {
            AudioTuple clip = _soundMap.Find(audio => audio.Id == audioId);
            if(clip != null)
            {
                MusicSource.clip = clip.Sound;
                MusicSource.Play();
            } else
            {
                GameLog.LogError("No sound configured for id: " + audioId.ToString());
            }
        }

        public void PlaySFX(AudioID audioId)
        {
            AudioTuple clip = _soundMap.Find(audio => audio.Id == audioId);
            if (clip != null)
            {
                SFXSource.PlayOneShot(clip.Sound);
            }
            else
            {
                GameLog.LogError("No sound configured for id: " + audioId.ToString());
            }
        }
    }
}