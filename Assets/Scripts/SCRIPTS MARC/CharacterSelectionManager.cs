using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // <-- IMPORTANT : Ajoutez ceci

namespace SCRIPTS_MARC
{
    // S'assure que le PlayerInputManager est sur le même objet
    [RequireComponent(typeof(PlayerInputManager))] 
    public class CharacterSelectionManager : MonoBehaviour
    {
        [SerializeField] private PlayerSelectionPanel[] panels;
        [SerializeField] private GameObject[] socles;
        [SerializeField] private string nextSceneName = "AreneSelection";
    
        private int readyPlayersCount = 0;
        private PlayerInputManager playerInputManager;

        void Awake()
        {
            playerInputManager = GetComponent<PlayerInputManager>();
            
            // On vérifie juste si le prefab est assigné
            if (playerInputManager.playerPrefab == null)
            {
                Debug.LogError("ERREUR : Le 'Player Prefab' (PlayerInputHandlerPrefab) n'est pas assigné " +
                               "dans l'inspecteur du PlayerInputManager !");
            }
        }
        
        void Start()
        {
            // On désactive tous les panneaux au début.
            foreach (var panel in panels)
            {
                if (panel != null)
                {
                    panel.gameObject.SetActive(false);
                }
            }
            
            foreach (var socle in socles)
            {
                if (socle != null)
                {
                    socle.SetActive(false);
                }
            }
        }

        // Nouvelle méthode appelée par PlayerInputHandler.cs
        public PlayerSelectionPanel RegisterPlayerAndGetPanel(PlayerInput playerInput)
        {
            int playerIndex = playerInput.playerIndex;

            Debug.Log($"Enregistrement du joueur {playerIndex}");

            // On vérifie si on a un panneau et un socle pour ce joueur
            if (playerIndex < panels.Length && panels[playerIndex] != null && 
                playerIndex < socles.Length && socles[playerIndex] != null)
            {
                PlayerSelectionPanel panel = panels[playerIndex];
                GameObject socle = socles[playerIndex];

                // On active le panneau et on l'initialise
                panel.gameObject.SetActive(true); 
                socle.SetActive(true);
                panel.Initialize(playerIndex, this, socle);
                
                return panel; // On retourne le panneau au PlayerInputHandler
            }

            Debug.LogWarning($"Aucun panneau ou socle trouvé pour l'index {playerIndex}");
            return null; // Pas de panneau disponible
        }

        public void PlayerIsReady(int playerIndex, string characterName)
        {
            readyPlayersCount++;
        
            PlayerPrefs.SetString("classTypePlayer" + (playerIndex + 1), characterName);
            Debug.Log("Player " + (playerIndex + 1) + " is ready with " + characterName);

            // On vérifie si tous les joueurs qui se SONT CONNECTÉS sont prêts
            int totalJoinedPlayers = playerInputManager.playerCount;
            
            if (readyPlayersCount > 0 && readyPlayersCount == totalJoinedPlayers)
            {
                // Tous les joueurs connectés sont prêts, on lance le jeu !
                StartGame();
            }
        }
        
        public void StartGame()
        {
            if (readyPlayersCount > 0)
            {
                Debug.Log(readyPlayersCount + " joueurs sont prêts !");
                PlayerPrefs.SetInt("numberOfPlayer", readyPlayersCount);
                PlayerPrefs.Save();
                
                // On empêche de nouveaux joueurs de se joindre pendant le chargement
                playerInputManager.DisableJoining();
                
                // On charge la scène de sélection d'arène
                // Les objets "PlayerInputHandler" vont persister grâce à DontDestroyOnLoad
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("Aucun joueur n'est prêt !");
            }
        }
    }
}