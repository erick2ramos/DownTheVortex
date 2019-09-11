using UnityEngine;
using System.Collections;
using BaseSystems.Managers;

namespace BaseSystems.Input
{
    public class InputListener : MonoBehaviour
    {
        InputManager _inputManager;

        protected virtual void OnEnable()
        {
            _inputManager = ManagerHandler.Get<InputManager>();
            _inputManager.OnTouchStart -= OnTouchStart;
            _inputManager.OnTouchStart += OnTouchStart;
            _inputManager.OnTouchStay -= OnTouchStay;
            _inputManager.OnTouchStay += OnTouchStay;
            _inputManager.OnTouchRelease -= OnTouchRelease;
            _inputManager.OnTouchRelease += OnTouchRelease;
        }

        protected virtual void OnDisable()
        {
            _inputManager.OnTouchStart -= OnTouchStart;
            _inputManager.OnTouchStay -= OnTouchStay;
            _inputManager.OnTouchRelease -= OnTouchRelease;
        }

        protected virtual void OnTouchStart(TouchInputEvent input)
        {

        }

        protected virtual void OnTouchStay(TouchInputEvent input)
        {

        }

        protected virtual void OnTouchRelease(TouchInputEvent input)
        {

        }
    }
}