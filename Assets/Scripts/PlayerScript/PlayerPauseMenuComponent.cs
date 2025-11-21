using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPauseMenuComponent : MonoBehaviour
{
    public bool isPaused = false;

    // Components
    PlayerInput playerInput;

    // Input Actions
    private InputAction pauseAction;

    private void Start()
    {
        if (playerInput == null)
        {
            Debug.LogWarning($"PlayerInput pas encore assigné pour {gameObject.name}. En attente...");
        }
        else
        {
            InitializePlayerInput();
        }
    }

    public void TogglePause(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            GameManagerScript.Instance.TogglePause();
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

        pauseAction = playerInput.actions["Pause"];

        pauseAction.performed += TogglePause;
        pauseAction.canceled += TogglePause;

    }

    void OnEnable() { }

    void OnDisable()
    {
        if (playerInput == null) return;

        playerInput.actions.FindAction("Player/Pause").performed -= TogglePause;
        playerInput.actions.FindAction("Player/Pause").canceled -= TogglePause;
    }

}