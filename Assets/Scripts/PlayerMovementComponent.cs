using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovementComponent : MonoBehaviour
{
    //playerStats
    private CharacterStats characterStats;
    private ClassType classType;
    private PlayerHealthComponent player;
    //Animation
    PlayerAnimationComponent playerAnimationComponent;

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
    private float dashPower;
    private float dashTime = 0.2f;
    private float dashCooldown = 1f;

    // Movement
    Vector3 direction = Vector3.zero;
    Vector3 jump = Vector3.zero;
    Vector2 move = Vector2.zero;
    Vector2 lastMove = Vector2.zero;
    bool canMove = true;

    // Input
    bool wantsToJump = false;

    // Components
    PlayerInput playerInput;
    CharacterController characterController;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction fastFallAction;

    void Awake()
    {
        playerAnimationComponent = GetComponent<PlayerAnimationComponent>();
        characterController = GetComponent<CharacterController>();
        player = GetComponent<PlayerHealthComponent>();
    }

    void Start()
    {
        if (playerInput == null)
        {
            Debug.LogWarning($"PlayerInput pas encore assigné pour {gameObject.name}. En attente...");
        }
        else
        {
            InitializePlayerInput();
        }

        // Initialisez vos stats ici
        if (characterStats == null)
        {
            characterStats = GetComponent<PlayerAttackScript>().GetCharacterStats();
            if (characterStats != null)
            {
                speed = characterStats.speed;
                dashPower = characterStats.dashPower;
            }
        }
    }

    
    public void SetPlayerInput(PlayerInput input)
    {
        playerInput = input;
        InitializePlayerInput();
    }

    private void InitializePlayerInput()
    {
        if (playerInput == null)
        {
            return;
        }
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        dashAction = playerInput.actions["Dash"];
        fastFallAction = playerInput.actions["FastFall"];

        moveAction.performed += Move;
        moveAction.canceled += Move;
        jumpAction.performed += Jump;
        jumpAction.canceled += Jump;
        dashAction.performed += Dash;
        fastFallAction.performed += FastFall;
        fastFallAction.canceled += FastFall;
        // S'abonner aux actions
        // playerInput.actions.FindAction("Player/Move").performed += Move;
        // playerInput.actions.FindAction("Player/Move").canceled += Move;
        // playerInput.actions.FindAction("Player/Jump").performed += Jump;
        // playerInput.actions.FindAction("Player/Jump").canceled += Jump;
        // playerInput.actions.FindAction("Player/Dash").performed += Dash;
        // playerInput.actions.FindAction("Player/FastFall").performed += FastFall;
        // playerInput.actions.FindAction("Player/FastFall").canceled += FastFall;
    }

    void OnEnable() { }
    
    void OnDisable()
    {
        if (playerInput == null) return;

        playerInput.actions.FindAction("Player/Move").performed -= Move;
        playerInput.actions.FindAction("Player/Move").canceled -= Move;
        
        playerInput.actions.FindAction("Player/Jump").performed -= Jump;
        playerInput.actions.FindAction("Player/Jump").canceled -= Jump;
        
        playerInput.actions.FindAction("Player/Dash").performed -= Dash;
        playerInput.actions.FindAction("Player/FastFall").performed -= FastFall;
        playerInput.actions.FindAction("Player/FastFall").canceled -= FastFall;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Movement();
        }
        else if(player.PlayerIsDead())
        {
            if (characterController.isGrounded)
            {
                characterController.enabled = false;
            }
            else
            {
                GravityMovement();
            }
        }
    }

    //Movement
    public void Movement()
    {
        direction = new Vector3(move.x, 0, 0).normalized;
        ChangeRotation();
        Jumping();
        Falling();
        if (direction.magnitude > 0)
        {
            playerAnimationComponent.ActivateRunning();
        }
        else
        {
            playerAnimationComponent.DeactivateRunning();
        }

        characterController.Move((speed * direction + jump) * Time.deltaTime);
    }

    public void Jumping()
    {
        playerAnimationComponent.DeactivateJumping();
        if (wantsToJump && jumpCount < maxJumpCount)
        {
            jump = transform.up * jumpForce;
            playerAnimationComponent.ActivateJumping();
            playerAnimationComponent.DeactivateRunning();
            jumpCount++;
            wantsToJump = false;
        }
    }

    void Falling()
    {
        multiplier = fastFall ? fallMultiplier : 1f;
        if (characterController.isGrounded && jump.y <= 0)
        {
            jumpCount = 0;
        }
        else
        {
            jump += Time.deltaTime * (gravityValue * multiplier) * transform.up;
        }
        if (!characterController.isGrounded)
        {
            playerAnimationComponent.ActivateFalling();
            playerAnimationComponent.DeactivateRunning();
        }
        else
        {
            playerAnimationComponent.DeactivateFalling();
        }
    }

    void ChangeRotation()
    {
        if (move.x > 0)
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (move.x < 0)
            transform.rotation = Quaternion.Euler(0, -90, 0);
    }
    
    public void Move(InputAction.CallbackContext ctx)
    {
        if (!player.PlayerIsDead())
        {
            move = ctx.ReadValue<Vector2>().normalized;
        }
    }

    //Jump
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !player.PlayerIsDead())
        {
            wantsToJump = true;
        }
        else
        {
            wantsToJump = false;
        }
    }

    //Dash
    public void Dash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canDash && !isDashing && canMove && !player.PlayerIsDead())
            StartCoroutine(DashCoroutine());
    }

    public IEnumerator DashCoroutine()
    {
        if (!player.PlayerIsDead())
        {
            canDash = false;
            isDashing = true;

            float gravity = gravityValue;
            gravityValue = 0;

            float elapsed = 0f;
            Vector3 dashDir = new Vector3(move.x, 0, 0).normalized;

            while (elapsed < dashTime)
            {
                float t = elapsed / dashTime;
                float speedDash = Mathf.Lerp(characterStats.dashPower, 0f, t);

                characterController.Move(dashDir * speedDash * Time.deltaTime);

                elapsed += Time.deltaTime;
                yield return null;
            }

            isDashing = false;
            gravityValue = gravity;
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }

    //FastFall
    public void FastFall(InputAction.CallbackContext ctx)
    {
        if(!player.PlayerIsDead())
            fastFall = ctx.performed;
    }

    public void GravityMovement()
    {
        characterController.Move((speed * direction + Vector3.down) * Time.deltaTime);
    }

    public void StopMovement()
    {
        canMove = false;
        playerAnimationComponent.DeactivateRunning();
        playerAnimationComponent.DeactivateJumping();
        playerAnimationComponent.DeactivateFalling();
    }

    public void ResumeMovement()
    {
        if(move == Vector2.zero)
            move = lastMove;
        canMove = true;
    }
}