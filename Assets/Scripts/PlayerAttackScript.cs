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
                    heavyDamage = 9f,
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
                    attackSpeed = 0.7f,
                    dashPower = 15f,
                    speed = 4f
                };
            case ClassType.Assassin:
                return new CharacterStats
                {
                    health = 80f,
                    lightDamage = 8f,
                    heavyDamage = 10f,
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

    public ClassType classType;
    private PlayerHealthComponent player;
    private CharacterStats characterStats;

    //Attack
    WeaponScript weapon;
    float attack1Duration;
    float attack2Duration;
    bool isAttacking = false;

    //input
    bool wantsToAttack1 = false;
    bool wantsToAttack2 = false;
    void Awake()
    {
        playerMovementComponent = GetComponent<PlayerMovementComponent>();
        playerAnimationComponent = GetComponent<PlayerAnimationComponent>();
        player = GetComponent<PlayerHealthComponent>();
        weapon = GetComponentInChildren<WeaponScript>();
        weapon.player = this.gameObject;

        characterStats = CharacterStats.GetStatsForClass(classType);

        attack1Duration = playerAnimationComponent.GetAttack1Duration();
        attack2Duration = playerAnimationComponent.GetAttack2Duration();
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

        isAttacking = false;
        wantsToAttack1 = false;
    }

    public IEnumerator CouroutineStartAttack2()
    {
        float beginingAnimationTime = (attack1Duration / characterStats.attackSpeed) / 2f;
        float endAnimationTime = beginingAnimationTime;
        isAttacking = true;
        playerMovementComponent.StopMovement();
        playerAnimationComponent.ActivateSecondAttack();

        weapon.damage = characterStats.heavyDamage;

        yield return new WaitForSeconds(beginingAnimationTime);
        playerMovementComponent.ResumeMovement();
        yield return new WaitForSeconds(endAnimationTime);
        playerAnimationComponent.DeactivateSecondAttack();

        isAttacking = false;
        wantsToAttack2 = false;
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

    //private void OnTriggerEnter(Collider collider)
    //{
    //    if (collider.CompareTag("Player"))
    //    {
    //        player.TakeDamage(characterStats.damage);
    //    }
    //}
}
