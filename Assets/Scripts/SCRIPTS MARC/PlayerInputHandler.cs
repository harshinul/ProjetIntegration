using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // AJOUTÉ

namespace SCRIPTS_MARC
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput playerInput;
        
        // Références aux managers possibles
        private CharacterSelectionManager charSelectManager;
        private MapManager mapManager;
        
        private PlayerSelectionPanel myPanel; // Gardé pour la sélection de perso

        void Awake()
        {
            Debug.Log("--- PlayerInputHandler: AWAKE() ---");
            DontDestroyOnLoad(this.gameObject);
            playerInput = GetComponent<PlayerInput>();

            // On s'abonne à l'événement de chargement de scène
            SceneManager.sceneLoaded += OnSceneLoaded;

            // On fait une première liaison au cas où on est déjà dans la bonne scène
            LinkToCurrentSceneManager();
        }

        void OnDestroy()
        {
            // IMPORTANT : Toujours se désabonner
            SceneManager.sceneLoaded -= OnSceneLoaded;
            UnsubscribeAllActions();
        }

        /// <summary>
        /// Appelé à chaque chargement de scène pour trouver le bon manager
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LinkToCurrentSceneManager();
        }

        /// <summary>
        /// Trouve le manager actif (CharSelect ou Map) et s'y connecte
        /// </summary>
        private void LinkToCurrentSceneManager()
        {
            // D'abord, on nettoie les anciens abonnements
            UnsubscribeAllActions();

            charSelectManager = FindObjectOfType<CharacterSelectionManager>();
            mapManager = FindObjectOfType<MapManager>();

            if (charSelectManager != null)
            {
                // --- On est dans la scène de Sélection de Personnage ---
                myPanel = charSelectManager.RegisterPlayerAndGetPanel(playerInput);

                if (myPanel != null)
                {
                    playerInput.SwitchCurrentActionMap("UI");
                    // On s'abonne aux actions de sélection de personnage
                    playerInput.actions.FindAction("UI/Next").performed += OnNextCharacter;
                    playerInput.actions.FindAction("UI/Previous").performed += OnPreviousCharacter;
                    playerInput.actions.FindAction("UI/Select").performed += OnSelectCharacter;
                }
                else
                {
                    Debug.LogWarning($"Joueur {playerInput.playerIndex} n'a pas pu trouver de panel.");
                    Destroy(gameObject);
                }
            }
            else if (mapManager != null)
            {
                // --- On est dans la scène de Sélection de Map ---
                playerInput.SwitchCurrentActionMap("UI");
                // On s'abonne aux NOUVELLES actions de sélection de map
                playerInput.actions.FindAction("UI/Navigate").performed += OnMapNavigate; // Doit exister dans votre Action Map
                playerInput.actions.FindAction("UI/Submit").performed += OnMapSubmit;     // Doit exister (ex: "Select")
            }
            // Si aucun manager n'est trouvé, on ne fait rien (on attend la scène de jeu)
        }

        /// <summary>
        /// Se désabonne de TOUTES les actions pour éviter les conflits
        /// </summary>
        private void UnsubscribeAllActions()
        {
            if (playerInput == null) return;
            var uiMap = playerInput.actions.FindActionMap("UI");
            if (uiMap == null) return;

            // Désabonnement CharSelect
            uiMap.FindAction("Next").performed -= OnNextCharacter;
            uiMap.FindAction("Previous").performed -= OnPreviousCharacter;
            uiMap.FindAction("Select").performed -= OnSelectCharacter;
            
            // Désabonnement MapSelect
            uiMap.FindAction("Navigate").performed -= OnMapNavigate;
            uiMap.FindAction("Submit").performed -= OnMapSubmit;
        }

        // --- Actions pour la Sélection de Personnage ---
        private void OnNextCharacter(InputAction.CallbackContext context)     { myPanel?.NextCharacter(); }
        private void OnPreviousCharacter(InputAction.CallbackContext context) { myPanel?.PreviousCharacter(); }
        private void OnSelectCharacter(InputAction.CallbackContext context)   { myPanel?.ConfirmSelection(); }


        // --- NOUVELLES Actions pour la Sélection de Map ---

        private void OnMapNavigate(InputAction.CallbackContext context)
        {
            if (mapManager == null || playerInput.playerIndex != mapManager.CurrentPlayerIndex) return;

            Vector2 move = context.ReadValue<Vector2>();
            
            // On utilise un seuil pour éviter la sensibilité du stick
            if (move.x > 0.8f) 
                mapManager.PlayerNavigate(playerInput.playerIndex, 1); // +1 = Droite
            else if (move.x < -0.8f) 
                mapManager.PlayerNavigate(playerInput.playerIndex, -1); // -1 = Gauche
        }

        private void OnMapSubmit(InputAction.CallbackContext context)
        {
            if (mapManager == null || playerInput.playerIndex != mapManager.CurrentPlayerIndex) return;

            mapManager.PlayerSubmit(playerInput.playerIndex);
        }
    }
}