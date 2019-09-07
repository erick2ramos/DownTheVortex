using BaseSystems.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UInput = UnityEngine.Input;

namespace BaseSystems.Input
{
    public class InputManager : Manager
    {
        EventSystem _eventSystem;

        public event Action<TouchInputEvent> OnTouchStart;
        public event Action<TouchInputEvent> OnTouchStay;
        public event Action<TouchInputEvent> OnTouchRelease;

        public override void Initialize()
        {
            _eventSystem = GetComponent<EventSystem>();
        }

        private void Update()
        {
            if (UInput.touchCount > 0)
            {
                // Only polling touch 0
                Touch touch = UInput.GetTouch(0);
                TouchInputEvent tie = new TouchInputEvent()
                {
                    touchPos = touch.position,
                    touchDelta = touch.deltaPosition
                };
                
                // Send the respective event depending on touch phase state
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        OnTouchStart?.Invoke(tie);
                        break;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        OnTouchStay?.Invoke(tie);
                        break;
                    case TouchPhase.Canceled:
                    case TouchPhase.Ended:
                        OnTouchRelease?.Invoke(tie);
                        break;
                }
            }
        }
    }
}