using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    const int maxPlayers = 4;
    [SerializeField] int currentPlayers = 0;
    public int numberOfReadyPlayers = 0;

    void Start()
    {
        PlayerPrefs.SetInt("numberOfPlayer", 0);
    }

    void Update()
    {

    }


    public void AddPlayer(string classPrefabName)
    {
        currentPlayers++;
        PlayerPrefs.SetString("classTypePlayer" + currentPlayers, classPrefabName);
        PlayerPrefs.SetInt("numberOfPlayer", currentPlayers);
        PlayerIsReady();
        
    }

    void PlayerIsReady()
    {
        numberOfReadyPlayers++;
        if(numberOfReadyPlayers == currentPlayers)
        {
            SceneManager.LoadScene("AreneSelection", LoadSceneMode.Single);
        }

    }
    void PlayerIsNotReady()
    {
        numberOfReadyPlayers--;
    }

}
