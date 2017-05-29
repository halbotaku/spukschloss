using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathFollowerGuest : MonoBehaviour {

    //Create variable for remembering the current waypoint
    public int currentWaypoint = 0;

    //Create Arranger holding the paths (pathArranger.cs)
    public pathArranger arranger;

    //Create variable for remembering the assigned room
    public string room;

    //minimum Distance
    private float minDist = 0.1f;

    //Movementspeed
    public float speed;
    public float idleSpeed;
    private float tempSpeed;

    //Create boolean for start/stop of Movement Sections
    public bool idleInRoom = true;
    public bool goingToReception = false;

    // variable to hold a reference to our SpriteRenderer component (Flipping the Sprite)
    private SpriteRenderer mySpriteRenderer;

    //boolean for controlling direction on path
    public bool directionReversed;

    //time in seconds of waiting on spot while idling
    public float idlingTime;
    private float idleCountdown;

    //time in seconds of waiting before leaving to reception / leaving the hotel
    private float waitUntilReception;
    private float waitUntilLeave;


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

        /*
         * Setting the speed dependant on movement mode
         */
        if (idleInRoom)
        {
            //set the speed of the hotel guest to the idle Speed
            if (speed != idleSpeed)
            {
                tempSpeed = speed;
                speed = idleSpeed;
            }
        }

        //Check if minimum Distance is achieved, if so go to next waypoint
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
                    if (idleInRoom)
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
                    else if (goingToReception)
                    {
                        //TO DO
                        //Hotel guest waits at reception
                        letGuestCheckOut();
                    }
                }
                //When hotel guest has not reached the end yet
                else
                {
                        if (goingToReception)
                        {
                            //increase the counting value to move to the next waypoint
                            currentWaypoint++;
                        }

                    if (idleInRoom)
                    {
                        //increase the counting value to move to the next waypoint
                        currentWaypoint++;
                    }
                }
            }
            //When direction is set to be reversed
            else
            {
                //When hotelguest has reached the side of the room/end of path (beginning waypoint)
                if (currentWaypoint == 0)
                {
                    if (idleInRoom)
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
                    else if (goingToReception)
                    {
                        idleInRoom = false;
                        directionReversed = false;
                    }
                }
                //When hotel guest has not reached the beginning yet
                else
                {
                    if (goingToReception)
                    {
                        //decrease the counting value to move to the previous waypoint
                        currentWaypoint--;
                    }

                    if (idleInRoom)
                    {
                        //decrease the counting value to move to the next waypoint
                        currentWaypoint--;
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

    public void letGuestReact(GameObject interactionObject)
    {
            //stop idling
            idleInRoom = false;

            //hotel guest reacts to damage
            Debug.Log("Uuuuuaaaaaarghhhh!");

            //call the time the guest is willing to wait until repair
            InteractionList list = interactionObject.GetComponent<InteractionList>();
            waitUntilReception = list.inRoomWaitingList[list.index];
            waitUntilLeave = list.atReceptionWaitingList[list.index];

            //let the guest wait in the room for the mentioned time being
            StartCoroutine(waitForRepair());
    }

    public void letGuestCheckOut()
    {
        //hotel guest reports damage at reception
        Debug.Log("Das geht so nicht!");

        //let the guest wait in the at the reception for the mentioned time being
        StartCoroutine(waitForRepairReception());

        Debug.Log("Ich muss mich beschweren!");
    }

    IEnumerator waitForRepair()
    {
        //wait for the according time
        yield return new WaitForSeconds(waitUntilReception);

        //check whether damage has been repaired
        //TO DO

        //otherwise go to the reception
        speed = tempSpeed;
        Debug.Log("Ich gehe mich beschweren.");

        //pick the way leading to the reception
        currentWaypoint = 0;
        arranger.currentPath = 1;
        directionReversed = false;
        goingToReception = true;
        if (speed != idleSpeed)
        {
        speed = tempSpeed;
        }

    }

    IEnumerator waitForRepairReception()
    {
        //wait for the according time
        yield return new WaitForSeconds(waitUntilLeave);

        //check whether damage has been repaired
        //TO DO

        //pick the way leading out of the hotel
        currentWaypoint = 0;
        arranger.currentPath = 2;
        directionReversed = false;
        goingToReception = true;
        idleInRoom = false;

        //otherwise leave the hotel
        Debug.Log("SO EIN SCHLECHTER SERVICE!");
    }
}
