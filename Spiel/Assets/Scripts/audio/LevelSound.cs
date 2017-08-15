using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSound : MonoBehaviour {

    public GameObject pauseMenu;
    private pauseMenu pauseSkript;

    private GameObject musicManager;
    private Singleton music;
    private AudioSource playing;

    public GameObject towerClock;
    private TowerClock clock;


    // Use this for initialization
    void Start()
    {
        //reference the sound Manager
        musicManager = GameObject.FindGameObjectWithTag("MusicManager");
        music = musicManager.GetComponent<Singleton>();
        playing = musicManager.GetComponent<AudioSource>();

        //if not already playing the level theme
        if (music.level != playing.clip)
        {
            playing.clip = music.level;
            playing.Play();
        }

        //reference the pausemenu script
        pauseSkript = pauseMenu.GetComponent<pauseMenu>();

        //reference the towerClock script
        clock = towerClock.GetComponent<TowerClock>();
    }

    void Update() {

        if (pauseSkript.paused == true)
        {
            //if the pause music is not playing yet
            if (music.pause != playing.clip)
            {
                playing.clip = music.pause;
                playing.loop = true;
                playing.Play();
            }
        }
        else if (clock.timeOut == true)
        {
            if (music.timeout != playing.clip)
            {
                playing.clip = music.timeout;
                playing.loop=false;
                playing.Play();
            }
        }
        else
        {
            //if the level music is not playing yet
            if (music.level != playing.clip)
            {
                playing.clip = music.level;
                playing.loop = true;
                playing.Play();
            }
        }

    }
}
