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
        Time.timeScale = 1f;
        try
        {
            SceneManager.LoadScene("selection");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading scene: " + e.Message);
        }
    }
    public void RestartFight()
    {
        Time.timeScale = 1f;
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
        Time.timeScale = 1f;
        try
        {
            SceneManager.LoadScene("MainMenu");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading scene: " + e.Message);
        }
    }
}