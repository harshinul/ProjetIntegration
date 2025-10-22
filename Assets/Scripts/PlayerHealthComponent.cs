using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthComponent : MonoBehaviour
{

    //Components
    PlayerAnimationComponent playerAnimationComponent;
    PlayerMovementComponent playerMovementComponent;
    UltimateAbilityComponent ultimateAbilityComponent;
    PlayerAttackScript playerWeapon;
    GameObject player;

    [SerializeField] float maxHealth = 100f;

    //UI Elements
    [SerializeField] Image frontHealthBar;
    [SerializeField] Image backHealthBar;
    [SerializeField] float chipSpeed = 4f;
    [SerializeField] private float chipDelay = 0.5f;
    private Coroutine HealthBarCoroutine;

    // Health
    private bool isDead;
    private float health;
    private bool isInvincible = false;

    //Character color
    SkinnedMeshRenderer[] meshRenderers;
    Material[] materials;
    Color[] originalColors;
    [SerializeField] Color damageColor = Color.red;
    [SerializeField] float flashDuration = 0.1f;
    [SerializeField] int flashCount = 3;

    void Start()
    {
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        List<Material> mats = new List<Material>();
        foreach (var rend in meshRenderers)
        {
            mats.AddRange(rend.materials);
        }
        materials = mats.ToArray();
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }

        playerAnimationComponent = GetComponent<PlayerAnimationComponent>();
        playerMovementComponent = GetComponent<PlayerMovementComponent>();
        ultimateAbilityComponent = GetComponent<UltimateAbilityComponent>();

        playerWeapon = GetComponent<PlayerAttackScript>();

        if (playerWeapon != null)
            player = playerWeapon.gameObject;

        health = maxHealth;

        frontHealthBar.fillAmount = 1f;
        backHealthBar.fillAmount = 1f;
    }

    public void SetHealthBarUI(Image backHealth, Image frontHealth)
    {
        backHealthBar = backHealth;
        frontHealthBar = frontHealth;
        frontHealthBar.fillAmount = 1f;
        backHealthBar.fillAmount = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            
            if (!isInvincible)
            {
                TakeDamage(100f);
                Debug.Log("Player takes 3 damage");
            }
                

        }
    }
    public void TakeDamage(float damage)
{
        if (isDead) return;
        health -= damage;
        health = Mathf.Clamp(health, 0f, maxHealth);
        isInvincible = true;
        if (health <= 0)
        {
            Debug.Log("Player is dead");
            isDead = true;
            playerMovementComponent.StopMovement();
            playerAnimationComponent.ActivateDeath();

        }
        //playerAnimationComponent.ActivateTakingDamage();
        StartCoroutine(DamageVisual());
        UpdateHealthUI();
    }

    public bool PlayerIsDead()
    {
        return isDead;
    }

    IEnumerator DamageVisual()
    {

        for (int i = 0; i < flashCount; i++)
        {
            foreach (var curentMaterial in materials)
            {
                curentMaterial.color = damageColor;
            }
            yield return new WaitForSeconds(flashDuration);
            for(int j = 0; j < materials.Length; j++)
            {
                materials[j].color = originalColors[j];
            }
            yield return new WaitForSeconds(flashDuration);
        }
        isInvincible = false;
        for (int j = 0; j < materials.Length; j++)
        {
            materials[j].color = originalColors[j];
        }
    }

    private void UpdateHealthUI()
    {
        float fillPercentage = health / maxHealth;

        frontHealthBar.fillAmount = fillPercentage;

        if (HealthBarCoroutine != null)
            StopCoroutine(HealthBarCoroutine);

        HealthBarCoroutine = StartCoroutine(AnimateHealthBar(backHealthBar.fillAmount, fillPercentage));
    }

    private IEnumerator AnimateHealthBar(float fromFill, float toFill)
    {
        float elapsed = 0f;
        float elapsedDelay = 0f;
        // Wait for the delay before starting the animation
        while (elapsedDelay < chipDelay)
        {
            elapsedDelay += Time.deltaTime;
            yield return null;
        }

        while (elapsed < chipSpeed)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fromFill, toFill, t);
            yield return null;
        }
        backHealthBar.fillAmount = toFill;
        backHealthBar.fillAmount = toFill;
    }
}
