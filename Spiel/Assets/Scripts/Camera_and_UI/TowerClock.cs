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
    public float counter;

    //getting the UI Window for the TimeOut
    public GameObject timeOutWindow;
    public Text txt;

    //counter for the scared-off hotel guests
    public int guestCounter;
    private float rotationSpeed;

    public bool timeOut; 

    private bool end = true;

    public void Awake()
    {
        counter = levelLength * 60;

        timeOut = false;
    }

    public void Update()
    {
        //calculate the necessary rotationSpeed out of the levelLength
        rotationSpeed = 360 / (levelLength * 60);

        //rotate the littleHand
        littleHand.transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);

        //rotate the Big Hand with 12 times the speed
        bigHand.transform.Rotate(0, 0, -rotationSpeed * 12 * Time.deltaTime);

        if (counter < 1)
        {
            timeOut = true;
        }
        if (counter > 0)
        {
            //decrease the counter
            counter -= Time.deltaTime;
        }
        
        if (counter < 0 || guestCounter >= 13)
        {
            if (end)
            {
                if (guestCounter >= 13)
                {
                    timeOut = true;
                }

                end = false;
                Text text = txt.GetComponent<Text>();
                text.text += guestCounter + " von 13 Gästen haben das Hotel verlassen. ";
                if (guestCounter < 13)
                {
                    text.text += "Das wird nicht reichen, um die Menschen ein für alle Mal zu vertreiben!";
                }

                if (guestCounter == 13)
                {
                    text.text += "Gut gemacht! Das wird den Menschen eine Lehre sein!";
                }

                timeOutWindow.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}