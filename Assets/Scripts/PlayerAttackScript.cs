using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ClassType
{
    Warrior,
    Mage,
    Assassin,
}

public class CharacterStats
{
    public float health;
    public float lightDamage;
    public float heavyDamage;
    public float ultDamage;
    public float attackSpeed;
    public float speed;
    public float dashPower;

    public static CharacterStats GetStatsForClass(ClassType type)
    {
        switch (type)
        {
            case ClassType.Warrior:
                return new CharacterStats
                {
                    health = 150f,
                    lightDamage = 6f,
                    heavyDamage = 8f,
                    ultDamage = 20f,
                    attackSpeed = 1f,
                    dashPower = 20f,
                    speed = 5f
                };
            case ClassType.Mage:
                return new CharacterStats
                {
                    health = 100f,
                    lightDamage = 8f,
                    heavyDamage = 13f,
                    ultDamage = 30f,
                    attackSpeed = 0.7f,
                    dashPower = 15f,
                    speed = 4f
                };
            case ClassType.Assassin:
                return new CharacterStats
                {
                    health = 80f,
                    lightDamage = 8f,
                    heavyDamage = 12f,
                    ultDamage = 25f,
                    attackSpeed = 1.5f,
                    dashPower = 25f,
                    speed = 6f,

                };
            default:
                return null;
        }
    }
}
public class PlayerAttackScript : MonoBehaviour
{
    //Animation
    PlayerAnimationComponent playerAnimationComponent;

    //Mouvement
    PlayerMovementComponent playerMovementComponent;
    CharacterController characterController;

    public ClassType classType;
    private PlayerHealthComponent player;
    private CharacterStats characterStats;
    private UltimateAbilityComponent ultimateAbilityComponent;

    //Attack
    WeaponScript weapon;
    float attack1Duration;
    float attack2Duration;
    float ultDuration;
    bool isAttacking = false;
    public bool IsAttacking => isAttacking;

    //input
    bool wantsToAttack1 = false;
    bool wantsToAttack2 = false;
    bool wantsToUltimate = false;
    void Awake()
    {
        playerMovementComponent = GetComponent<PlayerMovementComponent>();
        playerAnimationComponent = GetComponent<PlayerAnimationComponent>();
        player = GetComponent<PlayerHealthComponent>();
        ultimateAbilityComponent = GetComponent<UltimateAbilityComponent>();
        characterController = GetComponent<CharacterController>();
        weapon = GetComponentInChildren<WeaponScript>();
        weapon.player = this.gameObject;

        characterStats = CharacterStats.GetStatsForClass(classType);

        attack1Duration = playerAnimationComponent.GetAttack1Duration();
        attack2Duration = playerAnimationComponent.GetAttack2Duration();
        ultDuration = playerAnimationComponent.GetUltDuration();
    }

    void CanDealDamage()
    {
        weapon.canDealDamage = true;
    }

    void CannotDealDamage()
    {
        weapon.canDealDamage = false;
    }


    public IEnumerator CouroutineStartAttack1()
    {
        float beginingAnimationTime = (attack1Duration / characterStats.attackSpeed) / 2f;
        float endAnimationTime = beginingAnimationTime;
        isAttacking = true;
        playerMovementComponent.StopMovement();
        playerAnimationComponent.ActivateFirstAttack();

        weapon.damage = characterStats.lightDamage;

        yield return new WaitForSeconds(beginingAnimationTime);
        playerMovementComponent.ResumeMovement();
        yield return new WaitForSeconds(endAnimationTime);
        playerAnimationComponent.DeactivateFirstAttack();

        ResetAttack();
    }

    public IEnumerator CouroutineStartAttack2()
    {
        float beginingAnimationTime = (attack2Duration / characterStats.attackSpeed) / 2f;
        float endAnimationTime = beginingAnimationTime;
        isAttacking = true;
        playerMovementComponent.StopMovement();
        playerAnimationComponent.ActivateSecondAttack();

        weapon.damage = characterStats.heavyDamage;

        yield return new WaitForSeconds(beginingAnimationTime);
        playerMovementComponent.ResumeMovement();
        yield return new WaitForSeconds(endAnimationTime);
        playerAnimationComponent.DeactivateSecondAttack();

        ResetAttack();
    }

    public IEnumerator CouroutineStartUltimate()
    {
        float beginingAnimationTime = ultDuration / 2f;
        float endAnimationTime = beginingAnimationTime;
        isAttacking = true;
        playerMovementComponent.StopMovement();
        playerAnimationComponent.ActivateUltimate();

        weapon.damage = characterStats.ultDamage;

        yield return new WaitForSeconds(beginingAnimationTime);
        playerMovementComponent.ResumeMovement();
        yield return new WaitForSeconds(endAnimationTime);
        playerAnimationComponent.DeactivateUltimate();

        ResetAttack();

    }

    void Update()
    {
        if (wantsToAttack1 && !isAttacking)
        {
            StartCoroutine(CouroutineStartAttack1());
        }
        else if (wantsToAttack2 && !isAttacking)
        {
            StartCoroutine(CouroutineStartAttack2());
        }
        else if(wantsToUltimate && !isAttacking && ultimateAbilityComponent.IsUltReady() && characterController.isGrounded)
        {
            StartCoroutine(CouroutineStartUltimate());
        }
        else
        {
            ResetAttack();
        }
    }

    public void Attack1(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            wantsToAttack1 = true;
        }

    }

    public void Attack2(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            wantsToAttack2 = true;
        }
    }

    public void UseUltimate(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            wantsToUltimate = true;
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
        wantsToAttack1 = false;
        wantsToAttack2 = false;
        wantsToUltimate = false;
    }
    public void TriggerLightAttack()
    {
        if (!isAttacking) wantsToAttack1 = true;
    }

    public void TriggerHeavyAttack()
    {
        if (!isAttacking) wantsToAttack2 = true;
    }
    //private void OnTriggerEnter(Collider collider)
    //{
    //    if (collider.CompareTag("Player"))
    //    {
    //        player.TakeDamage(characterStats.damage);
    //    }
    //}

}
