using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthComponent : MonoBehaviour
{

    [SerializeField] float maxHealth = 100f;
    [SerializeField] float chipSpeed = 2f;

    [SerializeField] Image frontHealthBar;
    [SerializeField] Image backHealthBar;

    public bool isDead;
    private float health;
    private float elapsedSinceHit;
    private Coroutine backBarCoroutine;

    void Start()
    {
        health = maxHealth;

        frontHealthBar.fillAmount = 1f;
        backHealthBar.fillAmount = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(10f);
        }
        UpdateHealthUI();
    }
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        health -= damage;
        health = Mathf.Clamp(health, 0f, maxHealth);
        elapsedSinceHit = 0f;

        if (health <= 0)
        {
            isDead = true;

        }
        Debug.Log($"Player Health: {health}");
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        float targetFill = health / maxHealth;

        frontHealthBar.fillAmount = targetFill;

        if (backBarCoroutine != null)
            StopCoroutine(backBarCoroutine);
        backBarCoroutine = StartCoroutine(AnimateBackBar(backHealthBar.fillAmount, targetFill));
    }

    private IEnumerator AnimateBackBar(float fromFill, float toFill)
    {
        float elapsed = 0f;
        while (elapsed < chipSpeed)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / chipSpeed);
            backHealthBar.fillAmount = Mathf.Lerp(fromFill, toFill, t);
            yield return null;
        }
        backHealthBar.fillAmount = toFill;
    }
}
