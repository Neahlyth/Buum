using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUi;
    public GameObject lifeBar;

    void Update()
    {
        //Si la touche echap est appuy�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //si le jeu est en pause
            if (gameIsPaused)
            {
                //on peut relancer
                Resume();
            }else
            {
                //on met en pause
                Pause();
            }
        }
    }

     public void Resume()
    {
        //on desactive le pauseMenuUi
        pauseMenuUi.SetActive(false);
        lifeBar.SetActive(true);
        //TimeOut compris entre 0 et 1, 1=temps de base
        Time.timeScale = 1f;
        gameIsPaused = false;
    }


     public void Pause()
    {
        //pour activer le pauseMenuUi
        pauseMenuUi.SetActive(true);
        lifeBar.SetActive(false);
        //on frise le temps
        Time.timeScale = 0f;
        //on est en pause
        gameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
