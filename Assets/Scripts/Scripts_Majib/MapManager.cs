using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [System.Serializable]
    /// <summary>
    /// PairButton groups a UI button with its preview image and title text.
    /// Used to represent one selectable arena entry in the UI.
    /// </summary>
    public struct PairButton
    {
        public Button button;
        public GameObject image;
        public TMP_Text title;
    }

    public List<PairButton> pairs;
    //public Button ArenaConfirmation;
    public TMP_Text ArenaSelected;
    public Dictionary<int, string> ArenaName = new Dictionary<int, string>()
    {
        {0,"FightReal" },
        {1,"Temple" },
        {2,"Cathedrale_Anthique" },
        {3,"Hell" }
    };
    private Dictionary<int, int> PlayerSelection = new Dictionary<int, int>();
    private int selectedIndex = -1;
    private int numberOfPlayers;
    private int currentSelection = 0;
    private int playersWhoSelected = 0;

    /// <summary>
    /// Unity Start: initialize player selections using saved player count.
    /// Populates the PlayerSelection dictionary with -1 (no selection) for each player.
    /// </summary>
    private void Start()
    {
        this.numberOfPlayers = 4;//PlayerPrefs.GetInt("numberOfPlayer", 1);
        for (int i = 0; i < numberOfPlayers; i++)
        {
            PlayerSelection[i] = -1;
        }
    }

    /// <summary>
    /// Unity Awake: wire up UI button callbacks for preview and confirmation.
    /// Each arena button receives a DoubleClick callback with its index.
    /// The confirmation button triggers ValidateAll().
    /// </summary>
    private void Awake()
    {
        for (int i = 0; i < pairs.Count; i++)
        {
            int idx = i;
            pairs[i].button.onClick.AddListener(() => DoubleClick(idx));
        }

        //ArenaConfirmation.onClick.AddListener(ValidateAll);
    }

    /// <summary>
    /// Register the current player's arena choice.
    /// Advances turn to the next player when relevant and updates internal counters.
    /// </summary>
    /// <param name="arenaIndex">Index of the arena selected by the current player.</param>
    public void SelectArena(int arenaIndex)
    {
        PlayerSelection[currentSelection] = arenaIndex;
        selectedIndex = arenaIndex;  
        playersWhoSelected++;

        Debug.Log($"Joueur {currentSelection + 1} a sélectionné l'arène {arenaIndex}");
        if (numberOfPlayers > 1)
        {
            currentSelection = (currentSelection + 1) % numberOfPlayers;
            Debug.Log($"C'est au tour du joueur {currentSelection + 1} de sélectionner une arène.");
        }
    }

    /// <summary>
    /// ValidateAll ensures every player has chosen an arena (logs if someone hasn't),
    /// determines the final arena via DetermineArena() and loads it.
    /// </summary>
    public void ValidateAll()
    {
        foreach (var selection in PlayerSelection.Values)
        {
            if (selection == -1)
            {
                Debug.Log("Tous les joueurs doivent sélectionner une arène avant de valider.");
            }
            
        }
        int finalArena = DetermineArena();
        LoadArena(finalArena);
    }

    /// <summary>
    /// Determine the arena to use based on player votes.
    /// - If all players selected the same arena => return that arena.
    /// - If a majority (>=2) selected the same arena => return the majority arena.
    /// - Otherwise pick randomly among selected arenas.
    /// </summary>
    /// <returns>Index of the chosen arena.</returns>
    private int DetermineArena()
    {
        //Count votes for each arena
        Dictionary<int, int> voteCount = new Dictionary<int, int>();

        foreach (var selection in PlayerSelection.Values)
        {
            if (voteCount.ContainsKey(selection))
                voteCount[selection]++;
            else
                voteCount[selection] = 1;
        }

        // Trouver le nombre maximum de votes
        int maxVotes = voteCount.Values.Max();

        // Si tous les joueurs ont sélectionné la même arène
        if (maxVotes == numberOfPlayers)
        {
            int arena = voteCount.First(x => x.Value == maxVotes).Key;
            Debug.Log($"Tous les joueurs ont sélectionné la même arène : {arena}");
            return arena;
        }

        // Si 2 ou plus ont sélectionné la même arène
        if (maxVotes >= 2)
        {
            int arena = voteCount.First(x => x.Value == maxVotes).Key;
            Debug.Log($"{maxVotes} joueurs ont sélectionné l'arène {arena} - Majorité atteinte!");
            return arena;
        }

        // Sinon, choix aléatoire parmi toutes les sélections
        Debug.Log("Aucune majorité - Choix aléatoire parmi les sélections");
        List<int> allSelections = new List<int>(PlayerSelection.Values);
        int randomIndex = UnityEngine.Random.Range(0, allSelections.Count);
        int selectedArena = allSelections[randomIndex];
        Debug.Log($"Arène choisie aléatoirement : {selectedArena}");
        return selectedArena;
    }

    /// <summary>
    /// Handle a button click: if the same item is clicked twice in a row, confirm selection;
    /// otherwise show the preview for that index.
    /// </summary>
    /// <param name="index">Index of the clicked arena button.</param>
    public void DoubleClick(int index)
    {
        if (selectedIndex == index)
        {
            SelectArena(index);
            if (playersWhoSelected >= numberOfPlayers)
            {
                ValidateAll();
            }
        }
        else
        {
            ActivatePreview(index);
        }
    }

    /// <summary>
    /// Activate preview for the given arena index: show the preview image and title
    /// for the selected index and hide others.
    /// </summary>
    /// <param name="index">Index of the arena to preview.</param>
    void ActivatePreview(int index)
    {
        selectedIndex = index;

        for (int i = 0; i < pairs.Count; i++)
        {
            if (pairs[i].image) pairs[i].image.SetActive(i == index);
            if (pairs[i].title) pairs[i].title.gameObject.SetActive(i == index);
        }

        
    }

    /// <summary>
    /// Load the scene associated with the given arena index and persist that choice to PlayerPrefs.
    /// </summary>
    /// <param name="arenaIndex">Index of the arena to load.</param>
    void LoadArena(int arenaIndex)
    {
        if (ArenaName.TryGetValue(arenaIndex, out string name))
        {
            PlayerPrefs.SetInt("ArenaIndex", arenaIndex);
            PlayerPrefs.Save();
            Debug.Log("Chargement Scene " + name);
            SceneManager.LoadScene(name);
        }
    }
    public void RandomArena()
    {
        int randomIndex = UnityEngine.Random.Range(0, pairs.Count);
        LoadArena(randomIndex);
    }
}
