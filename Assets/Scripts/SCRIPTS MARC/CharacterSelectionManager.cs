using UnityEngine;
using UnityEngine.SceneManagement;

namespace SCRIPTS_MARC
{
    public class CharacterSelectionManager : MonoBehaviour
    {
        [Header("Configuration de la Scène")]
        [Tooltip("Les 4 panneaux UI, dans l'ordre P1, P2, P3, P4")]
        [SerializeField] private PlayerSelectionPanel[] panels;

        [Tooltip("Les 4 socles 3D, dans l'ordre P1, P2, P3, P4")]
        [SerializeField] private GameObject[] socles;
    
        [Header("Logique du Jeu")]
        [SerializeField] private string nextSceneName = "AreneSelection";
    
        private int readyPlayersCount = 0;

        // S'exécute au démarrage de la scène
        void Start()
        {
            // Initialise chaque panneau avec son socle correspondant
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i] != null && socles[i] != null)
                {
                    panels[i].Initialize(i, this, socles[i]);
                }
            }
        }

        // Appelée par un PlayerSelectionPanel quand son joueur est prêt
        public void PlayerIsReady(int playerIndex, string characterName)
        {
            readyPlayersCount++;
        
            // Sauvegarde le choix du personnage pour la prochaine scène
            PlayerPrefs.SetString("classTypePlayer" + (playerIndex + 1), characterName);
            Debug.Log("Player " + (playerIndex + 1) + " is ready with " + characterName);

            // Si tous les joueurs sont prêts, on change de scène
            if (readyPlayersCount == panels.Length)
            {
                Debug.Log("All players are ready! Loading next scene...");
                PlayerPrefs.SetInt("numberOfPlayer", panels.Length);
                PlayerPrefs.Save();
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
