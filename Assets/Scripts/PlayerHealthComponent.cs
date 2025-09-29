using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthComponent : MonoBehaviour
{

    //Components
    PlayerAnimationComponent playerAnimationComponent;

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

    //Character color
    [SerializeField] Material defaultMaterial;
    Material curentMaterial;

    void Start()
    {
        curentMaterial = defaultMaterial;
        playerAnimationComponent = GetComponent<PlayerAnimationComponent>();
        health = maxHealth;

        frontHealthBar.fillAmount = 1f;
        backHealthBar.fillAmount = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Player takes 3 damage");
            TakeDamage(3f);
        }
    }
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        health -= damage;
        health = Mathf.Clamp(health, 0f, maxHealth);

        if (health <= 0)
        {
            isDead = true;
            playerAnimationComponent.ActivateDeath();

        }
        //playerAnimationComponent.ActivateTakingDamage();
        StartCoroutine(DamageVisual());
        Debug.Log($"Player Health: {health}");
        UpdateHealthUI();
    }

    IEnumerator DamageVisual()
    {
        // Implement visual feedback for taking damage (e.g., flashing red)


        Color originalColor = defaultMaterial.color;
        Color flashColor = Color.red;

        float flashDuration = 0.1f;
        int flashCount = 3;

        for (int i = 0; i < flashCount; i++)
        {
            curentMaterial.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            curentMaterial.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }

        curentMaterial.color = defaultMaterial.color;
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
