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
    public struct PairButton
    {
        public Button button;
        public GameObject image;
        public TMP_Text title;
        public GameObject back;
        public TMP_Text playerSelect;
    }

    public List<PairButton> pairs;
    public TMP_Text ArenaSelected;
    public Dictionary<int, string> ArenaName = new Dictionary<int, string>()
    {
        {0,"FightReal" },
        {1,"TempleReal" },
        {2,"Cathedrale_Anthique" },
        {3,"Hell" }
    };

    private Dictionary<int, int> PlayerSelection = new Dictionary<int, int>();
    private int selectedIndex = -1;
    private int numberOfPlayers;
    private int currentSelection = 0; // Tour du joueur actuel (0 = J1, 1 = J2...)
    private int playersWhoSelected = 0;

    // AJOUTÉ : Indice de l'arène actuellement en surbrillance
    private int previewIndex = 0;

    // AJOUTÉ : Propriété publique pour que les Handlers sachent qui joue
    public int CurrentPlayerIndex => currentSelection;

    private void Start()
    {
        this.numberOfPlayers = PlayerPrefs.GetInt("numberOfPlayer", 4);
        for (int i = 0; i < numberOfPlayers; i++)
        {
            PlayerSelection[i] = -1;
        }

        // On désactive tout visuel au départ
        for (int i = 0; i < pairs.Count; i++)
        {
            if (pairs[i].image) pairs[i].image.SetActive(false);
            if (pairs[i].title) pairs[i].title.gameObject.SetActive(false);
            if (pairs[i].back) pairs[i].back.SetActive(false);
        }

        // très important: selectedIndex = -1 pour dire "rien encore"
        selectedIndex = -1;
        previewIndex = 0; // point de départ logique pour la navigation
        ActivatePreview(previewIndex);
    }

    private void Awake()
    {
        // SUPPRIMÉ : Nous n'utilisons plus les clics de souris, 
        // mais les fonctions publiques appelées par les manettes.
        // Vous pouvez les laisser si vous voulez un support souris ET manette,
        // mais cela complique la logique de qui clique.
        
        for (int i = 0; i < pairs.Count; i++)
        {
            int idx = i;
            pairs[i].button.onClick.AddListener(() => DoubleClick(idx));
        }
        
    }

    // ... (SelectArena, ValidateAll, DetermineArena restent identiques) ...
    // ...
    public void SelectArena(int arenaIndex)
    {
        PlayerSelection[currentSelection] = arenaIndex;
        selectedIndex = arenaIndex;
            // Met à jour le texte pour indiquer quel joueur doit sélectionner
            if (pairs[arenaIndex].playerSelect != null)
            {
                pairs[arenaIndex].playerSelect.text += $" J {currentSelection + 1}";
                pairs[arenaIndex].playerSelect.gameObject.SetActive(true);
            }
        
        playersWhoSelected++;

        Debug.Log($"Joueur {currentSelection + 1} a sélectionné l'arène {arenaIndex}");
        if (numberOfPlayers > 1 && playersWhoSelected < numberOfPlayers) // Modifié pour ne pas boucler après le dernier vote
        {
            currentSelection = (currentSelection + 1) % numberOfPlayers;
            Debug.Log($"C'est au tour du joueur {currentSelection + 1} de sélectionner une arène.");

            // Met à jour la prévisualisation pour le nouveau joueur
            ActivatePreview(previewIndex); 
        }
    }
    
    public void ValidateAll()
    {
        foreach (var selection in PlayerSelection.Values)
        {
            if (selection == -1)
            {
                Debug.Log("Tous les joueurs doivent sélectionner une arène avant de valider.");
                return; // AJOUTÉ : Arrête la validation si quelqu'un n'a pas voté
            }
        }
        int finalArena = DetermineArena();
        LoadArena(finalArena);
    }

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
    // ...

    /// <summary>
    /// Logique de double-clic, maintenant appelée par PlayerSubmit
    /// </summary>
    public void DoubleClick(int index)
    {
        // Si c'est le 2e clic (ou une confirmation)
        if (selectedIndex == index)
        {
            SelectArena(index);
            if (playersWhoSelected >= numberOfPlayers)
            {
                ValidateAll();
            }
        }
        else // Si c'est le 1er clic (prévisualisation)
        {
            
            ActivatePreview(index);
        }
    }

    // AJOUTÉ : Fonction publique pour la navigation par manette
    public void PlayerNavigate(int playerIndex, int direction)
    {
        // On vérifie si c'est bien le tour de ce joueur
        if (playerIndex != currentSelection) return;

        previewIndex += direction; // direction est 1 ou -1

        // Boucle la sélection
        if (previewIndex < 0) previewIndex = pairs.Count - 1;
        if (previewIndex >= pairs.Count) previewIndex = 0;

        // Met à jour l'aperçu

        ActivatePreview(previewIndex);
    }

    // AJOUTÉ : Fonction publique pour la sélection par manette
    public void PlayerSubmit(int playerIndex)
    {
        // On vérifie si c'est bien le tour de ce joueur
        if (playerIndex != currentSelection) return;

        // On appelle votre logique existante avec l'index en surbrillance
        DoubleClick(previewIndex);
    }

    /// <summary>
    /// Active preview pour l'index donné.
    /// </summary>
    private void ActivatePreview(int index)
    {
        // Désactiver tous les visuels
        for (int i = 0; i < pairs.Count; i++)
        {
            if (pairs[i].image) pairs[i].image.SetActive(false);
            if (pairs[i].title) pairs[i].title.gameObject.SetActive(false);
            if (pairs[i].back) pairs[i].back.SetActive(false); // AJOUTÉ : Gérer le fond ici
        }

        // Activer les visuels de l'arène sélectionnée
        if (index >= 0 && index < pairs.Count)
        {
            if (pairs[index].image) pairs[index].image.SetActive(true);
            if (pairs[index].title) pairs[index].title.gameObject.SetActive(true);
            if (pairs[index].back) pairs[index].back.SetActive(true); // AJOUTÉ : Activer le fond

            if (ArenaSelected != null && ArenaName.ContainsKey(index))
            {
                ArenaSelected.text = ArenaName[index];
            }

            selectedIndex = index;
        }
    }
    
    // ... (LoadArena et RandomArena restent identiques) ...
    // ...
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
    // ...
}