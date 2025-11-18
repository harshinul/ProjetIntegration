using MaykerStudio.Demo;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

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
                    health = 135f,
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
                    speed = 5f
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
    private GameObject closestPlayer;

    //Attack
    [SerializeField] GameObject ultimateProjectile;
    WeaponScript weapon;
    float attack1Duration;
    float attack2Duration;
    float ultDuration;
    public bool isAttacking = false;
    public bool IsAttacking => isAttacking;

    //input
    bool wantsToAttack1 = false;
    bool wantsToAttack2 = false;
    bool wantsToUltimate = false;

    //SFX
    [SerializeField] AudioClip[] attack1Audio;
    [SerializeField] AudioClip[] attack2Audio;
    [SerializeField] AudioClip[] ultimateAudio;

    private PlayerInput playerInput;

    [SerializeField] Transform firePoint;

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

    }

    void Start()
    {
        if (playerInput == null)
        {
            Debug.LogWarning($"PlayerInput pas encore assignÃ© pour {gameObject.name}. En attente...");
        }
        else
        {
            InitializePlayerInput();
        }

        attack1Duration = playerAnimationComponent.GetAttack1Duration();
        attack2Duration = playerAnimationComponent.GetAttack2Duration();
        ultDuration = playerAnimationComponent.GetUltDuration();
    }


    public void SetPlayerInput(PlayerInput input)
    {
        playerInput = input;
        InitializePlayerInput();
    }


    private void InitializePlayerInput()
    {
        if (playerInput == null)
        {
            return;
        }

        playerInput.actions.FindAction("Player/Attack").performed += Attack1;
        playerInput.actions.FindAction("Player/Attack2").performed += Attack2;
        playerInput.actions.FindAction("Player/Ultimate").performed += UseUltimate;
    }

    void OnEnable() { }

    void OnDisable()
    {
        if (playerInput == null) return;
        playerInput.actions.FindAction("Player/Attack").performed -= Attack1;
        playerInput.actions.FindAction("Player/Attack2").performed -= Attack2;
        playerInput.actions.FindAction("Player/Ultimate").performed -= UseUltimate;
    }

    public CharacterStats GetCharacterStats()
    {
        if (characterStats != null)
        {
            characterStats = CharacterStats.GetStatsForClass(classType);
        }
        return characterStats;
    }

    //Attack calls to weapon - animation events
    void Attack1()
    {
        weapon.Attack1();
    }

    void Attack2()
    {
        weapon.Attack2();
    }

    void stopAttack()
    {
        weapon.StopAttack();
    }

    //Audio calls - animation events

    void PlayAttack1Sound()
    {
        PlaySound(attack1Audio);
    }
    void PlayAttack2Sound()
    {
        PlaySound(attack2Audio);
    }

    void PlayUltimateSound()
    {
        PlaySound(ultimateAudio);
    }


    public IEnumerator CouroutineStartAttack1()
    {
        float beginingAnimationTime = (attack1Duration / characterStats.attackSpeed) / 2f;
        float endAnimationTime = attack1Duration - beginingAnimationTime;
        isAttacking = true;
        // playerMovementComponent.StopMovement();
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
        float endAnimationTime = attack2Duration - beginingAnimationTime;
        isAttacking = true;
        //playerMovementComponent.StopMovement();
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
        if (classType.Equals(ClassType.Warrior))
        {
            var obj = Instantiate(ultimateProjectile, firePoint.position, Quaternion.Euler(0, transform.rotation.y > 0 ? 90 : -90, 90));
            obj.GetComponent<Projectile>().damage = characterStats.ultDamage;
            obj.GetComponent<Projectile>().player = this.gameObject;
            obj.GetComponent<Projectile>().Fire();
        }
        else if (classType.Equals(ClassType.Assassin))
        {
            var obj = Instantiate(ultimateProjectile, firePoint.position, Quaternion.Euler(0, 0, 0)/*Quaternion.Euler(0, transform.rotation.y > 0 ? 90 : -90, 90)*/);
            obj.GetComponentInChildren<DamageOverTime>().player = this.gameObject;
            obj.GetComponent<ParticleSystem>().Play();
        }
        else if (classType.Equals(ClassType.Mage))
        {
            closestPlayer = ClosestPlayer();
            if (closestPlayer != null)
            {
                var obj = Instantiate(ultimateProjectile, new Vector3(closestPlayer.transform.position.x + 0.8f, 0, closestPlayer.transform.position.z), Quaternion.Euler(90, 0, 0));
                obj.GetComponent<Projectile>().damage = characterStats.ultDamage;
                obj.GetComponent<Projectile>().player = this.gameObject;
                obj.GetComponent<Projectile>().speed = 0;
                obj.GetComponent<Projectile>().Fire();
            }
        }
        playerMovementComponent.ResumeMovement();
        yield return new WaitForSeconds(endAnimationTime);
        playerAnimationComponent.DeactivateUltimate();
        ultimateAbilityComponent.ActivateUltimate();
        ResetAttack();

    }

    void PlaySound(AudioClip[] audioClips)
    {
        if (audioClips.Length == 0) return;
        int index = UnityEngine.Random.Range(0, audioClips.Length);
        SFXManager.Instance.PlaySFX(audioClips[index], transform, 1);
    }


    public GameObject ClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject player in players)
        {
            if (player != this.gameObject)
            {
                float distance = Vector3.Distance(player.transform.position, currentPosition);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = player;
                }
            }
        }
        return closestPlayer;
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
        else if (wantsToUltimate && !isAttacking && ultimateAbilityComponent.IsUltReady() && characterController.isGrounded)
        {
            StartCoroutine(CouroutineStartUltimate());
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