using UnityEngine;
using System.Collections;
using BaseSystems.Audio;
using BaseSystems.Managers;

namespace BaseSystems.Feedback {
    public class SoundFeedback : Feedback
    {
        public AudioID SFXID;

        protected override void CustomPlay()
        {
            ManagerHandler.Get<AudioManager>().PlaySFX(SFXID);
        }
    }
}