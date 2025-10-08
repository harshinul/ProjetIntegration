using UnityEngine;
using UnityEngine.UI;

public class UltimateAbilityComponent : MonoBehaviour
{
    [SerializeField] Image ultBar;
    PlayerAttackScript playerWeapon;
    GameObject player;
    void Start()
    {
        ultBar.fillAmount = 0f;
        playerWeapon = GetComponent<PlayerAttackScript>();

        if(playerWeapon != null)
            player = playerWeapon.gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        ChargeUltOverTime();
    }

    public void SetUltBarUI(Image ultImage)
    {
        ultBar = ultImage;
        ultBar.fillAmount = 0f;
    }

    public void ChargeUltDamage(float amount,GameObject damageDealer)
    {
        if(damageDealer == player)
        {
            Debug.Log("Ult charged by " + amount);
            ultBar.fillAmount = Mathf.Clamp01(ultBar.fillAmount + (amount / 100));
        }

    }
    public void ChargeUltOverTime()
    {
        ultBar.fillAmount = Mathf.Clamp01(ultBar.fillAmount + Time.deltaTime * 0.01f);
    }
}
