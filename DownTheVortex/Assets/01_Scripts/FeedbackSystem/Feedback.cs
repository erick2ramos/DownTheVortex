using UnityEngine;
using System.Collections;

namespace BaseSystems.Feedback
{
    public abstract class Feedback : MonoBehaviour
    {
        public bool IsActive = true;
        public GameObject Owner { get; set; }
        public float StartingTimeOffset = 0;

        Coroutine _playCoroutine;

        public void Initialize(GameObject owner)
        {
            if (!IsActive) return;
            Owner = owner;
        }

        public void Play()
        {
            if (!IsActive) return;
            if (StartingTimeOffset > 0)
            {
                _playCoroutine = StartCoroutine(PlayCoroutine());
            } else
            {
                CustomPlay();
            }
        }

        public void Stop()
        {
            if (!IsActive) return;
            if (_playCoroutine != null) { StopCoroutine(_playCoroutine); }

            CustomStop();
        }

        public void Restart()
        {
            if (!IsActive) return;
            CustomRestart();
        }

        IEnumerator PlayCoroutine()
        {
            yield return new WaitForSeconds(StartingTimeOffset);

            CustomPlay();
        }

        #region Custom behaviours
        protected virtual void CustomInitialize(GameObject owner) { }
        protected abstract void CustomPlay();
        protected virtual void CustomStop() { }
        protected virtual void CustomRestart() { }

        #endregion
    }
}