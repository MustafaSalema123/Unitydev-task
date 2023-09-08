
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void ExitApplication()
    {
        Application.Quit();
    }
}
