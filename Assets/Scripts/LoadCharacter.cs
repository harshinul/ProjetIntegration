using UnityEngine;
using TMPro;


public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform spawnPoint;
    public TMP_Text label;
    public void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter");
        GameObject prefab = characterPrefabs[selectedCharacter];
        label.text = prefab.name;
    }
}
