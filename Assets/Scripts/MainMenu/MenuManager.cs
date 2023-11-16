using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public AudioSource clickSound;
    
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Garden");
        clickSound.Play();
    }
    
    public void QuitGame()
    {
        Application.Quit();
        clickSound.Play();
    }
    
    public void AboutScene()
    {
        SceneManager.LoadSceneAsync("About");
        clickSound.Play();
    }

    public void ControlsScene()
    {
        SceneManager.LoadSceneAsync("Controls");
        clickSound.Play();
    }
    public void MainMenuScene()
    {
        SceneManager.LoadSceneAsync("Main Menu");
        clickSound.Play();
    }
}