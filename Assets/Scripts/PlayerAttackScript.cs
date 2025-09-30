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
    public float damage;
    public float attackSpeed;
    public float speed;
}
public class PlayerAttackScript : MonoBehaviour
{
    //Animation
    PlayerAnimationComponent playerAnimationComponent;

    //Mouvement
    PlayerMovementComponent playerMovementComponent;

    [SerializeField] ClassType classType;
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
    void Start()
    {
        playerMovementComponent = GetComponent<PlayerMovementComponent>();
        playerAnimationComponent = GetComponent<PlayerAnimationComponent>();
        player = GetComponent<PlayerHealthComponent>();
        weapon = GetComponentInChildren<WeaponScript>();
        weapon.player = this.gameObject;
        characterStats = GetStatsForClass(classType);

        attack1Duration = playerAnimationComponent.GetAttack1Duration();
        attack2Duration = playerAnimationComponent.GetAttack2Duration();
    }

    public CharacterStats GetStatsForClass(ClassType type)
    {
        switch (type)
        {
            case ClassType.Warrior:
                return new CharacterStats
                {
                    health = 150f,
                    damage = 7f,
                    attackSpeed = 1f,
                    speed = 5f
                };
            case ClassType.Mage:
                return new CharacterStats
                {
                    health = 100f,
                    damage = 13f,
                    attackSpeed = 0.7f,
                    speed = 4f
                };
            case ClassType.Assassin:
                return new CharacterStats
                {
                    health = 100f,
                    damage = 11f,
                    attackSpeed = 1.5f,
                    speed = 6f
                };
            default:
                return null;
        }
    }

    public IEnumerator CouroutineStartAttack1()
    {
        float beginingAnimationTime = (attack1Duration / characterStats.attackSpeed) / 2f;
        float endAnimationTime = beginingAnimationTime;
        isAttacking = true;
        playerMovementComponent.StopMovement();
        playerAnimationComponent.ActivateFirstAttack();

        weapon.canDealDamage = true;
        weapon.damage = characterStats.damage;

        yield return new WaitForSeconds(beginingAnimationTime);
        playerMovementComponent.ResumeMovement();
        yield return new WaitForSeconds(endAnimationTime);
        playerAnimationComponent.DeactivateFirstAttack();
        weapon.canDealDamage = false;

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

        weapon.canDealDamage = true;
        weapon.damage = characterStats.damage * 1.5f;

        yield return new WaitForSeconds(beginingAnimationTime);
        playerMovementComponent.ResumeMovement();
        yield return new WaitForSeconds(endAnimationTime);
        playerAnimationComponent.DeactivateSecondAttack();
        weapon.canDealDamage = false;

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
