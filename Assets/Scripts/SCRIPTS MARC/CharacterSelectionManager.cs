using UnityEngine;
using UnityEngine.SceneManagement;

namespace SCRIPTS_MARC
{
    public class CharacterSelectionManager : MonoBehaviour
    {
        [SerializeField] private PlayerSelectionPanel[] panels;

        [SerializeField] private GameObject[] socles;
    
        [SerializeField] private string nextSceneName = "AreneSelection";
    
        private int readyPlayersCount = 0;
        
        void Start()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i] != null && socles[i] != null)
                {
                    panels[i].Initialize(i, this, socles[i]);
                }
            }
        }

        public void PlayerIsReady(int playerIndex, string characterName)
        {
            readyPlayersCount++;
        
            PlayerPrefs.SetString("classTypePlayer" + (playerIndex + 1), characterName);
            Debug.Log("Player " + (playerIndex + 1) + " is ready with " + characterName);
        }
        public void StartGame()
        {
            if (readyPlayersCount > 0)
            {
                Debug.Log(readyPlayersCount + "players are ready!");
                PlayerPrefs.SetInt("numberOfPlayer", readyPlayersCount);
                PlayerPrefs.Save();
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("No player are ready!");
            }
        }
    }
}
