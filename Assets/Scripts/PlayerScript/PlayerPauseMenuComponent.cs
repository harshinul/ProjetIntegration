using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPauseMenuComponent : MonoBehaviour
{
    public bool isPaused = false;

    public void TogglePause(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            isPaused = !isPaused;
        }

    }

}