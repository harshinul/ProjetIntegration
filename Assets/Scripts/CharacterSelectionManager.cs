using UnityEngine;

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

    void AddPlayer()
    {
        PlayerPrefs.SetInt("numberOfPlayer", currentPlayers + 1);
        
    }

    void PlayerIsReady(GameObject characterPrefab)
    {
        numberOfReadyPlayers++;


    }
    void PlayerIsNotReady()
    {
        numberOfReadyPlayers--;
    }

}
