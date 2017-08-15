using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //reference the sound Manager
        GameObject musicManager = GameObject.FindGameObjectWithTag("MusicManager");
        Singleton music = musicManager.GetComponent<Singleton>();
        AudioSource playing = musicManager.GetComponent<AudioSource>();

        //if not already playing the main menu theme
        if (music.mainMenu != playing.clip)
        {
            playing.clip = music.mainMenu;
            playing.loop = true;
            playing.Play();
        }		
	}

}
