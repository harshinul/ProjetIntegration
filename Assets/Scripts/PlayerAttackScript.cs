using UnityEngine;

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
    [SerializeField] ClassType classType;
    private PlayerHealthComponent player;
    private CharacterStats characterStats;
    void Start()
    {

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

    void Update()
    {

    }

    public void Attack()
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
