using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public bool isInPlayer = false;

    private void OnTriggerEnter(Collider other)
    {

            Debug.Log("Trigger Entered");
    }
}
