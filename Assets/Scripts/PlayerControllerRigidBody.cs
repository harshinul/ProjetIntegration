using UnityEngine;

public class PlayerControllerRigidBody : MonoBehaviour
{
    Rigidbody rb;
    float forceMagnitude = 9.81f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0,forceMagnitude,0),ForceMode.VelocityChange);
    }
}

