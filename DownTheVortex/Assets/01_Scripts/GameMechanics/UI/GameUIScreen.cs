using UnityEngine;
using System.Collections;

namespace Gameplay.UI
{
    [RequireComponent(typeof(Animator), typeof(CanvasGroup))]
    public class GameUIScreen : MonoBehaviour
    {
        protected Animator _animator;

        public virtual void Init()
        {
            _animator = GetComponent<Animator>();
            gameObject.SetActive(false);
        }
        public virtual IEnumerator Activate()
        {
            gameObject.SetActive(true);
            _animator.SetTrigger("Activate");
            yield return _animator.WaitForTagedAnimationEnd("Active", null);

        }
        public virtual IEnumerator Deactivate()
        {
            _animator.SetTrigger("Deactivate");
            yield return _animator.WaitForTagedAnimationEnd("Deactive", null);
            gameObject.SetActive(false);
        }
    }
}