using UnityEngine;
using UnityEngine.UI;

public class PlayersManagerScript : MonoBehaviour
{

    [SerializeField] Transform[] spawnPoints = new Transform[5];
    [SerializeField] Image[] backHealthBar = new Image[4];
    [SerializeField] Image[] frontHealthBar = new Image[4];

    [SerializeField] GameObject warriorPrefab;
    [SerializeField] GameObject assassinPrefab;
    [SerializeField] GameObject magePrefab;

    private void Start()
    {
        int numberOfPlayer = PlayerPrefs.GetInt("numberOfPlayer", 1);
        SpawnPlayers(numberOfPlayer);
    }

    void SpawnPlayerType(int playerNumber, Vector3 position)
    {
        GameObject player;
        string classTypeString = PlayerPrefs.GetString("classTypePlayer" + playerNumber);

        if(classTypeString == warriorPrefab.name)
        {
            player = Instantiate(warriorPrefab, position, Quaternion.identity);
        }
        else if(classTypeString == assassinPrefab.name)
        {
            player = Instantiate(assassinPrefab, position, Quaternion.identity);
        }
        else if (classTypeString == magePrefab.name)
        {
            player = Instantiate(magePrefab, position, Quaternion.identity);
        }
        else
        {
            player = Instantiate(warriorPrefab, position, Quaternion.identity);
        }

        player.GetComponent<PlayerHealthComponent>().SetHealthBarUI(backHealthBar[playerNumber - 1], frontHealthBar[playerNumber - 1]);
    }

    void SpawnPlayers(int numberOfPlayer)
    {
        switch (numberOfPlayer)
        {
            case 1:
                SpawnPlayerType(1, spawnPoints[0].position);
                break;
            case 2:
                SpawnPlayerType(1, spawnPoints[0].position);
                SpawnPlayerType(2, spawnPoints[4].position);
                break;
            case 3:
                SpawnPlayerType(1, spawnPoints[0].position);
                SpawnPlayerType(2, spawnPoints[2].position);
                SpawnPlayerType(3, spawnPoints[4].position);
                break;
            case 4:
                SpawnPlayerType(1, spawnPoints[0].position);
                SpawnPlayerType(2, spawnPoints[1].position);
                SpawnPlayerType(3, spawnPoints[3].position);
                SpawnPlayerType(4, spawnPoints[4].position);
                break;
            default:
                break;
        }
    }
}
