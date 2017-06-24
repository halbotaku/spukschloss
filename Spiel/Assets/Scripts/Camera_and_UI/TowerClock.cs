using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerClock : MonoBehaviour {

    //the hands of the clockTower
    public GameObject bigHand;
    public GameObject littleHand;

    //duration of the Countdown in minutes
    public float levelLength;
    private float counter;

    //getting the UI Window for the TimeOut
    public GameObject timeOutWindow;
    public Text txt;

    //counter for the scared-off hotel guests
    private int guestCounter;
    private float rotationSpeed;

    public void Start()
    {
        counter = levelLength * 6;
    }

    public void Update()
    {
        //calculate the necessary rotationSpeed out of the levelLength
        rotationSpeed = 360 / (levelLength * 60);

        //rotate the littleHand
        littleHand.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);

        //rotate the Big Hand with 12 times the speed
        bigHand.transform.Rotate(0, 0, -rotationSpeed * 12 * Time.deltaTime);

        if (counter > 0)
        {
            //decrease the counter
            counter -= Time.deltaTime;
            Debug.Log(counter);
        }
        else
        {
            Text text = txt.GetComponent<Text>();
            text.text = "Time Out! Anzahl der verjagten Gäste: " + guestCounter;
            timeOutWindow.SetActive(true);
            Time.timeScale = 0;
        }
    }
}