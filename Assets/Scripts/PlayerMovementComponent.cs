using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementComponent : MonoBehaviour
{
    //Movement
    [SerializeField] private float speed = 5f;
    // Gravity
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float fallMultiplier = 2.5f;
    private bool fastFall = false;
    float multiplier;

    // Jumping
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private int maxJumpCount = 2;
    int jumpCount = 0;

    //Dashing
    private bool canDash = true;
    private bool isDashing = false;
    [SerializeField] float dashPower = 20f;
    private float dashTime = 0.2f;
    private float dashCooldown = 1f;

    // Movement
    Vector3 direction = Vector3.zero;
    Vector3 jump = Vector3.zero;
    Vector2 move = Vector2.zero;

    // Components
    PlayerInput playerInput;
    CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Jump"].started += Jump; 
        playerInput.actions["Jump"].canceled += Jump;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    //Movement
    public void Movement()
    {

        direction = new Vector3(move.x, 0, 0).normalized;

        multiplier = fastFall ? fallMultiplier : 1f;
        if (characterController.isGrounded && jump.y <= 0)
        {
            jumpCount = 0;
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
    //Jump
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && jumpCount < maxJumpCount)
        {
            jump = transform.up * jumpForce;
            jumpCount++;
        }
    }
    //Dash
    public void Dash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canDash && !isDashing)
            StartCoroutine(DashCoroutine());
    }
    public IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;

        float gravity = gravityValue;
        gravityValue = 0;

        float elapsed = 0f;
        Vector3 dashDir = new Vector3(move.x, 0, 0).normalized;

        while (elapsed < dashTime)
        {
            // Smoothly decrease dash speed over time
            //act as a progress bar
            float t = elapsed / dashTime;
            //act as a decrease of speed over time based on t
            float speedDash = Mathf.Lerp(dashPower, 0f, t);

            characterController.Move(dashDir * speedDash * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        gravityValue = gravity;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    //FastFall
    public void FastFall(InputAction.CallbackContext ctx)
    {
        fastFall = ctx.performed;
    }
}
