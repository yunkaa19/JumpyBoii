using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Garden");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void AboutScene()
    {
        SceneManager.LoadSceneAsync("About");
    }

    public void ControlsScene()
    {
        SceneManager.LoadSceneAsync("Controls");
    }
    public void MainMenuScene()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
}