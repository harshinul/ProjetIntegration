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

    void Start()
    {
        playerAnimationComponent = GetComponent<PlayerAnimationComponent>();
        health = maxHealth;

        frontHealthBar.fillAmount = 1f;
        backHealthBar.fillAmount = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
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

        }
        Debug.Log($"Player Health: {health}");
        UpdateHealthUI();
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
