using UnityEngine;

public class PlayersManagerScript : MonoBehaviour
{

    [SerializeField] private Transform[] spawnPoints = new Transform[5];

    [SerializeField] GameObject warriorPrefab;
    [SerializeField] GameObject assassinPrefab;
    [SerializeField] GameObject magePrefab;

    private void Start()
    {
        PlayerPrefs.SetInt("numberOfPlayer", 2);
        PlayerPrefs.SetString("classTypePlayer1", "Warrior");
        PlayerPrefs.SetString("classTypePlayer2", "Mage");
        SpawnPlayers(PlayerPrefs.GetInt("numberOfPlayer"));
    }

    GameObject AssignePlayerType(int playerNumber)
    {
        string classTypeString = PlayerPrefs.GetString("classTypePlayer" + playerNumber);
        switch
            (classTypeString)
        {
            case "Warrior":
                return warriorPrefab;
            case "Assassin":
                return assassinPrefab;
            case "Mage":
                return magePrefab;
            default:
                Debug.Log("Class type not recognized, defaulting to Warrior");
                return magePrefab;
        }
    }
    void SpawnPlayers(int numberOfPlayer)
    {
        switch
            (numberOfPlayer)
        {
            case 1:
                Instantiate(AssignePlayerType(1), spawnPoints[0].position, Quaternion.identity);
                break;
            case 2:
                Instantiate(AssignePlayerType(1), spawnPoints[0].position, Quaternion.identity);
                Instantiate(AssignePlayerType(2), spawnPoints[4].position, Quaternion.identity);
                break;
            case 3:
                Instantiate(AssignePlayerType(1), spawnPoints[0].position, Quaternion.identity);
                Instantiate(AssignePlayerType(2), spawnPoints[2].position, Quaternion.identity);
                Instantiate(AssignePlayerType(3), spawnPoints[4].position, Quaternion.identity);
                break;
            case 4:
                Instantiate(AssignePlayerType(1), spawnPoints[0].position, Quaternion.identity);
                Instantiate(AssignePlayerType(2), spawnPoints[1].position, Quaternion.identity);
                Instantiate(AssignePlayerType(3), spawnPoints[3].position, Quaternion.identity);
                Instantiate(AssignePlayerType(4), spawnPoints[4].position, Quaternion.identity);
                break;
            default:
                Debug.Log("Number of players not supported");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
