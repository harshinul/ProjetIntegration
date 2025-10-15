using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [System.Serializable]
    public struct PairButton
    {
        public Button button;
        public GameObject image;
        public TMP_Text title;
    }

    public List<PairButton> pairs;
    public Button ArenaConfirmation;
    public Dictionary<int, string> ArenaName = new Dictionary<int, string>()
    {
        {0,"FightReal" },
        {1,"Temple" },
        {2,"Cathedrale_Anthique" },
        {3,"Cathedrale_Anthique" }
    };
    private int selectedIndex = -1;

    private void Awake()
    {
        for (int i = 0; i < pairs.Count; i++)
        {
            int idx = i;
            pairs[i].button.onClick.AddListener(() => DoubleClick(idx));
        }

        ArenaConfirmation.onClick.AddListener(LevelSelected);
    }
    public void DoubleClick(int index)
    {
        if (selectedIndex == index)
        {
            LevelSelected();
        }
        else
        {
            ActivateOnly(index);
        }
    }

    void ActivateOnly(int index)
    {
        selectedIndex = index;

        for (int i = 0; i < pairs.Count; i++)
        {
            if (pairs[i].image) pairs[i].image.SetActive(i == index);
            if (pairs[i].title) pairs[i].title.gameObject.SetActive(i == index);
        }

        PlayerPrefs.SetInt("ArenaIndex", index);
        PlayerPrefs.Save();
        Debug.Log("Selected Arena : " + index);
    }

    void LevelSelected()
    {
        if (ArenaName.TryGetValue(selectedIndex, out string name))
        {
            Debug.Log("Chargement Scene " + name);
            SceneManager.LoadScene(name);
        }
    }
}
