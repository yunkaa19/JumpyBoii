using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject HUD;
    [Header("Pause Menu")]
    private bool isPaused = false;
    public GameObject pauseMenu;
    public Volume pauseVolume;
    
        private void Awake()
        {
            // Implement Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            pauseMenu.SetActive(false);
            pauseVolume.weight = 0;
            LockCursor();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        #region Pause Menu
        void PauseGame()
        {
            Time.timeScale = 0;
            isPaused = true;
            pauseMenu.SetActive(true);
            pauseVolume.weight = 1;
            UnlockCursor();
            HUD.SetActive(false);
        }
        
        public void ResumeGame()
        {
            Time.timeScale = 1;
            isPaused = false;
            pauseMenu.SetActive(false);
            pauseVolume.weight = 0;
            LockCursor();
            HUD.SetActive(true);
        }
        
        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        public bool IsGamePaused()
        {
            return isPaused;
        }
    
        
        
        public void MainMenu()
        {
            Time.timeScale = 1;
            isPaused = false;
            pauseMenu.SetActive(false);
            pauseVolume.weight = 0;
            UnlockCursor();
            HUD.SetActive(true);
            SceneManager.LoadSceneAsync("Main Menu");
        }
        
        #endregion

      
}

