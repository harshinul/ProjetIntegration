using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 

namespace SCRIPTS_MARC
{
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

            playerInputManager.DisableJoining();

            foreach (var player in PlayerInput.all.ToArray())
            {
                Destroy(player.gameObject);
            }

            playerWhoJoinedCount = 0;
            readyPlayersCount = 0;

            foreach (var socle in socles)
            {
                if (socle != null) socle.SetActive(false);
            }

            if (playerInputManager.playerPrefab == null)
            {
                Debug.LogError("ERREUR : Player Prefab non assigné dans le PlayerInputManager !");
            }

            playerInputManager.EnableJoining();
        }

        void Start()
        {
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

            if (playerIndex < panels.Length && panels[playerIndex] != null && 
                playerIndex < socles.Length && socles[playerIndex] != null)
            {
                PlayerSelectionPanel panel = panels[playerIndex];
                GameObject socle = socles[playerIndex];

                panel.gameObject.SetActive(true); 
                socle.SetActive(true);
                panel.Initialize(playerIndex, this, socle);

                playerWhoJoinedCount++;

                return panel; 
            }

            Debug.LogWarning($"Aucun panneau ou socle trouvé pour l'index {playerIndex}");
            return null; 
        }

        public void PlayerIsReady(int playerIndex, string characterName)
        {
            readyPlayersCount++;
        
            PlayerPrefs.SetString("classTypePlayer" + (playerIndex + 1), characterName);
            Debug.Log("Player " + (playerIndex + 1) + " is ready with " + characterName);

            
            if (readyPlayersCount > 1 && readyPlayersCount == playerWhoJoinedCount)
            {
                StartGame();
            }
        }
        
        public void StartGame()
        {
                Debug.Log(readyPlayersCount + " joueurs sont prêts !");
                PlayerPrefs.SetInt("numberOfPlayer", readyPlayersCount);
                PlayerPrefs.Save();
                
                playerInputManager.DisableJoining();
                
                SceneManager.LoadScene(nextSceneName);
        }
    }
}