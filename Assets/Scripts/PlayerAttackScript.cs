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

    [SerializeField] ClassType classType;
    private PlayerHealthComponent player;
    private CharacterStats characterStats;

    //input
    bool wantsToAttack1 = false;
    bool wantsToAttack2 = false;
    void Start()
    {
        playerAnimationComponent = GetComponent<PlayerAnimationComponent>();
        player = GetComponent<PlayerHealthComponent>();
        characterStats = GetStatsForClass(classType);
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

    public void StartAttack1()
    {
        playerAnimationComponent.ActivateFirstAttack();
    }

    void Update()
    {
        if (wantsToAttack1)
        {
            StartAttack1();
        }
        else
        {
            playerAnimationComponent.DeactivateFirstAttack();
        }
    }

    public void Attack1(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            wantsToAttack1 = true;
        }
        else
        {
            wantsToAttack1 = false;
        }

    }

    public void Attack2()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            player.TakeDamage(characterStats.damage);
        }
    }
}
