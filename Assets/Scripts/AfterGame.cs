using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SCRIPTS_MARC; // <-- IMPORTANT : Pour trouver PlayerInputHandler

public class AfterGame : MonoBehaviour
{
    [SerializeField]
    public Button reStart;
    public Button CharacSelect;
    public Button MainMenu;

    /// <summary>
    /// Libère les PlayerInputHandlers pour qu'ils persistent.
    /// </summary>
    private void UnparentPlayerHandlers()
    {
        // Trouve tous les handlers de joueur qui existent
        PlayerInputHandler[] handlers = FindObjectsOfType<PlayerInputHandler>();
        
        if (handlers.Length > 0)
        {
            Debug.Log($"Libération de {handlers.Length} PlayerInputHandlers...");
            foreach (var handler in handlers)
            {
                // On les détache de leur parent (le joueur)
                // pour qu'ils ne soient pas détruits avec la scène.
                handler.transform.SetParent(null); 
            }
        }
    }

    public void GoToCharacSelect()
    {
        try
        {
            UnparentPlayerHandlers(); // <-- AJOUTÉ
            UnityEngine.SceneManagement.SceneManager.LoadScene("selection");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading scene: " + e.Message);
        }
    }
    public void RestartFight()
    {
        try
        {
            UnparentPlayerHandlers(); // <-- AJOUTÉ
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading scene: " + e.Message);
        }
    }
    public void BackMEnu()
    {
        try
        {
            UnparentPlayerHandlers(); // <-- AJOUTÉ
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu 2");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading scene: " + e.Message);
        }
    }
}