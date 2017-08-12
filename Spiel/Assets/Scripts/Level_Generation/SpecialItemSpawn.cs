using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItemSpawn : MonoBehaviour {

    //variables determining the possible time of spawning the special items
    public float firstPossibleSpawn;
    public float lastPossibleSpawn;
    public float minimumSpawnTimeDifference;
    public int spawnProbility;

    //referencing the prefabs
    public GameObject screamPrefab;
    public GameObject slimePrefab;
    public GameObject bananaPrefab;

    //private variable for generating random numbers
    System.Random rnd = new System.Random();

    //referencing the level countdown time
    private float levelCountdown;
    private float levelLength;
    private GameObject towerClock;
    private bool isCooling;
    private float coolingPeriod;

    // Use this for initialization
    void Start () {
        //reference the current level countdown 
        towerClock = GameObject.Find("towerClock");
        TowerClock clock = towerClock.GetComponent<TowerClock>();
        levelCountdown = clock.counter;
        levelLength = clock.counter;
        coolingPeriod = minimumSpawnTimeDifference;
    }

    void Update()
    {
        //see how much time of the level is left
        TowerClock clock = towerClock.GetComponent<TowerClock>();
        levelCountdown = clock.counter;


        //for as long as the time is within the allowed range
        if (levelCountdown < levelLength - firstPossibleSpawn && levelCountdown > lastPossibleSpawn)
        {
            if (coolingPeriod > 0)
            {
                coolingPeriod = coolingPeriod - Time.deltaTime;
            }

            if (coolingPeriod < 0)
            {
                int random = rnd.Next(1, spawnProbility+1);

                if (random == 1)
                {
                    Debug.Log("Spawn Special");

                    int prefabNumber = rnd.Next(1, 4);
                    GameObject prefab = screamPrefab;

                    switch (prefabNumber)
                    {
                        case 1:
                            prefab = screamPrefab;
                            break;
                        case 2:
                            prefab = slimePrefab;
                            break;
                        case 3:
                            prefab = bananaPrefab;
                            break;
                    }

                    int firstxDigit = rnd.Next(-8, 9);
                    int secondxDigit = rnd.Next(0, 10);

                    string xString = firstxDigit + "." + secondxDigit;
                    float xpos = float.Parse(xString);

                    int firstyDigit = rnd.Next(-4, 5);
                    int secondyDigit = rnd.Next(0, 10);

                    string yString = firstyDigit + "." + secondyDigit;
                    float ypos = float.Parse(yString);

                    GameObject go = (GameObject)Instantiate(prefab);
                    go.transform.position = new Vector2(xpos, ypos);
                }

                coolingPeriod = minimumSpawnTimeDifference;
            }
        }
    }
}
