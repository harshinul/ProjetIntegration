using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float fallMultiplier = 2.5f;

    private bool fastFall = false;
    float multiplier;

    Vector3 direction = Vector3.zero;
    Vector3 jump = Vector3.zero;
    Vector2 move = Vector2.zero;

    CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Movement()
    {

        direction = new Vector3(move.x, 0, 0).normalized;

        multiplier = fastFall ? fallMultiplier : 1f;
        if (characterController.isGrounded)
        {
            jump.y = Mathf.Max(-1, jump.y);
        }
        else
        {
            jump += Time.deltaTime * (gravityValue * multiplier) * transform.up;
        }

        characterController.Move((speed * direction + jump) * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>().normalized;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && characterController.isGrounded)
        {
            jump = transform.up * jumpForce;
        }
    }

    public void FastFall(InputAction.CallbackContext ctx)
    {
        fastFall = ctx.performed;
    }

}
