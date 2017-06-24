using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour {

    public GameObject exitWindow;

    public Button start;
    public Button impressum;
    public Button optionen;
    public Button beenden;
    public Button ja;
    public Button nein;

    void Start()
    {
        //reference the buttons
        start = start.GetComponent<Button>();
        impressum = impressum.GetComponent<Button>();
        optionen = optionen.GetComponent<Button>();
        beenden = beenden.GetComponent<Button>();

        //reference the yes/no buttons for exiting the game
        ja = ja.GetComponent<Button>();
        nein = nein.GetComponent<Button>();

        //turn off the exit Window
        exitWindow.SetActive(false);
    }

    public void exitPress()
    {
        exitWindow.SetActive(true);

        start.enabled = false;
        impressum.enabled = false;
        optionen.enabled = false;
        beenden.enabled = false;
    }

    public void noPress()
    {
        exitWindow.SetActive(false);

        start.enabled = true;
        impressum.enabled = true;
        optionen.enabled = true;
        beenden.enabled = true;
    }

    public void yesPress()
    {
        Application.Quit();
    }

    public void startLevel()
    {
        SceneManager.LoadScene(1);
    }
}
