using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{

    [SerializeField] Transform[] spawnPoints = new Transform[5];
    [SerializeField] Image[] backHealthBar = new Image[4];
    [SerializeField] Image[] frontHealthBar = new Image[4];
    [SerializeField] Image[] ultBar = new Image[4];

    [SerializeField] GameObject warriorPrefab;
    [SerializeField] GameObject assassinPrefab;
    [SerializeField] GameObject magePrefab;

    [SerializeField] Canvas gameOverCanva;
    [SerializeField] Canvas pauseMenuCanva;

    List<PlayerHealthComponent> playersHealthComponents = new List<PlayerHealthComponent>();
    List<PlayerPauseMenuComponent> playersPauseMenuComponents = new List<PlayerPauseMenuComponent>();

    private void Start()
    {
        PlayerPrefs.SetInt("numberOfPlayer", 2);
        PlayerPrefs.SetString("classTypePlayer1", warriorPrefab.name);
        PlayerPrefs.SetString("classTypePlayer2", magePrefab.name);
        //PlayerPrefs.SetString("classTypePlayer3", assassinPrefab.name);
        //PlayerPrefs.SetString("classTypePlayer4", magePrefab.name);
        SpawnPlayers(PlayerPrefs.GetInt("numberOfPlayer"));

        gameOverCanva.enabled = false;
        pauseMenuCanva.enabled = false;
    }

    private void Update()
    {
        (bool isPaused, int playerIndex) = CheckIfPlayerPaused();
        if (isPaused)
        {
            pauseMenuCanva.enabled = true;
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenuCanva.enabled = false;
            Time.timeScale = 1f;
        }


        if (CheckNumberOfPlayerAlive() == 1)
        {
            GameOver();
        }
    }

    void SpawnPlayerType(int playerNumber, Vector3 position, Quaternion rotation)
    {
        GameObject player;
        PlayerHealthComponent pHC;
        UltimateAbilityComponent uAC;
        string classTypeString = PlayerPrefs.GetString("classTypePlayer" + playerNumber);

        if (classTypeString == warriorPrefab.name)
        {
            player = Instantiate(warriorPrefab, position, rotation);
        }
        else if (classTypeString == assassinPrefab.name)
        {
            player = Instantiate(assassinPrefab, position, rotation);
        }
        else if (classTypeString == magePrefab.name)
        {
            player = Instantiate(magePrefab, position, rotation);
        }
        else
        {
            Debug.Log("Class type not recognized, defaulting to Warrior");
            player = Instantiate(warriorPrefab, position, rotation);
        }

        pHC = player.GetComponent<PlayerHealthComponent>();
        uAC = player.GetComponent<UltimateAbilityComponent>();
        uAC.SetUltBarUI(ultBar[playerNumber - 1]);
        pHC.SetHealthBarUI(backHealthBar[playerNumber - 1], frontHealthBar[playerNumber - 1]);
        playersHealthComponents.Add(pHC);
        playersPauseMenuComponents.Add(player.GetComponent<PlayerPauseMenuComponent>());
    }

    void SpawnPlayers(int numberOfPlayer)
    {
        Quaternion rotationLeft = Quaternion.Euler(0f, -90f, 0f);
        Quaternion rotationRight = Quaternion.Euler(0f, 90f, 0f);

        switch (numberOfPlayer)
        {
            case 1:
                SpawnPlayerType(1, spawnPoints[0].position, rotationRight);
                break;
            case 2:
                SpawnPlayerType(1, spawnPoints[0].position, rotationRight);
                SpawnPlayerType(2, spawnPoints[4].position, rotationLeft);
                break;
            case 3:
                SpawnPlayerType(1, spawnPoints[0].position, rotationRight);
                SpawnPlayerType(2, spawnPoints[2].position, rotationLeft);
                SpawnPlayerType(3, spawnPoints[4].position, rotationLeft);
                break;
            case 4:
                SpawnPlayerType(1, spawnPoints[0].position, rotationRight);
                SpawnPlayerType(2, spawnPoints[1].position, rotationLeft);
                SpawnPlayerType(3, spawnPoints[3].position, rotationRight);
                SpawnPlayerType(4, spawnPoints[4].position, rotationLeft);
                break;
            default:
                Debug.Log("Number of players not supported");
                break;
        }
    }

    int CheckNumberOfPlayerAlive()
    {
        int playerAlive = 0;
        foreach (PlayerHealthComponent player in playersHealthComponents)
        {
            if (!player.PlayerIsDead())
            {
                playerAlive++;
            }
        }
        return playerAlive;
    }

    (bool,int) CheckIfPlayerPaused()
    {
        int index = 0;
        foreach (PlayerPauseMenuComponent player in playersPauseMenuComponents)
        {
            if (player.isPaused)
            {
                return (true,index);
            }
            index++;
        }
        return (false,0);
    }

    void GameOver()
    {
        gameOverCanva.enabled = true;
        Debug.Log("Game Over");
        Time.timeScale = 0f;
    }

}
