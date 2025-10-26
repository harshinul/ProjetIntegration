using System.Collections.Generic;
using System.Linq; // <-- NÉCESSAIRE
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // <-- NÉCESSAIRE
using SCRIPTS_MARC; // <-- NÉCESSAIRE (pour PlayerInputHandler)
//using NUnit.Framework; // <-- Vous pouvez probablement supprimer ceci

public class GameManagerScript : MonoBehaviour
{
    [Header("Références de Spawning")]
    [SerializeField] Transform[] spawnPoints = new Transform[5];
    [SerializeField] GameObject warriorPrefab;
    [SerializeField] GameObject assassinPrefab;
    [SerializeField] GameObject magePrefab;

    [Header("Références UI")]
    [SerializeField] Image[] backHealthBar = new Image[4];
    [SerializeField] Image[] frontHealthBar = new Image[4];
    [SerializeField] Canvas gameOverCanva;
    [SerializeField] Canvas pauseMenuCanva;

    // Listes pour suivre les joueurs
    List<PlayerHealthComponent> playersHealthComponents = new List<PlayerHealthComponent>();
    List<PlayerPauseMenuComponent> playersPauseMenuComponents = new List<PlayerPauseMenuComponent>();

    // Liste privée pour trouver les prefabs par nom
    private List<GameObject> characterPrefabsList = new List<GameObject>();

    private void Start()
    {
        // On remplit la liste des prefabs
        if (warriorPrefab) characterPrefabsList.Add(warriorPrefab);
        if (assassinPrefab) characterPrefabsList.Add(assassinPrefab);
        if (magePrefab) characterPrefabsList.Add(magePrefab);

        // UI
        gameOverCanva.enabled = false;
        pauseMenuCanva.enabled = false;
        Time.timeScale = 1f;

        // On remplace votre ancienne logique de spawn par la nouvelle
        SpawnPlayersFromHandlers();
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

        // On vérifie > 1 pour les modes à 1 joueur (ou test)
        if (CheckNumberOfPlayerAlive() <= 1 && playersHealthComponents.Count > 1)
        {
            GameOver();
        }
    }

    /// <summary>
    /// NOUVELLE MÉTHODE : Trouve tous les PlayerInputHandlers et instancie leurs personnages.
    /// </summary>
    void SpawnPlayersFromHandlers()
    {
        // 1. Trouver tous les "handlers" de joueur qui ont persisté
        var playerHandlers = FindObjectsOfType<PlayerInputHandler>();

        if (playerHandlers.Length == 0)
        {
            Debug.LogWarning("Aucun PlayerInputHandler trouvé. Impossible d'instancier les joueurs.");
            // Note : Vous pourriez vouloir instancier un joueur par défaut ici pour les tests
            return;
        }

        // 2. Trier les handlers par index pour s'assurer que P1 est P1, P2 est P2, etc.
        var sortedHandlers = playerHandlers.OrderBy(h => h.GetComponent<PlayerInput>().playerIndex).ToArray();
        
        int totalPlayers = sortedHandlers.Length;
        Debug.Log($"Instanciation de {totalPlayers} joueurs...");

        foreach (PlayerInputHandler handler in sortedHandlers)
        {
            PlayerInput playerInput = handler.GetComponent<PlayerInput>();
            int playerIndex = playerInput.playerIndex; // Index de 0 à 3
            int playerNumber = playerIndex + 1; // Numéro de 1 à 4

            // 3. Trouver le prefab du personnage choisi (logique de votre ancien SpawnPlayerType)
            string classTypeString = PlayerPrefs.GetString("classTypePlayer" + playerNumber);
            GameObject prefabToSpawn = characterPrefabsList.FirstOrDefault(p => p.name == classTypeString);

            if (prefabToSpawn == null)
            {
                Debug.LogWarning($"Prefab '{classTypeString}' non trouvé pour Joueur {playerNumber}. Utilisation du Guerrier.");
                prefabToSpawn = warriorPrefab;
            }

            (Vector3 pos, Quaternion rot) spawnData = GetSpawnData(playerIndex, totalPlayers);

            // 5. Instancier le personnage (DÉSACTIVÉ temporairement)
            GameObject playerCharacter = Instantiate(prefabToSpawn, spawnData.pos, spawnData.rot);
            playerCharacter.SetActive(false); // ← IMPORTANT

            // 6. Changer l'Action Map pour les contrôles de jeu
            playerInput.SwitchCurrentActionMap("Player"); 

            // 7. Parenter le Handler au personnage
            handler.transform.SetParent(playerCharacter.transform);

            // 8. RÉACTIVER le personnage
            playerCharacter.SetActive(true); // ← IMPORTANT

            // 9. Configuration de la Health Bar (reste identique)
            PlayerHealthComponent pHC = playerCharacter.GetComponent<PlayerHealthComponent>();
            if (pHC != null && playerIndex < backHealthBar.Length)
            {
                pHC.SetHealthBarUI(backHealthBar[playerIndex], frontHealthBar[playerIndex]);
                playersHealthComponents.Add(pHC);
            }
            else
            {
                Debug.LogWarning($"Pas de HealthBar ou de HealthComponent pour Joueur {playerNumber}");
            }

            PlayerPauseMenuComponent pPMC = playerCharacter.GetComponent<PlayerPauseMenuComponent>();
            if (pPMC != null)
            {
                playersPauseMenuComponents.Add(pPMC);
            }
        }
    }

    (Vector3, Quaternion) GetSpawnData(int playerIndex, int totalPlayers)
    {
        Quaternion rotationLeft = Quaternion.Euler(0f, -90f, 0f);
        Quaternion rotationRight = Quaternion.Euler(0f, 90f, 0f);

        // Note : playerIndex est de 0 à 3
        switch (totalPlayers)
        {
            case 1: // Index 0
                return (spawnPoints[0].position, rotationRight);
            case 2: // Index 0, 1
                if (playerIndex == 0) return (spawnPoints[0].position, rotationRight);
                if (playerIndex == 1) return (spawnPoints[4].position, rotationLeft);
                break;
            case 3: // Index 0, 1, 2
                if (playerIndex == 0) return (spawnPoints[0].position, rotationRight);
                if (playerIndex == 1) return (spawnPoints[2].position, rotationLeft);
                if (playerIndex == 2) return (spawnPoints[4].position, rotationLeft);
                break;
            case 4: // Index 0, 1, 2, 3
                if (playerIndex == 0) return (spawnPoints[0].position, rotationRight);
                if (playerIndex == 1) return (spawnPoints[1].position, rotationLeft);
                if (playerIndex == 2) return (spawnPoints[3].position, rotationRight);
                if (playerIndex == 3) return (spawnPoints[4].position, rotationLeft);
                break;
        }
        
        Debug.LogWarning($"Cas de spawn non géré pour {playerIndex} / {totalPlayers}. Utilisation du point 0.");
        return (spawnPoints[0].position, rotationRight);
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

    (bool, int) CheckIfPlayerPaused()
    {
        int index = 0;
        foreach (PlayerPauseMenuComponent player in playersPauseMenuComponents)
        {
            if (player.isPaused)
            {
                return (true, index);
            }
            index++;
        }
        return (false, 0);
    }

    void GameOver()
    {
        gameOverCanva.enabled = true;
        Debug.Log("Game Over");
        Time.timeScale = 0f;
    }

}