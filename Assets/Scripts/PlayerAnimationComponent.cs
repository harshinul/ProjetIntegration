using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationComponent : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ActivateFirstAttack()
    {
        animator.SetBool("isFirstAttacking", true);
    }

    public void DeactivateFirstAttack()
    {
        animator.SetBool("isFirstAttacking", false);
    }

    public void ActivateSecondAttack()
    {
        animator.SetBool("isSecondAttacking", true);
    }

    public void DeactivateSecondAttack()
    {
        animator.SetBool("isSecondAttacking", false);
    }

    public void ActivateRunning()
    {
        animator.SetBool("isRunning", true);
    }

    public void DeactivateRunning()
    {
        animator.SetBool("isRunning", false);
    }

    public void ActivateTakingDamage()
    {
        animator.SetBool("isTakingDamage", true);
    }

    public void ActivateJumping()
    {
        animator.SetBool("startJump", true);
    }

    public void DeactivateJumping()
    {
        animator.SetBool("startJump", false);
    }

    public void ActivateFalling()
    {
        animator.SetBool("isFalling", true);
    }
    public void DeactivateFalling()
    {
        animator.SetBool("isFalling", false);
    }
    public void DeactivateTakingDamage()
    {
        animator.SetBool("isTakingDamage", false);
    }

    public void ActivateDeath()
    {
        animator.SetTrigger("isDead");
    }
}
