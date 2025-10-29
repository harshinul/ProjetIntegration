using UnityEngine;
using UnityEngine.UI;

public class UltimateAbilityComponent : MonoBehaviour
{
    [SerializeField] Image ultBar;
    public float percentage;
    void Start()
    {
        ultBar.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        percentage = ultBar.fillAmount;
        ChargeUltOverTime();
    }

    public void SetUltBarUI(Image ultImage)
    {
        ultBar = ultImage;
        ultBar.fillAmount = 0f;
    }

    public void ChargeUltDamage(float amount, GameObject damageDealer)
    {
        if (damageDealer != null)
        {
            Debug.Log("Ult charged by " + amount);
            ultBar.fillAmount = Mathf.Clamp01(ultBar.fillAmount + (amount * 1.5f / 100));
        }
    }
    public void ChargeUltOverTime()
    {
        ultBar.fillAmount = Mathf.Clamp01(ultBar.fillAmount + Time.deltaTime * 1f);
    }

    public bool IsUltReady()
    {
        return ultBar.fillAmount >= 1f;
    }

    public void ActivateUltimate()
    {
        if(IsUltReady())
        {
            Debug.Log("Ultimate Activated!");
            ultBar.fillAmount = 0f;
            // Implement ultimate ability effect here
        }
        else
        {
            Debug.Log("Ultimate not ready yet!");
        }
    }
}
