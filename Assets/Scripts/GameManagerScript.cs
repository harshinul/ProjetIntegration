using System.Collections.Generic;
using System.Linq; 
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using SCRIPTS_MARC;
using Unity.Cinemachine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints = new Transform[5];
    [SerializeField] GameObject warriorPrefab;
    [SerializeField] GameObject assassinPrefab;
    [SerializeField] GameObject magePrefab;
    [SerializeField] GameObject[] playersUI = new GameObject[4];
    [SerializeField] Image[] backHealthBar = new Image[4];
    [SerializeField] Image[] frontHealthBar = new Image[4];
    [SerializeField] Image[] ultBar = new Image[4];
    [SerializeField] CinemachineTargetGroup cinemachineTargetGroup;
    [SerializeField] Canvas afterGameLocal;
    [SerializeField] Canvas pauseMenuCanva;

    // Listes pour suivre les joueurs
    List<PlayerHealthComponent> playersHealthComponents = new List<PlayerHealthComponent>();
    List<PlayerPauseMenuComponent> playersPauseMenuComponents = new List<PlayerPauseMenuComponent>();

    // Liste pour trouver les prefabs par nom
    private List<GameObject> characterPrefabsList = new List<GameObject>();

    private bool isGameOver = false;

    private void Start()
    {
        // On remplit la liste des prefabs
        if (warriorPrefab) characterPrefabsList.Add(warriorPrefab);
        if (assassinPrefab) characterPrefabsList.Add(assassinPrefab);
        if (magePrefab) characterPrefabsList.Add(magePrefab);

        // UI
        for (int i = 0; i < playersUI.Length; i++)
        {
            playersUI[i].SetActive(false);
        }
        pauseMenuCanva.enabled = false;
        afterGameLocal.enabled = false;
        Time.timeScale = 1f;

        SpawnPlayersFromHandlers();
    }

    private void Update()
    {
        if (isGameOver) return;

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

    void SpawnPlayersFromHandlers()
    {
        // 1. Trouver tous les "handlers" de joueur qui ont persisté
        var playerHandlers = FindObjectsOfType<PlayerInputHandler>();

        if (playerHandlers.Length == 0)
        {
            return;
        }

        // 2. Trier les handlers par index pour s'assurer que P1 est P1, P2 est P2, etc.
        var sortedHandlers = playerHandlers.OrderBy(h => h.GetComponent<PlayerInput>().playerIndex).ToArray();

        int totalPlayers = sortedHandlers.Length;

        foreach (PlayerInputHandler handler in sortedHandlers)
        {
            PlayerInput playerInput = handler.GetComponent<PlayerInput>();
            int playerIndex = playerInput.playerIndex; // Index de 0 à 3
            int playerNumber = playerIndex + 1; // Numéro de 1 à 4

            // 3. Trouver le prefab du personnage choisi
            string classTypeString = PlayerPrefs.GetString("classTypePlayer" + playerNumber);
            GameObject prefabToSpawn = characterPrefabsList.FirstOrDefault(p => p.name == classTypeString);

            if (prefabToSpawn == null)
            {
                prefabToSpawn = warriorPrefab;
            }

            // 4. Obtenir la position et rotation de spawn
            (Vector3 pos, Quaternion rot) spawnData = GetSpawnData(playerIndex, totalPlayers);

            // 5. Instancier le personnage (DÉSACTIVÉ temporairement)
            GameObject playerCharacter = Instantiate(prefabToSpawn, spawnData.pos, spawnData.rot);
            playerCharacter.SetActive(false);

            // 6. Changer l'Action Map pour les contrôles de jeu
            playerInput.SwitchCurrentActionMap("Player");

            // 7. Assigner le movementComponent au personnage
            PlayerMovementComponent movementComponent = playerCharacter.GetComponent<PlayerMovementComponent>();
            if (movementComponent != null)
            {
                movementComponent.SetPlayerInput(playerInput);
            }
            else
            {
                Debug.LogWarning($"PlayerMovementComponent non trouvé sur {playerCharacter.name}");
            }
            // 8. Assigner le attackComponent au personnage
            PlayerAttackScript attackComponent = playerCharacter.GetComponent<PlayerAttackScript>();
            if (attackComponent != null)
            {
                attackComponent.SetPlayerInput(playerInput);
            }
            else
            {
                Debug.LogWarning($"PlayerAttackScript non trouvé sur {playerCharacter.name}");
            }

            // 9. RÉACTIVER le personnage
            playerCharacter.SetActive(true);

            // 9. Activation du UI et configuration de la Health Bar
            playersUI[playerIndex].SetActive(true);
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

            // 10. Configuration du menu pause
            PlayerPauseMenuComponent pPMC = playerCharacter.GetComponent<PlayerPauseMenuComponent>();
            if (pPMC != null)
            {
                pPMC.SetPlayerInput(playerInput);
                playersPauseMenuComponents.Add(pPMC);
            }
            UltimateAbilityComponent uAC;
            uAC = playerCharacter.GetComponent<UltimateAbilityComponent>();
            uAC.SetUltBarUI(ultBar[playerNumber - 1]);

            // 11. Ajouter le joueur au Cinemachine Target Group
            cinemachineTargetGroup.AddMember(playerCharacter.GetComponentInChildren<headBodyPart>().transform, 1f, 1f);

            // 12. Changer la couleur de la flèche du joueur

            switch(playerNumber)
            {
                case 1:
                    playerCharacter.GetComponentInChildren<Image>().color = Color.blue;
                    break;
                case 2:
                    playerCharacter.GetComponentInChildren<Image>().color = Color.red;
                    break;
                case 3:
                    playerCharacter.GetComponentInChildren<Image>().color = Color.green;
                    break;
                case 4:
                    playerCharacter.GetComponentInChildren<Image>().color = Color.yellow;
                    break;
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
        return (spawnPoints[0].position, rotationRight);
    }

    int CheckNumberOfPlayerAlive()
    {
        int playerAlive = 0;
        foreach (PlayerHealthComponent player in playersHealthComponents)
        {
            if (!player.isPlayerDead())
            {
                playerAlive++;
            }
            else
            {
                cinemachineTargetGroup.RemoveMember(player.GetComponentInChildren<headBodyPart>().transform);
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
        isGameOver = true;

        afterGameLocal.enabled = true;
        //Time.timeScale = 0f;
    }
}