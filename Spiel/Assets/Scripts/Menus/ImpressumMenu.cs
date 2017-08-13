using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImpressumMenu : MonoBehaviour
{

    public GameObject exitWindow;

    public Button hauptmenue;
    public Button beenden;
    public Button ja;
    public Button nein;

    void Start()
    {
        //reference the buttons
        hauptmenue = hauptmenue.GetComponent<Button>();
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

        hauptmenue.enabled = false;
        beenden.enabled = false;
    }

    public void noPress()
    {
        exitWindow.SetActive(false);

        hauptmenue.enabled = true;
        beenden.enabled = true;
    }

    public void yesPress()
    {
        Application.Quit();
    }

    public void returnMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
