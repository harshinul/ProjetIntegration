using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AfterGame : MonoBehaviour
{
    [SerializeField]
    public Button reStart;
    public Button CharacSelect;
    public Button MainMenu;
    void Start()
    {
      
    }
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
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu 2");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading scene: " + e.Message);
        }
    }
}
