using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour {

    public GameObject pauseUI;

    private bool paused = false;

    public GameObject exitWindow;
    public GameObject timeOutWindow;

    public Button fortsetzen;
    public Button levelNeustart;
    public Button hauptMenu;
    public Button beenden;
    public Button ja;
    public Button nein;

    public Button gameFinishedHauptmenu;
    public Button gameFinishedRestart;
    public Button gameFinishedBeen;

    void Start() {
        pauseUI.SetActive(false);

        exitWindow.SetActive(false);

        timeOutWindow.SetActive(false);

        fortsetzen = fortsetzen.GetComponent<Button>(); ;
        levelNeustart = levelNeustart.GetComponent<Button>();
        hauptMenu = hauptMenu.GetComponent<Button>(); ;
        beenden = beenden.GetComponent<Button>(); ;
        ja = ja.GetComponent<Button>(); ;
        nein = nein.GetComponent<Button>();
        gameFinishedHauptmenu= gameFinishedHauptmenu.GetComponent<Button>();
        gameFinishedRestart = gameFinishedRestart.GetComponent<Button>();
        gameFinishedBeen = gameFinishedBeen.GetComponent<Button>();
    
    }

    void Update() {
        if (Input.GetButtonDown("Pause"))
        {
            paused = !paused;
        }

        if (paused)
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void continueGame()
    {
        paused = !paused;
    }

    public void restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void mainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void exitPress()
    {
        exitWindow.SetActive(true);

        fortsetzen.enabled = false;
        levelNeustart.enabled = false;
        hauptMenu.enabled = false;
        beenden.enabled = false;
        gameFinishedHauptmenu.enabled = false;
        gameFinishedRestart.enabled = false;
        gameFinishedBeen.enabled = false;
    }

    public void noPress()
    {
        exitWindow.SetActive(false);

        fortsetzen.enabled = true;
        levelNeustart.enabled = true;
        hauptMenu.enabled = true;
        beenden.enabled = true;
        gameFinishedHauptmenu.enabled = true;
        gameFinishedRestart.enabled = true;
        gameFinishedBeen.enabled = true;
    }

    public void yesPress()
    {
        Application.Quit();
    }
}