using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathFollower : MonoBehaviour
{

    //Create variable for remembering the current waypoint
    public int currentWaypoint = 2;

    //Create Arranger holding the paths (pathArranger.cs)
    public pathArranger arranger;

    //Create variable for holding WaypointOfArrival
    public string destination = "reception";

    //minimum Distance
    private float minDist = 0.1f;
    //Movementspeed
    public float speed;

    // variable to hold a reference to our SpriteRenderer component (Flipping the Sprite)
    private SpriteRenderer mySpriteRenderer;

    //boolean for controlling direction on path
    public bool directionReversed;

    //Array for saving the rooms & items in need of repair
    private string[] repairObjectList;
    private string[] roomList;

    //Animator for Controlling the hotelowner animations
    private Animator myAnimator;

    //RepairScript reference
    private RepairScript repairScript;

    //referencing the repairLists
    public List<string> roomRepairList;
    public List<string> objectRepairList;

    //bool handling repair
    public bool isRepairing;

    //bool handling the slip on the magic banana
    public bool isSlipping;


    private void Awake()
    {
        // get a reference to the SpriteRenderer component on this gameObject (Flipping the Sprite)
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // get a reference to the Animator for controlling the states of the animation
        myAnimator = GetComponentInChildren<Animator>();

        //set the current way to the one at the reception
        arranger.currentPath = 14;
        directionReversed = false;
        isRepairing = false;

        //reference the repairScript
        repairScript = GetComponent<RepairScript>();

        //reference the repairLists
        roomRepairList = repairScript.roomRepairList;
        objectRepairList = repairScript.objectRepairList;

        isSlipping = false;
    }



    // Update is called once per frame
    void Update()
    {
        //when there are no objects to repair
        if (objectRepairList.Count == 0)
        {
            //head for the reception
            destination = "reception";
        }
        else
        {
            //pick the first object out of the list and make it your destination
            destination = roomRepairList[0];
        }

        //Check the distance of the object to the next waypoint
        float dist = Vector3.Distance(gameObject.transform.position, arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position);

        getPath();

        //Check if minimum Distance is achieved, if so go to next waypoint
        if (dist > minDist)
        {
            //only start the play animation when you have stood before, otherweise avoid a reload
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("stand"))
            {
                myAnimator.Play("walk");
            }

            //Move the Object to next waypoint
            Move();
        }
        else
        {
            if (directionReversed == false)
            {
                //When hotelowner has not reached the end of the path yet
                if (currentWaypoint + 1 != arranger.paths[arranger.currentPath].transform.childCount)
                {
                    //if the hotelowner is not slipping right now
                    if (isSlipping == false)
                    {
                        //increase the counting value to move to the next waypoint
                        currentWaypoint++;
                    }
                }
            }
            //When direction is set to be reversed
            else
            {
                //When hotelowner has reached the end of path (beginning waypoint)
                if (currentWaypoint != 0)
                {
                    if (isSlipping == false)
                    {
                        //decrease the counting value to move to the previous waypoint
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


    public void getPath()
    {
        switch (destination)
        {
            //when he is to head for the left room 1 first floor
            case "RL1":
                checkRL1Path();
                break;

            //when he is to head for the left room 2 first floor
            case "RL2":
                checkRL2Path();
                break;

            //when he is to head for the right room 1 first floor
            case "RR1":
                checkRR1Path();
                break;

            //when he is to head for the right room 2 first floor
            case "RR2":
                checkRR2Path();
                break;

            //when he is to head for the reception
            case "reception":
                checkReceptionPath();
                break;

            //when he is to head for the pool
            case "pool":
                checkPoolPath();
                break;

            //when he is to head for the left room second floor
            case "RL":
                checkRLPath();
                break;

            //when he is to head for the center room second floor
            case "RC":
                checkRCPath();
                break;

            //when he is to head for the left room second floor
            case "RR":
                checkRRPath();
                break;

            //when he is to head for the left room second floor
            case "kitchen":
                checkKitchenPath();
                break;

            //when he is to head for the left room 1 third floor
            case "RTL1":
                checkRTL1Path();
                break;

            //when he is to head for the left room 2 third floor
            case "RTL2":
                checkRTL2Path();
                break;


            //when he is to head for the right room 1 third floor
            case "RTR1":
                checkRTR1Path();
                break;


            //when he is to head for the right room 2 third floor
            case "RTR2":
                checkRTR2Path();
                break;


            default:
                destination = "reception";
                break;
        }
    }

    private void checkReceptionPath() {

        //when he is at the reception already then he has got nothing to do besides awaiting guests
        if (arranger.currentPath == 14 && currentWaypoint == 2)
        {
            directionReversed = false;

            //start doing shit but dont scare me ok
            //Debug.Log("Ich habe nichts zu tun!");
        }

        //when he is at a room on the ground floor except for the reception
        if (arranger.currentPath == 1 && currentWaypoint == 1 || arranger.currentPath == 2 && currentWaypoint == 1 || arranger.currentPath == 3 && currentWaypoint == 1 || arranger.currentPath == 4 && currentWaypoint == 1)
        {
            //just reverse the direction of the path leading to the waypoint
            directionReversed = true;
        }

        //when he is at the ground floor center waypoint
        if (arranger.currentPath == 1 && currentWaypoint == 0 || arranger.currentPath == 2 && currentWaypoint == 0 || arranger.currentPath == 3 && currentWaypoint == 0 || arranger.currentPath == 4 && currentWaypoint == 0 
            || arranger.currentPath == 0 && currentWaypoint == 0 || arranger.currentPath == 14 && currentWaypoint == 0)
        {
            //go back to the waypoint at the reception first
            arranger.currentPath = 14;
            currentWaypoint = 0;
            directionReversed = false;
        }

        //when positioned at one of the upper floors go to the reception waypoint first
        checkUpperFloorPaths();

    }

    private void checkRL1Path()
    {
        //when he is at RL1 already
        if (arranger.currentPath == 1 && currentWaypoint == 1)
        {
            directionReversed = false;

            //start repairing, avoid reloading
            if (isRepairing == false)
            {
                repairScript.repair();
                isRepairing = true;
            }
        }

        //when he is at the room next door
        if (arranger.currentPath == 2 && currentWaypoint == 1)
        {
            //go to RL1 right away
            arranger.currentPath = 1;
            directionReversed = false;
        }

        //when he is at any other room of the ground floor
        if (arranger.currentPath == 14 && currentWaypoint == 2 || arranger.currentPath == 3 && currentWaypoint == 1 || arranger.currentPath == 4 && currentWaypoint == 1)
        {
            //go to back the waypoint first
            directionReversed = true;
        }

        //when he is at the ground floor center waypoint
        if (arranger.currentPath == 1 && currentWaypoint == 0 || arranger.currentPath == 2 && currentWaypoint == 0 || arranger.currentPath == 3 && currentWaypoint == 0 || arranger.currentPath == 4 && currentWaypoint == 0
            || arranger.currentPath == 0 && currentWaypoint == 0 || arranger.currentPath == 14 && currentWaypoint == 0)
        {
            //go to RL1
            arranger.currentPath = 1;
            currentWaypoint = 0;
            directionReversed = false;
        }

        //when positioned at one of the upper floors go to the reception waypoint first
        checkUpperFloorPaths();
    }

    private void checkRL2Path()
    {
        //when he is at RL1 already
        if (arranger.currentPath == 2 && currentWaypoint == 1)
        {
            directionReversed = false;

            //start repairing, avoid reloading
            if (isRepairing == false)
            {
                repairScript.repair();
                isRepairing = true;
            }
        }

        //when he is at the room next door
        if (arranger.currentPath == 1 && currentWaypoint == 1)
        {
            //go to RL1 right away
            arranger.currentPath = 2;
            directionReversed = false;
        }

        //when he is at any other room of the ground floor
        if (arranger.currentPath == 14 && currentWaypoint == 2 || arranger.currentPath == 3 && currentWaypoint == 1 || arranger.currentPath == 4 && currentWaypoint == 1)
        {
            //go to back the waypoint first
            directionReversed = true;
        }

        //when he is at the ground floor center waypoint
        if (arranger.currentPath == 1 && currentWaypoint == 0 || arranger.currentPath == 2 && currentWaypoint == 0 || arranger.currentPath == 3 && currentWaypoint == 0 || arranger.currentPath == 4 && currentWaypoint == 0
            || arranger.currentPath == 0 && currentWaypoint == 0 || arranger.currentPath == 14 && currentWaypoint == 0)
        {
            //go to RL2
            arranger.currentPath = 2;
            currentWaypoint = 0;
            directionReversed = false;
        }

        //when positioned at one of the upper floors go to the reception waypoint first
        checkUpperFloorPaths();
    }

    private void checkRR1Path()
    {
        //when he is at RR1 already
        if (arranger.currentPath == 3 && currentWaypoint == 1)
        {
            directionReversed = false;

            //start repairing, avoid reloading
            if (isRepairing == false)
            {
                repairScript.repair();
                isRepairing = true;
            }
        }

        //when he is at the room next door
        if (arranger.currentPath == 4 && currentWaypoint == 1)
        {
            //go to RL1 right away
            arranger.currentPath = 3;
            directionReversed = false;
        }

        //when he is at any other room of the ground floor
        if (arranger.currentPath == 14 && currentWaypoint == 2 || arranger.currentPath == 1 && currentWaypoint == 1 || arranger.currentPath == 2 && currentWaypoint == 1)
        {
            //go to back the waypoint first
            directionReversed = true;
        }

        //when he is at the ground floor center waypoint
        if (arranger.currentPath == 1 && currentWaypoint == 0 || arranger.currentPath == 2 && currentWaypoint == 0 || arranger.currentPath == 3 && currentWaypoint == 0 || arranger.currentPath == 4 && currentWaypoint == 0
            || arranger.currentPath == 0 && currentWaypoint == 0 || arranger.currentPath == 14 && currentWaypoint == 0)
        {
            //go to RR1
            arranger.currentPath = 3;
            currentWaypoint = 0;
            directionReversed = false;
        }

        //when positioned at one of the upper floors go to the reception waypoint first
        checkUpperFloorPaths();
    }

    private void checkRR2Path()
    {
        //when he is at RR1 already
        if (arranger.currentPath == 4 && currentWaypoint == 1)
        {
            directionReversed = false;

            //start repairing, avoid reloading
            if (isRepairing == false)
            {
                repairScript.repair();
                isRepairing = true;
            }
        }

        //when he is at the room next door
        if (arranger.currentPath == 3 && currentWaypoint == 1)
        {
            //go to RL1 right away
            arranger.currentPath = 4;
            directionReversed = false;
        }

        //when he is at any other room of the ground floor
        if (arranger.currentPath == 14 && currentWaypoint == 2 || arranger.currentPath == 1 && currentWaypoint == 1 || arranger.currentPath == 2 && currentWaypoint == 1)
        {
            //go to back the waypoint first
            directionReversed = true;
        }

        //when he is at the ground floor center waypoint
        if (arranger.currentPath == 1 && currentWaypoint == 0 || arranger.currentPath == 2 && currentWaypoint == 0 || arranger.currentPath == 3 && currentWaypoint == 0 || arranger.currentPath == 4 && currentWaypoint == 0
            || arranger.currentPath == 0 && currentWaypoint == 0 || arranger.currentPath == 14 && currentWaypoint == 0)
        {
            //go to RR1
            arranger.currentPath = 4;
            currentWaypoint = 0;
            directionReversed = false;
        }

        //when positioned at one of the upper floors go to the reception waypoint first
        checkUpperFloorPaths();
    }

    private void checkPoolPath()
    {
        //when he is at the pool already
        if (arranger.currentPath == 5 && currentWaypoint == 2)
        {
            directionReversed = false;

            //start repairing, avoid reloading
            if (isRepairing == false)
            {
                repairScript.repair();
                isRepairing = true;
            }
        }

        //when he is at any other room on the upper left side of the house
        if (arranger.currentPath == 6 && currentWaypoint == 2 || arranger.currentPath == 10 && currentWaypoint == 2 || arranger.currentPath == 11 && currentWaypoint == 2)
        {
            //go one step back
            directionReversed = true;
        }

            //when you are heading for the pool out of one of these rooms and reach the corridor
            if (arranger.currentPath == 6 && currentWaypoint == 0 && directionReversed == true|| arranger.currentPath == 10 && currentWaypoint == 0 && directionReversed == true|| 
                arranger.currentPath == 11 && currentWaypoint == 0 && directionReversed == true)
            {
                //switch to the path leading to the pool
                arranger.currentPath = 5;
                currentWaypoint = 1;
                directionReversed = false;
            }

        //when he is at any room on the upper right side of the house
        if (arranger.currentPath == 7 && currentWaypoint == 1 || arranger.currentPath == 8 && currentWaypoint == 2 || arranger.currentPath == 9 && currentWaypoint == 2 ||
            arranger.currentPath == 12 && currentWaypoint == 2 || arranger.currentPath == 13 && currentWaypoint == 2)
        {
            //go back to the center of the upper floor
            directionReversed = true;
        }

        //when positioned at the ground floor go to the upper floor center first
        checkGroundFloorPaths();

        //when he is at the center of the corridor
        if (arranger.currentPath == 7 && currentWaypoint == 0 || arranger.currentPath == 8 && currentWaypoint == 0 || arranger.currentPath == 9 && currentWaypoint == 0 ||
            arranger.currentPath == 12 && currentWaypoint == 0 || arranger.currentPath == 13 && currentWaypoint == 0 || arranger.currentPath == 0 && currentWaypoint == 1 ||
            arranger.currentPath == 6 && currentWaypoint == 0 || arranger.currentPath == 10 && currentWaypoint == 0 || arranger.currentPath == 11 && currentWaypoint == 0 ||
            arranger.currentPath == 5 && currentWaypoint == 0)
        {
            //go to the pool
            arranger.currentPath = 5;
            currentWaypoint = 0;
            directionReversed = false;
        }
    }

    private void checkRLPath()
    {
        //when he is at RL already
        if (arranger.currentPath == 6 && currentWaypoint == 2)
        {
            directionReversed = false;

            //start repairing, avoid reloading
            if (isRepairing == false)
            {
                repairScript.repair();
                isRepairing = true;
            }
        }

        //when he is at any other room on the upper left side of the house
        if (arranger.currentPath == 5 && currentWaypoint == 2 || arranger.currentPath == 10 && currentWaypoint == 2 || arranger.currentPath == 11 && currentWaypoint == 2)
        {
            //go one step back
            directionReversed = true;
        }

            //when you are heading for RL out of one of these rooms and reach the corridor
            if (arranger.currentPath == 5 && currentWaypoint == 0 && directionReversed == true || arranger.currentPath == 10 && currentWaypoint == 0 && directionReversed == true ||
                arranger.currentPath == 11 && currentWaypoint == 0 && directionReversed == true)
            {
                //switch to the path leading to the pool
                arranger.currentPath = 6;
                currentWaypoint = 1;
                directionReversed = false;
            }

        //when he is at any room on the upper right side of the house
        if (arranger.currentPath == 7 && currentWaypoint == 1 || arranger.currentPath == 8 && currentWaypoint == 2 || arranger.currentPath == 9 && currentWaypoint == 2 ||
            arranger.currentPath == 12 && currentWaypoint == 2 || arranger.currentPath == 13 && currentWaypoint == 2)
        {
            //go back to the center of the upper floor
            directionReversed = true;
        }

        //when positioned at the ground floor go to the upper floor center first
        checkGroundFloorPaths();

        //when he is at the center of the corridor
        if (arranger.currentPath == 7 && currentWaypoint == 0 || arranger.currentPath == 8 && currentWaypoint == 0 || arranger.currentPath == 9 && currentWaypoint == 0 ||
            arranger.currentPath == 12 && currentWaypoint == 0 || arranger.currentPath == 13 && currentWaypoint == 0 || arranger.currentPath == 0 && currentWaypoint == 1 ||
            arranger.currentPath == 6 && currentWaypoint == 0 || arranger.currentPath == 10 && currentWaypoint == 0 || arranger.currentPath == 11 && currentWaypoint == 0 ||
            arranger.currentPath == 5 && currentWaypoint == 0)
        {
            //go to the pool
            arranger.currentPath = 6;
            currentWaypoint = 0;
            directionReversed = false;
        }
    }

    private void checkRTL1Path() {
        //when he is at the RTL1 already
        if (arranger.currentPath == 10 && currentWaypoint == 2)
        {
            directionReversed = false;

            //start repairing, avoid reloading
            if (isRepairing == false)
            {
                repairScript.repair();
                isRepairing = true;
            }
        }

        //when he is at any other room on the upper left side of the house
        if (arranger.currentPath == 5 && currentWaypoint == 2 || arranger.currentPath == 6 && currentWaypoint == 2 || arranger.currentPath == 11 && currentWaypoint == 2)
        {
            //go one step back
            directionReversed = true;
        }

            //when you are heading for RL out of one of these rooms and reach the corridor
            if (arranger.currentPath == 5 && currentWaypoint == 0 && directionReversed == true || arranger.currentPath == 6 && currentWaypoint == 0 && directionReversed == true ||
                arranger.currentPath == 11 && currentWaypoint == 0 && directionReversed == true)
            {
                //switch to the path leading to the pool
                arranger.currentPath = 10;
                currentWaypoint = 1;
                directionReversed = false;
            }

        //when he is at any room on the upper right side of the house
        if (arranger.currentPath == 7 && currentWaypoint == 1 || arranger.currentPath == 8 && currentWaypoint == 2 || arranger.currentPath == 9 && currentWaypoint == 2 ||
            arranger.currentPath == 12 && currentWaypoint == 2 || arranger.currentPath == 13 && currentWaypoint == 2)
        {
            //go back to the center of the upper floor
            directionReversed = true;
        }

        //when positioned at the ground floor go to the upper floor center first
        checkGroundFloorPaths();

        //when he is at the center of the corridor
        if (arranger.currentPath == 7 && currentWaypoint == 0 || arranger.currentPath == 8 && currentWaypoint == 0 || arranger.currentPath == 9 && currentWaypoint == 0 ||
            arranger.currentPath == 12 && currentWaypoint == 0 || arranger.currentPath == 13 && currentWaypoint == 0 || arranger.currentPath == 0 && currentWaypoint == 1 ||
            arranger.currentPath == 6 && currentWaypoint == 0 || arranger.currentPath == 10 && currentWaypoint == 0 || arranger.currentPath == 11 && currentWaypoint == 0 ||
            arranger.currentPath == 5 && currentWaypoint == 0)
        {
            //go to the pool
            arranger.currentPath = 10;
            currentWaypoint = 0;
            directionReversed = false;
        }
    }

    private void checkRTL2Path()
    {
        //when he is at the RTL2 already
        if (arranger.currentPath == 11 && currentWaypoint == 2)
        {
            directionReversed = false;

            //start repairing, avoid reloading
            if (isRepairing == false)
            {
                repairScript.repair();
                isRepairing = true;
            }
        }

        //when he is at any other room on the upper left side of the house
        if (arranger.currentPath == 5 && currentWaypoint == 2 || arranger.currentPath == 6 && currentWaypoint == 2 || arranger.currentPath == 10 && currentWaypoint == 2)
        {
            //go one step back
            directionReversed = true;
        }

            //when you are heading for RL out of one of these rooms and reach the corridor
            if (arranger.currentPath == 5 && currentWaypoint == 0 && directionReversed == true || arranger.currentPath == 6 && currentWaypoint == 0 && directionReversed == true ||
                arranger.currentPath == 10 && currentWaypoint == 0 && directionReversed == true)
            {
                //switch to the path leading to the pool
                arranger.currentPath = 11;
                currentWaypoint = 1;
                directionReversed = false;
            }

        //when he is at any room on the upper right side of the house
        if (arranger.currentPath == 7 && currentWaypoint == 1 || arranger.currentPath == 8 && currentWaypoint == 2 || arranger.currentPath == 9 && currentWaypoint == 2 ||
            arranger.currentPath == 12 && currentWaypoint == 2 || arranger.currentPath == 13 && currentWaypoint == 2)
        {
            //go back to the center of the upper floor
            directionReversed = true;
        }

        //when positioned at the ground floor go to the upper floor center first
        checkGroundFloorPaths();

        //when he is at the center of the corridor
        if (arranger.currentPath == 7 && currentWaypoint == 0 || arranger.currentPath == 8 && currentWaypoint == 0 || arranger.currentPath == 9 && currentWaypoint == 0 ||
            arranger.currentPath == 12 && currentWaypoint == 0 || arranger.currentPath == 13 && currentWaypoint == 0 || arranger.currentPath == 0 && currentWaypoint == 1 ||
            arranger.currentPath == 6 && currentWaypoint == 0 || arranger.currentPath == 10 && currentWaypoint == 0 || arranger.currentPath == 11 && currentWaypoint == 0 ||
            arranger.currentPath == 5 && currentWaypoint == 0)
        {
            //go to the pool
            arranger.currentPath = 11;
            currentWaypoint = 0;
            directionReversed = false;
        }
    }

    private void checkRCPath()
    {
        //when he is at RC already
        if (arranger.currentPath == 7 && currentWaypoint == 1)
        {
            directionReversed = false;

            //start repairing, avoid reloading
            if (isRepairing == false)
            {
                repairScript.repair();
                isRepairing = true;
            }
        }

        //when he is at any other room on the upper right side of the house
        if (arranger.currentPath == 8 && currentWaypoint == 2 || arranger.currentPath == 9 && currentWaypoint == 2 || arranger.currentPath == 12 && currentWaypoint == 2 || arranger.currentPath == 13 && currentWaypoint == 2)
        {
            //go one step back
            directionReversed = true;
        }

            //when you are heading for the RC out of one of these rooms and reach the corridor
            if (arranger.currentPath == 8 && currentWaypoint == 0 && directionReversed == true || arranger.currentPath == 9 && currentWaypoint == 0 && directionReversed == true ||
                arranger.currentPath == 12 && currentWaypoint == 0 && directionReversed == true || arranger.currentPath == 13 && currentWaypoint == 0 && directionReversed == true)
            {
                //switch to the path leading to RL
                arranger.currentPath = 7;
                currentWaypoint = 0;
                directionReversed = false;
            }

        //when he is at any room on the upper left side of the house
        if (arranger.currentPath == 5 && currentWaypoint == 2 || arranger.currentPath == 6 && currentWaypoint == 2 || arranger.currentPath == 10 && currentWaypoint == 2 || arranger.currentPath == 11 && currentWaypoint == 2)
        {
            //go back to the center of the upper floor
            directionReversed = true;
        }

        //when positioned at the ground floor go to the upper floor center first
        checkGroundFloorPaths();

        //when he is at the center of the corridor
        if (arranger.currentPath == 7 && currentWaypoint == 0 || arranger.currentPath == 8 && currentWaypoint == 0 || arranger.currentPath == 9 && currentWaypoint == 0 ||
            arranger.currentPath == 12 && currentWaypoint == 0 || arranger.currentPath == 13 && currentWaypoint == 0 || arranger.currentPath == 0 && currentWaypoint == 1 ||
            arranger.currentPath == 6 && currentWaypoint == 0 || arranger.currentPath == 10 && currentWaypoint == 0 || arranger.currentPath == 11 && currentWaypoint == 0 ||
            arranger.currentPath == 5 && currentWaypoint == 0)
        {
            //go to RL
            arranger.currentPath = 7;
            currentWaypoint = 0;
            directionReversed = false;
        }
    }

    private void checkRRPath()
    {
        //when he is at RR already
        if (arranger.currentPath == 8 && currentWaypoint == 2)
        {
            directionReversed = false;

            //start repairing, avoid reloading
            if (isRepairing == false)
            {
                repairScript.repair();
                isRepairing = true;
            }
        }

        //when he is at any other room on the upper right side of the house
        if (arranger.currentPath == 7 && currentWaypoint == 1 || arranger.currentPath == 9 && currentWaypoint == 2 || arranger.currentPath == 12 && currentWaypoint == 2 || arranger.currentPath == 13 && currentWaypoint == 2)
        {
            //go one step back
            directionReversed = true;
        }

            //when you are heading for the RR out of one of these rooms and reach the corridor
            if (arranger.currentPath == 7 && currentWaypoint == 0 && directionReversed == true || arranger.currentPath == 9 && currentWaypoint == 0 && directionReversed == true ||
                arranger.currentPath == 12 && currentWaypoint == 0 && directionReversed == true || arranger.currentPath == 13 && currentWaypoint == 0 && directionReversed == true)
            {
                //switch to the path leading to RR
                arranger.currentPath = 8;
                currentWaypoint = 0;
                directionReversed = false;
            }

        //when he is at any room on the upper left side of the house
        if (arranger.currentPath == 5 && currentWaypoint == 2 || arranger.currentPath == 6 && currentWaypoint == 2 || arranger.currentPath == 10 && currentWaypoint == 2 || arranger.currentPath == 11 && currentWaypoint == 2)
        {
            //go back to the center of the upper floor
            directionReversed = true;
        }

        //when positioned at the ground floor go to the upper floor center first
        checkGroundFloorPaths();

        //when he is at the center of the corridor
        if (arranger.currentPath == 7 && currentWaypoint == 0 || arranger.currentPath == 8 && currentWaypoint == 0 || arranger.currentPath == 9 && currentWaypoint == 0 ||
            arranger.currentPath == 12 && currentWaypoint == 0 || arranger.currentPath == 13 && currentWaypoint == 0 || arranger.currentPath == 0 && currentWaypoint == 1 ||
            arranger.currentPath == 6 && currentWaypoint == 0 || arranger.currentPath == 10 && currentWaypoint == 0 || arranger.currentPath == 11 && currentWaypoint == 0 ||
            arranger.currentPath == 5 && currentWaypoint == 0)
        {
            //go to RR
            arranger.currentPath = 8;
            currentWaypoint = 0;
            directionReversed = false;
        }
    }

    private void checkKitchenPath()
    {
        //when he is at the kitchen already
        if (arranger.currentPath == 9 && currentWaypoint == 2)
        {
            directionReversed = false;

            //start repairing, avoid reloading
            if (isRepairing == false)
            {
                repairScript.repair();
                isRepairing = true;
            }
        }

        //when he is at any other room on the upper right side of the house
        if (arranger.currentPath == 7 && currentWaypoint == 1 || arranger.currentPath == 8 && currentWaypoint == 2 || arranger.currentPath == 12 && currentWaypoint == 2 || arranger.currentPath == 13 && currentWaypoint == 2)
        {
            //go one step back
            directionReversed = true;
        }

            //when you are heading for the kitchen out of one of these rooms and reach the corridor
            if (arranger.currentPath == 7 && currentWaypoint == 0 && directionReversed == true || arranger.currentPath == 8 && currentWaypoint == 0 && directionReversed == true ||
                arranger.currentPath == 12 && currentWaypoint == 0 && directionReversed == true || arranger.currentPath == 13 && currentWaypoint == 0 && directionReversed == true)
            {
                //switch to the path leading to the kitchen
                arranger.currentPath = 9;
                currentWaypoint = 0;
                directionReversed = false;
            }

        //when he is at any room on the upper left side of the house
        if (arranger.currentPath == 5 && currentWaypoint == 2 || arranger.currentPath == 6 && currentWaypoint == 2 || arranger.currentPath == 10 && currentWaypoint == 2 || arranger.currentPath == 11 && currentWaypoint == 2)
        {
            //go back to the center of the upper floor
            directionReversed = true;
        }

        //when positioned at the ground floor go to the upper floor center first
        checkGroundFloorPaths();

        //when he is at the center of the corridor
        if (arranger.currentPath == 7 && currentWaypoint == 0 || arranger.currentPath == 8 && currentWaypoint == 0 || arranger.currentPath == 9 && currentWaypoint == 0 ||
            arranger.currentPath == 12 && currentWaypoint == 0 || arranger.currentPath == 13 && currentWaypoint == 0 || arranger.currentPath == 0 && currentWaypoint == 1 ||
            arranger.currentPath == 6 && currentWaypoint == 0 || arranger.currentPath == 10 && currentWaypoint == 0 || arranger.currentPath == 11 && currentWaypoint == 0 ||
            arranger.currentPath == 5 && currentWaypoint == 0)
        {
            //go to RR
            arranger.currentPath = 9;
            currentWaypoint = 0;
            directionReversed = false;
        }
    }

    private void checkRTR1Path()
    {
        //when he is at RTR1 already
        if (arranger.currentPath == 12 && currentWaypoint == 2)
        {
            directionReversed = false;

            //start repairing, avoid reloading
            if (isRepairing == false)
            {
                repairScript.repair();
                isRepairing = true;
            }
        }

        //when he is at any other room on the upper right side of the house
        if (arranger.currentPath == 7 && currentWaypoint == 1 || arranger.currentPath == 8 && currentWaypoint == 2 || arranger.currentPath == 9 && currentWaypoint == 2 || arranger.currentPath == 13 && currentWaypoint == 2)
        {
            //go one step back
            directionReversed = true;
        }

            //when you are heading for the kitchen out of one of these rooms and reach the corridor
            if (arranger.currentPath == 7 && currentWaypoint == 0 && directionReversed == true || arranger.currentPath == 8 && currentWaypoint == 0 && directionReversed == true ||
                arranger.currentPath == 9 && currentWaypoint == 0 && directionReversed == true || arranger.currentPath == 13 && currentWaypoint == 0 && directionReversed == true)
            {
                //switch to the path leading to RTR1
                arranger.currentPath = 12;
                currentWaypoint = 0;
                directionReversed = false;
            }

        //when he is at any room on the upper left side of the house
        if (arranger.currentPath == 5 && currentWaypoint == 2 || arranger.currentPath == 6 && currentWaypoint == 2 || arranger.currentPath == 10 && currentWaypoint == 2 || arranger.currentPath == 11 && currentWaypoint == 2)
        {
            //go back to the center of the upper floor
            directionReversed = true;
        }

        //when positioned at the ground floor go to the upper floor center first
        checkGroundFloorPaths();

        //when he is at the center of the corridor
        if (arranger.currentPath == 7 && currentWaypoint == 0 || arranger.currentPath == 8 && currentWaypoint == 0 || arranger.currentPath == 9 && currentWaypoint == 0 ||
            arranger.currentPath == 12 && currentWaypoint == 0 || arranger.currentPath == 13 && currentWaypoint == 0 || arranger.currentPath == 0 && currentWaypoint == 1 ||
            arranger.currentPath == 6 && currentWaypoint == 0 || arranger.currentPath == 10 && currentWaypoint == 0 || arranger.currentPath == 11 && currentWaypoint == 0 ||
            arranger.currentPath == 5 && currentWaypoint == 0)
        {
            //go to RTR1
            arranger.currentPath = 12;
            currentWaypoint = 0;
            directionReversed = false;
        }
    }

    private void checkRTR2Path()
    {
        //when he is at RTR2 already
        if (arranger.currentPath == 13 && currentWaypoint == 2)
        {
            directionReversed = false;

            //start repairing, avoid reloading
            if (isRepairing == false)
            {
                repairScript.repair();
                isRepairing = true;
            }
        }

        //when he is at any other room on the upper right side of the house
        if (arranger.currentPath == 7 && currentWaypoint == 1 || arranger.currentPath == 8 && currentWaypoint == 2 || arranger.currentPath == 9 && currentWaypoint == 2 || arranger.currentPath == 12 && currentWaypoint == 2)
        {
            //go one step back
            directionReversed = true;
        }

            //when you are heading for the kitchen out of one of these rooms and reach the corridor
            if (arranger.currentPath == 7 && currentWaypoint == 0 && directionReversed == true || arranger.currentPath == 8 && currentWaypoint == 0 && directionReversed == true ||
                arranger.currentPath == 9 && currentWaypoint == 0 && directionReversed == true || arranger.currentPath == 12 && currentWaypoint == 0 && directionReversed == true)
            {
                //switch to the path leading to RTR2
                arranger.currentPath = 13;
                currentWaypoint = 0;
                directionReversed = false;
            }

        //when he is at any room on the upper left side of the house
        if (arranger.currentPath == 5 && currentWaypoint == 2 || arranger.currentPath == 6 && currentWaypoint == 2 || arranger.currentPath == 10 && currentWaypoint == 2 || arranger.currentPath == 11 && currentWaypoint == 2)
        {
            //go back to the center of the upper floor
            directionReversed = true;
        }

        //when positioned at the ground floor go to the upper floor center first
        checkGroundFloorPaths();

        //when he is at the center of the corridor
        if (arranger.currentPath == 7 && currentWaypoint == 0 || arranger.currentPath == 8 && currentWaypoint == 0 || arranger.currentPath == 9 && currentWaypoint == 0 ||
            arranger.currentPath == 12 && currentWaypoint == 0 || arranger.currentPath == 13 && currentWaypoint == 0 || arranger.currentPath == 0 && currentWaypoint == 1 ||
            arranger.currentPath == 6 && currentWaypoint == 0 || arranger.currentPath == 10 && currentWaypoint == 0 || arranger.currentPath == 11 && currentWaypoint == 0 ||
            arranger.currentPath == 5 && currentWaypoint == 0)
        {
            //go to RTR1
            arranger.currentPath = 13;
            currentWaypoint = 0;
            directionReversed = false;
        }
    }



    private void checkUpperFloorPaths()
    {
        //when he is at a room on the first or second upper floor
        if (arranger.currentPath == 5 && currentWaypoint == 2 || arranger.currentPath == 6 && currentWaypoint == 2 || arranger.currentPath == 7 && currentWaypoint == 2 || arranger.currentPath == 8 && currentWaypoint == 2 ||
            arranger.currentPath == 9 && currentWaypoint == 2 || arranger.currentPath == 10 && currentWaypoint == 2 || arranger.currentPath == 11 && currentWaypoint == 2 || arranger.currentPath == 12 && currentWaypoint == 2 ||
            arranger.currentPath == 13 && currentWaypoint == 2)
        {
            //go back to the center of the corridor first
            directionReversed = true;
        }

        //when he ist at the center of the corridor
        if (arranger.currentPath == 5 && currentWaypoint == 0 || arranger.currentPath == 6 && currentWaypoint == 0 || arranger.currentPath == 7 && currentWaypoint == 0 || arranger.currentPath == 8 && currentWaypoint == 0 ||
            arranger.currentPath == 9 && currentWaypoint == 0 || arranger.currentPath == 10 && currentWaypoint == 0 || arranger.currentPath == 11 && currentWaypoint == 0 || arranger.currentPath == 12 && currentWaypoint == 0 ||
            arranger.currentPath == 13 && currentWaypoint == 0 || arranger.currentPath == 0 && currentWaypoint == 1)
        {
            //go back to the waypoint at the reception first
            arranger.currentPath = 0;
            currentWaypoint = 1;
            directionReversed = true;
        }
    }

    private void checkGroundFloorPaths()
    {
        //when he is at any room of the ground floor
        if (arranger.currentPath == 1 && currentWaypoint == 1 || arranger.currentPath == 2 && currentWaypoint == 1 || arranger.currentPath == 3 && currentWaypoint == 1 ||
            arranger.currentPath == 4 && currentWaypoint == 1 || arranger.currentPath == 14 && currentWaypoint == 2)
        {
            //go back to the reception waypoint
            directionReversed = true;
        }

        //when he is at the ground floor center waypoint
        if (arranger.currentPath == 1 && currentWaypoint == 0 || arranger.currentPath == 2 && currentWaypoint == 0 || arranger.currentPath == 3 && currentWaypoint == 0 || arranger.currentPath == 4 && currentWaypoint == 0
            || arranger.currentPath == 0 && currentWaypoint == 0 || arranger.currentPath == 14 && currentWaypoint == 0)
        {
            //go to the center of the upper floor corridor
            arranger.currentPath = 0;
            currentWaypoint = 0;
            directionReversed = false;
        }
    }
}
