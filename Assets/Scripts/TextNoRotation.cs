using UnityEngine;

public class TextNoRotation : MonoBehaviour
{

    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
