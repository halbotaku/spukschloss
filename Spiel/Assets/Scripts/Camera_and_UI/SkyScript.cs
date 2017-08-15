using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyScript : MonoBehaviour
{
    //Colors needed for changing
    public Color backgroundNight = new Color(0.2f, 0.2f, 0.7f, 1f);
    public Color backgroundDay = new Color(0.7f, 0.8f, 1f, 1f);

    //reference the length of the level
    public GameObject clock;
    private float levelLength;

    //GameObject Array managing the clouds and stars
    GameObject[] clouds;
    GameObject[] stars;

    //reference the house background
    public GameObject background;

    void Awake()
    {
        //reference the chosen level Length
        levelLength = clock.GetComponent<TowerClock>().levelLength * 60;

        GameObject sunMoon = GameObject.FindGameObjectWithTag("sunmoon");

        float speed = -levelLength/190;

        sunMoon.GetComponent<SunMoonSpawn>().speed = speed;
    }

    void Start()
    {
        //get all the clouds
        clouds = GameObject.FindGameObjectsWithTag("cloud");

        //get all the clouds
        stars = GameObject.FindGameObjectsWithTag("star");

        //make the clouds transparent towards the night
        foreach (GameObject star in stars)
        {
            star.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
    }

    void Update()
    {

        //calculate how much time has passed
        float variable = (levelLength - clock.GetComponent<TowerClock>().counter) / levelLength;

        //slowly fade to night
        GetComponent<Camera>().backgroundColor = Color.Lerp(backgroundDay, backgroundNight, variable);
        background.GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 1f, 1f), new Color(0.7f, 0.7f, 0.7f, 1f), variable);

        if (variable < 0.8f)
        {
            //make the clouds transparent towards the night
            foreach (GameObject cloud in clouds)
            {
                cloud.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1 - variable - 0.1f);
            }
        }

        if (variable > 0.5)
        {
            //make the clouds transparent towards the night
            foreach (GameObject star in stars)
            {
                star.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, variable-0.5f);
            }
        }
    }
}