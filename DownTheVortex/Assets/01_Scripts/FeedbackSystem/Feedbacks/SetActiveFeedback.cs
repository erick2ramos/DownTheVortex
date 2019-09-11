using UnityEngine;
using System.Collections;

namespace BaseSystems.Feedback {
    public class SetActiveFeedback : Feedback
    {
        public GameObject BoundedObject;
        public bool InitState;
        public bool PlayState;
        public bool StopState;
        public bool RestartState;

        protected override void CustomInitialize(GameObject owner)
        {
            BoundedObject?.SetActive(InitState);
        }

        protected override void CustomPlay()
        {
            BoundedObject?.SetActive(PlayState);
        }

        protected override void CustomStop()
        {
            BoundedObject?.SetActive(StopState);
        }

        protected override void CustomRestart()
        {
            BoundedObject?.SetActive(RestartState);
        }
    }
}