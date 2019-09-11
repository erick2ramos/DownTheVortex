using UnityEngine;
using System.Collections;

namespace BaseSystems.Feedback
{
    public class ParticleFeedback : Feedback
    {
        public ParticleSystem BoundedParticleSystem;

        protected override void CustomInitialize(GameObject owner)
        {
            BoundedParticleSystem?.Stop();
        }

        protected override void CustomPlay()
        {
            BoundedParticleSystem?.Play();
        }

        protected override void CustomStop()
        {
            BoundedParticleSystem?.Stop();
        }

        protected override void CustomRestart()
        {
            BoundedParticleSystem?.Stop();
        }
    }
}