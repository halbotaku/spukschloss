using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathFollowerGuest : MonoBehaviour {

    //Create variable for remembering the current waypoint
    public int currentWaypoint = 0;

    //Create Arranger holding the paths (pathArranger.cs)
    public pathArranger arranger;

    //Create variable for holding WaypointOfArrival
    public string destination = "idlePosition";

    //minimum Distance
    private float minDist = 0.1f;

    //Movementspeed
    public float speed;
    public float idleSpeed;
    private float tempSpeed;

    //Create boolean for start/stop of Movement Sections
    public bool idleInRoom = true;
    public bool waitForRepair = false;
    public bool go = false;
    public bool goingBack = false;
    public bool bitchImOut = false;


    // variable to hold a reference to our SpriteRenderer component (Flipping the Sprite)
    private SpriteRenderer mySpriteRenderer;

    //boolean for controlling direction on path
    public bool directionReversed;

    //time in seconds of waiting on spot while idling
    public float idlingTime;
    private float idleCountdown;


    // This function is called just one time by Unity the moment the game loads
    private void Awake()
    {
        // get a reference to the SpriteRenderer component on this gameObject (Flipping the Sprite)
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        //save the speed value into a variable for switching between idling and going speed
        tempSpeed = speed;

        //Set idling time for counting down
        idleCountdown = idlingTime;
    }

    // Update is called once per frame
    void Update()
    {

        //Check the distance of the object to the next waypoint
        float dist = Vector3.Distance(gameObject.transform.position, arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position);

        //Check if Object is set to Idling
        if (idleInRoom)
        {
            speed = idleSpeed;
            //Check if minimum Distance is achieved, if so switch to next waypoint
            if (dist > minDist)
            {
                //Move the Object to next waypoint
                Move();
            }
            else
            {
                if (directionReversed == false)
                {

                    //When hotelguest has reached the side of the room
                    if (currentWaypoint + 1 == arranger.paths[arranger.currentPath].transform.childCount)
                    {

                        if (idleCountdown > 0)
                        {
                            idleCountdown = idleCountdown - Time.deltaTime;
                        }
                        else
                        {
                            //stop movement for a few seconds before returning to other side of the room
                            directionReversed = true;
                            idleCountdown = idlingTime;
                        }
                    }
                    else
                    {
                        //increase the counting value to move to the next waypoint
                        currentWaypoint++;
                    }
                }
                else
                {
                    if (currentWaypoint < arranger.paths[arranger.currentPath].transform.childCount &&
                        currentWaypoint > 0)
                    {
                        //decrease the counting value to move to the previous waypoint (backwards)
                        currentWaypoint--;
                    }
                    else
                    {
                        if (idleCountdown > 0)
                        {
                            idleCountdown = idleCountdown - Time.deltaTime;
                        }
                        else
                        {
                            //stop movement for a few seconds before returning to other side of the room
                            directionReversed = false;
                            idleCountdown = idlingTime;
                        }
                    }
                }
            }
        }
    }


    public void Move()
    {
        //Get the difference of the current position to the next waypoint
        Vector3 difference = arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position - transform.position;

        //calculate the normal vector
        difference.Normalize();

        //move the object from current to next waypoint
        gameObject.transform.position += difference * speed * Time.deltaTime;

        // if the variable isn't empty (we have a reference to our SpriteRenderer)
        if (mySpriteRenderer != null)
        {
            //When movement is going to the left
            if (difference.x < 0)
            {
                // flip the sprite
                mySpriteRenderer.flipX = true;
            }
            else if (difference.x > 0)
            {
                // revert the sprite to normal
                mySpriteRenderer.flipX = false;
            }
        }

    }
}
