using System.Collections;
using UnityEngine;

public static class AnimatorExtension {

    public static IEnumerator WaitForAnimationEnd(this Animator animator, string animationName, System.Action onAnimationEnd, float offset = 0)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + offset);

        if (onAnimationEnd != null)
            onAnimationEnd.Invoke();
    }

    public static IEnumerator WaitForTagedAnimationEnd(this Animator animator, string tagName, System.Action onAnimationEnd, float offset = 0)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsTag(tagName))
        {
            yield return null;
        }

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + offset);

        if (onAnimationEnd != null)
            onAnimationEnd.Invoke();
    }
}
