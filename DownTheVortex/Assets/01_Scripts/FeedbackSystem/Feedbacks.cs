using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseSystems.Feedback
{
    public class Feedbacks : MonoBehaviour
    {
        public bool AutoInitialize = false;
        public List<Feedback> RegisteredFeedbacks;

        private void Start()
        {
            if (AutoInitialize)
            {
                Initialize();
            }
        }

        public void Initialize()
        {
            Initialize(gameObject);
        }

        public void Initialize(GameObject owner)
        {
            foreach(var feedback in RegisteredFeedbacks)
            {
                feedback.Initialize(owner);
            }
        }

        public void PlayAll()
        {
            foreach (var feedback in RegisteredFeedbacks)
            {
                feedback.Play();
            }
        }

        public void StopAll()
        {
            foreach (var feedback in RegisteredFeedbacks)
            {
                feedback.Stop();
            }
        }

        public void RestartAll()
        {
            foreach (var feedback in RegisteredFeedbacks)
            {
                feedback.Restart();
            }
        }
    }
}