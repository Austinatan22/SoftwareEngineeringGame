using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimationOnce(string animationName)
    {
        animator.Play(animationName);
        // Optionally, disable the Animator component after playing the animation.
        animator.enabled = false;
    }
}
