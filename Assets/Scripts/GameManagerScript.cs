using SCRIPTS_MARC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [SerializeField] Selectable element; // bouton par défaut
    [SerializeField] Selectable gameOver; // bouton par défaut

    List<PlayerHealthComponent> playersHealthComponents = new List<PlayerHealthComponent>();
    List<PlayerPauseMenuComponent> playersPauseMenuComponents = new List<PlayerPauseMenuComponent>();
    private List<GameObject> characterPrefabsList = new List<GameObject>();


    private bool isGameOver = false;
    private bool pauseJustOpened = false; 

    public void Kill() // Fonction de debug pour tuer tous les joueurs
    {
        if (Keyboard.current.fKey.isPressed) // Utilisation correcte de l'API InputSystem
        {
            foreach (PlayerHealthComponent phc in playersHealthComponents)
            {
                phc.TakeDamage(99999f);
            }
        }
    }

    private void Start()
    {
        if (warriorPrefab) characterPrefabsList.Add(warriorPrefab);
        if (assassinPrefab) characterPrefabsList.Add(assassinPrefab);
        if (magePrefab) characterPrefabsList.Add(magePrefab);

        for (int i = 0; i < playersUI.Length; i++)
            playersUI[i].SetActive(false);

        pauseMenuCanva.enabled = false;
        afterGameLocal.enabled = false;
        Time.timeScale = 1f;

        SpawnPlayersFromHandlers();
    }

    private void Update()
    {
        Kill();
        if (isGameOver) return;

        (bool isPaused, int playerIndex) = CheckIfPlayerPaused();

        if (isPaused)
        {
            pauseMenuCanva.enabled = true;
            Time.timeScale = 0f;

            // Sélection du bouton UNIQUEMENT au moment où la pause s'ouvre
            if (!pauseJustOpened)
            {
                pauseJustOpened = true;

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(element.gameObject);
            }
        }
        else if (!isGameOver)
        {
            pauseMenuCanva.enabled = false;
            Time.timeScale = 1f;

            pauseJustOpened = false; // reset pour la prochaine pause

            EventSystem.current.SetSelectedGameObject(null);
        }

        if (CheckNumberOfPlayerAlive() <= 1 && playersHealthComponents.Count > 1)
            GameOver();
    }

    void SpawnPlayersFromHandlers()
    {
        var playerHandlers = FindObjectsOfType<PlayerInputHandler>();

        if (playerHandlers.Length == 0)
            return;

        var sortedHandlers = playerHandlers.OrderBy(h => h.GetComponent<PlayerInput>().playerIndex).ToArray();

        int totalPlayers = sortedHandlers.Length;

        foreach (PlayerInputHandler handler in sortedHandlers)
        {
            PlayerInput playerInput = handler.GetComponent<PlayerInput>();
            int playerIndex = playerInput.playerIndex;
            int playerNumber = playerIndex + 1;

            string classTypeString = PlayerPrefs.GetString("classTypePlayer" + playerNumber);
            GameObject prefabToSpawn = characterPrefabsList.FirstOrDefault(p => p.name == classTypeString);

            if (prefabToSpawn == null)
                prefabToSpawn = warriorPrefab;

            (Vector3 pos, Quaternion rot) spawnData = GetSpawnData(playerIndex, totalPlayers);

            GameObject playerCharacter = Instantiate(prefabToSpawn, spawnData.pos, spawnData.rot);
            playerCharacter.SetActive(false);

            playerInput.SwitchCurrentActionMap("Player");

            PlayerMovementComponent movementComponent = playerCharacter.GetComponent<PlayerMovementComponent>();
            if (movementComponent != null)
                movementComponent.SetPlayerInput(playerInput);

            PlayerAttackScript attackComponent = playerCharacter.GetComponent<PlayerAttackScript>();
            if (attackComponent != null)
                attackComponent.SetPlayerInput(playerInput);

            playerCharacter.SetActive(true);

            playersUI[playerIndex].SetActive(true);

            PlayerHealthComponent pHC = playerCharacter.GetComponent<PlayerHealthComponent>();
            if (pHC != null)
            {
                pHC.SetHealthBarUI(backHealthBar[playerIndex], frontHealthBar[playerIndex]);
                playersHealthComponents.Add(pHC);
            }

            PlayerPauseMenuComponent pPMC = playerCharacter.GetComponent<PlayerPauseMenuComponent>();
            if (pPMC != null)
            {
                pPMC.SetPlayerInput(playerInput);
                playersPauseMenuComponents.Add(pPMC);
            }

            UltimateAbilityComponent uAC = playerCharacter.GetComponent<UltimateAbilityComponent>();
            uAC.SetUltBarUI(ultBar[playerIndex]);

            cinemachineTargetGroup.AddMember(playerCharacter.GetComponentInChildren<headBodyPart>().transform, 1f, 1f);

            switch (playerNumber)
            {
                case 1: playerCharacter.GetComponentInChildren<Image>().color = Color.blue; break;
                case 2: playerCharacter.GetComponentInChildren<Image>().color = Color.red; break;
                case 3: playerCharacter.GetComponentInChildren<Image>().color = Color.green; break;
                case 4: playerCharacter.GetComponentInChildren<Image>().color = Color.yellow; break;
            }
        }
    }

    (Vector3, Quaternion) GetSpawnData(int playerIndex, int totalPlayers)
    {
        Quaternion rotationLeft = Quaternion.Euler(0f, -90f, 0f);
        Quaternion rotationRight = Quaternion.Euler(0f, 90f, 0f);

        switch (totalPlayers)
        {
            case 1:
                return (spawnPoints[0].position, rotationRight);
            case 2:
                if (playerIndex == 0) return (spawnPoints[0].position, rotationRight);
                if (playerIndex == 1) return (spawnPoints[4].position, rotationLeft);
                break;
            case 3:
                if (playerIndex == 0) return (spawnPoints[0].position, rotationRight);
                if (playerIndex == 1) return (spawnPoints[2].position, rotationLeft);
                if (playerIndex == 2) return (spawnPoints[4].position, rotationLeft);
                break;
            case 4:
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
                playerAlive++;
            else
                cinemachineTargetGroup.RemoveMember(player.GetComponentInChildren<headBodyPart>().transform);
        }

        return playerAlive;
    }

    (bool, int) CheckIfPlayerPaused()
    {
        int index = 0;
        foreach (PlayerPauseMenuComponent player in playersPauseMenuComponents)
        {
            if (player.isPaused)
                return (true, index);
            index++;
        }
        return (false, 0);
    }
    private IEnumerator SelectGameOverButton()
    {
        yield return null; // attendre 1 frame pour que l'UI se mette à jour

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameOver.gameObject);
    }

    void GameOver()
    {
        yield return new WaitForSecondsRealtime(1f); // Petit délai 

        foreach (PlayerMovementComponent pmc in playerMovementComponents)
        {
            pmc.StopMovement();
        }

        isGameOver = true;
        afterGameLocal.enabled = true;
        Time.timeScale = 0f;

        StartCoroutine(SelectGameOverButton());
    }

}
