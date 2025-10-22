using UnityEngine;
using UnityEngine.InputSystem;

namespace SCRIPTS_MARC
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput playerInput;
        private PlayerSelectionPanel myPanel;
        private CharacterSelectionManager manager;
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            playerInput = GetComponent<PlayerInput>();
            manager = FindObjectOfType<CharacterSelectionManager>(); 
            
            if (manager != null)
            {
                myPanel = manager.RegisterPlayerAndGetPanel(playerInput);

                if (myPanel != null)
                {
                    // 1. On active la bonne carte d'action
                    playerInput.SwitchCurrentActionMap("UI");
                    
                    // 2. On s'abonne aux actions en utilisant leur chemin complet (Map/Action)
                    // C'est la syntaxe correcte et la plus sûre
                    playerInput.actions.FindAction("UI/Next").performed += OnNext;
                    playerInput.actions.FindAction("UI/Previous").performed += OnPrevious;
                    playerInput.actions.FindAction("UI/Select").performed += OnSelect;
                }
                else
                {
                    Debug.LogWarning($"Joueur {playerInput.playerIndex} s'est connecté mais aucun panneau n'est disponible.");
                    Destroy(gameObject);
                }
            }
        }

        void OnDisable()
        {
            if (playerInput != null && myPanel != null)
            {
                // On se désabonne de la même manière
                playerInput.actions.FindAction("UI/Next").performed -= OnNext;
                playerInput.actions.FindAction("UI/Previous").performed -= OnPrevious;
                playerInput.actions.FindAction("UI/Select").performed -= OnSelect;
            }
        }

        // --- Le reste de vos fonctions (OnNext, OnPrevious, OnSelect) est correct ---

        private void OnNext(InputAction.CallbackContext context)
        {
            myPanel?.NextCharacter();
        }

        private void OnPrevious(InputAction.CallbackContext context)
        {
            myPanel?.PreviousCharacter();
        }

        private void OnSelect(InputAction.CallbackContext context)
        {
            myPanel?.ConfirmSelection();
        }
    }
}