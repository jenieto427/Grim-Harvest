using UnityEngine;

public class NPCAlienTallAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerYes()
    {
        animator.ResetTrigger("NoTrigger");
        animator.SetTrigger("YesTrigger");
    }

    public void TriggerNo()
    {
        animator.ResetTrigger("YesTrigger");
        animator.SetTrigger("NoTrigger");
    }
}
