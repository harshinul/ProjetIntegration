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

    public void GoToCharacSelect()
    {
        try
        {
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
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading scene: " + e.Message);
        }
    }
}