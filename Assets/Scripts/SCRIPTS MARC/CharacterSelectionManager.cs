using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 

namespace SCRIPTS_MARC
{
    // S'assure que le PlayerInputManager est sur le même objet
    [RequireComponent(typeof(PlayerInputManager))] 
    public class CharacterSelectionManager : MonoBehaviour
    {
        [SerializeField] private PlayerSelectionPanel[] panels;
        [SerializeField] private GameObject[] socles;
        [SerializeField] private string nextSceneName = "AreneSelectionFRL 1";
    
        private int playerWhoJoinedCount = 0;
        private int readyPlayersCount = 0;
        private PlayerInputManager playerInputManager;

        void Awake()
        {
            Time.timeScale = 1f;
            playerInputManager = GetComponent<PlayerInputManager>();

            // 1. IMPORTANT : On coupe l'arrivée de nouveaux joueurs immédiatement
            playerInputManager.DisableJoining();

            // 2. NETTOYAGE FORCÉ
            foreach (var player in PlayerInput.all.ToArray())
            {
                Destroy(player.gameObject);
            }

            // 3. Reset des compteurs
            playerWhoJoinedCount = 0;
            readyPlayersCount = 0;

            // Reset visuel des socles
            foreach (var socle in socles)
            {
                if (socle != null) socle.SetActive(false);
            }

            // Vérification de sécurité
            if (playerInputManager.playerPrefab == null)
            {
                Debug.LogError("ERREUR : Player Prefab non assigné dans le PlayerInputManager !");
            }

            // 4. On rouvre les vannes
            // Maintenant que c'est propre, on autorise les joueurs à appuyer sur un bouton pour rejoindre.
            playerInputManager.EnableJoining();
        }

        void Start()
        {
            // On s'assure que tous les panneaux sont ACTIFS au début
            foreach (var panel in panels)
            {
                if (panel != null)
                {
                    panel.gameObject.SetActive(true);
                }
            }
        }

        public PlayerSelectionPanel RegisterPlayerAndGetPanel(PlayerInput playerInput)
        {
            int playerIndex = playerInput.playerIndex;
            if (playerIndex < 0)
                return null;

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

                // On incrémente le nombre de joueurs qui se sont connectés
                playerWhoJoinedCount++;

                return panel; // On retourne le panneau au PlayerInputHandler
            }

            Debug.LogWarning($"Aucun panneau ou socle trouvé pour l'index {playerIndex}");
            return null; 
        }

        public void PlayerIsReady(int playerIndex, string characterName)
        {
            readyPlayersCount++;
        
            PlayerPrefs.SetString("classTypePlayer" + (playerIndex + 1), characterName);
            Debug.Log("Player " + (playerIndex + 1) + " is ready with " + characterName);

            // On vérifie si tous les joueurs qui se SONT CONNECTÉS sont prêts
            //int totalJoinedPlayers = playerInputManager.playerCount;
            
            if (readyPlayersCount > 1 && readyPlayersCount == playerWhoJoinedCount)
            {
                // Tous les joueurs connectés sont prêts, on lance le jeu !
                StartGame();
            }
        }
        
        public void StartGame()
        {
            //if (readyPlayersCount > 1)
            //{
                Debug.Log(readyPlayersCount + " joueurs sont prêts !");
                PlayerPrefs.SetInt("numberOfPlayer", readyPlayersCount);
                PlayerPrefs.Save();
                
                // On empêche de nouveaux joueurs de se joindre pendant le chargement
                playerInputManager.DisableJoining();
                
                // On charge la scène de sélection d'arène
                // Les objets "PlayerInputHandler" vont persister grâce à DontDestroyOnLoad
                SceneManager.LoadScene(nextSceneName);
            //}
            //else
            //{
            //    Debug.LogWarning("Aucun joueur n'est prêt !");
            //}
        }
    }
}