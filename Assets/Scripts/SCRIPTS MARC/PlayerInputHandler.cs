using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // AJOUTÉ

namespace SCRIPTS_MARC
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput playerInput;
        
        private CharacterSelectionManager charSelectManager;
        private MapManager mapManager;
        
        private PlayerSelectionPanel myPanel; 

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            playerInput = GetComponent<PlayerInput>();

            SceneManager.sceneLoaded += OnSceneLoaded;

            LinkToCurrentSceneManager();
        }

        void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            UnsubscribeAllActions();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LinkToCurrentSceneManager();
        }

        private void LinkToCurrentSceneManager()
        {
            UnsubscribeAllActions();

            charSelectManager = FindObjectOfType<CharacterSelectionManager>();
            mapManager = FindObjectOfType<MapManager>();

            if (charSelectManager != null)
            {
                myPanel = charSelectManager.RegisterPlayerAndGetPanel(playerInput);

                if (myPanel != null)
                {
                    playerInput.SwitchCurrentActionMap("UI");
                    playerInput.actions.FindAction("UI/Next").performed += OnNextCharacter;
                    playerInput.actions.FindAction("UI/Previous").performed += OnPreviousCharacter;
                    playerInput.actions.FindAction("UI/Select").performed += OnJoinSelection;
                    playerInput.actions.FindAction("UI/Start").performed += OnConfirmSelection;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else if (mapManager != null)
            {
                
                playerInput.SwitchCurrentActionMap("UI");
                playerInput.actions.FindAction("UI/Navigate").performed += OnMapNavigate; 
                playerInput.actions.FindAction("UI/Submit").performed += OnMapSubmit;     
            }
        }

       
        private void UnsubscribeAllActions()
        {
            if (playerInput == null) return;
            var uiMap = playerInput.actions.FindActionMap("UI");
            if (uiMap == null) return;

            var nextAction = uiMap.FindAction("Next");
            if (nextAction != null) nextAction.performed -= OnNextCharacter;

            var previousAction = uiMap.FindAction("Previous");
            if (previousAction != null) previousAction.performed -= OnPreviousCharacter;

            var selectAction = uiMap.FindAction("Select");
            if (selectAction != null) selectAction.performed -= OnJoinSelection;

            var startAction = uiMap.FindAction("Start");
            if (startAction != null) startAction.performed -= OnConfirmSelection;
            
            var navigateAction = uiMap.FindAction("Navigate");
            if (navigateAction != null) navigateAction.performed -= OnMapNavigate;

            var submitAction = uiMap.FindAction("Submit");
            if (submitAction != null) submitAction.performed -= OnMapSubmit;
        }

        private void OnNextCharacter(InputAction.CallbackContext context)     { myPanel?.NextCharacter(); }
        private void OnPreviousCharacter(InputAction.CallbackContext context) { myPanel?.PreviousCharacter(); }
        private void OnJoinSelection(InputAction.CallbackContext context)     { myPanel?.JoinSelection(); }
        private void OnConfirmSelection(InputAction.CallbackContext context)  { myPanel?.ConfirmSelection(); }


        private void OnMapNavigate(InputAction.CallbackContext context)
        {
            if (mapManager == null || playerInput.playerIndex != mapManager.CurrentPlayerIndex) return;

            Vector2 move = context.ReadValue<Vector2>();
            
            if (move.x > 0.8f) 
                mapManager.PlayerNavigate(playerInput.playerIndex, 1); 
            else if (move.x < -0.8f) 
                mapManager.PlayerNavigate(playerInput.playerIndex, -1); 
        }

        private void OnMapSubmit(InputAction.CallbackContext context)
        {
            if (mapManager == null || playerInput.playerIndex != mapManager.CurrentPlayerIndex) return;

            mapManager.PlayerSubmit(playerInput.playerIndex);
        }
    }
}