using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathFollower : MonoBehaviour {

    //Create variable for remembering the current waypoint
    public int currentWaypoint = 0;

    //Create Arranger holding the paths (pathArranger.cs)
    public pathArranger arranger;

    //Create variable for holding WaypointOfArrival
    public string destination = "reception";

    //minimum Distance
    private float minDist = 0.1f;
    //Movementspeed
    public float speed;

    //Create boolean for start/stop of Movement
    public bool go = true;

    // variable to hold a reference to our SpriteRenderer component (Flipping the Sprite)
    private SpriteRenderer mySpriteRenderer;

    //boolean for controlling direction on path
    public bool directionReversed;

    //Array for saving the rooms & items in need of repair
    private string[] repairObjectList;
    private string[] roomList;

    
    
    
    // This function is called just one time by Unity the moment the game loads
    private void Awake()
    {
        // get a reference to the SpriteRenderer component on this gameObject (Flipping the Sprite)
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
	
	
    
    // Update is called once per frame
	void Update () {

        //Check the distance of the object to the next waypoint
        float dist = Vector3.Distance(gameObject.transform.position, arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position);

        //Check if Object is set to Movement
        if (go)
        {
            //check your destination
            getPath();

            //Check if minimum Distance is achieved, if so switch to next waypoint
            if (dist > minDist)
            {
                //Move the Object to next waypoint
                Move();
            }
            else
            {           if (directionReversed == false)
                        {

                            if (currentWaypoint + 1 == arranger.paths[arranger.currentPath].transform.childCount)
                            {
                                //TO DO
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
                                    //TO DO
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

                //function for checking whether player is at ground floor center already, if so choose the path leading to the room
                receptionCheckFirst(1);

                //function for checking if player is at center, then move to ground floor center
                centerCheckFirst();

                //function for calling upper floor room positions to center first
                upperFloorToCenterFirst();

                //function for calling first floor room positions to ground floor center first, except for current destination
                firstFloorToReceptionFirst(1, 1);
                break;

            //when he is to head for the left room 2 first floor
            case "RL2":

                receptionCheckFirst(2);

                centerCheckFirst();

                upperFloorToCenterFirst();

                firstFloorToReceptionFirst(2, 1);
                break;


            //when he is to head for the right room 1 first floor
            case "RR1":

                receptionCheckFirst(3);

                centerCheckFirst();

                upperFloorToCenterFirst();

                firstFloorToReceptionFirst(3, 1);
                break;


            //when he is to head for the right room 2 first floor
            case "RR2":

                receptionCheckFirst(4);

                centerCheckFirst();

                upperFloorToCenterFirst();

                firstFloorToReceptionFirst(4, 1);
                break;


            //when he is to head for the reception
            case "reception":

                receptionCheckFirst(14);

                centerCheckFirst();

                upperFloorToCenterFirst();

                firstFloorToReceptionFirst(14, 1);
                break;


            //when he is to head for the pool
            case "pool":

                //function to check if figure is already at center upper floors, if so move to the destinaton
                upperCenterCheckFirst(5);

                //function to check if figure is at first floor center, if so move to the upper floor center
                lowerCenterCheckFirst();

                //function to check if figure is at any first floor position apart from the center, if so move to the center
                lowerFloorToCenterFirst();

                //function to check if figure is at any second, third or corridor position apart from the upper floor center, if so move to the center
                upperFloorsToCenterFirst(5, 2);
                break;


            //when he is to head for the left room second floor
            case "RL":

                upperCenterCheckFirst(6);

                lowerCenterCheckFirst();

                lowerFloorToCenterFirst();

                upperFloorsToCenterFirst(6, 2);
                break;


            //when he is to head for the center room second floor
            case "RC":

                upperCenterCheckFirst(7);

                lowerCenterCheckFirst();

                lowerFloorToCenterFirst();

                upperFloorsToCenterFirst(7, 1);
                break;


            //when he is to head for the left room second floor
            case "RR":

                upperCenterCheckFirst(8);

                lowerCenterCheckFirst();

                lowerFloorToCenterFirst();

                upperFloorsToCenterFirst(8, 2);
                break;


            //when he is to head for the left room second floor
            case "kitchen":

                upperCenterCheckFirst(9);

                lowerCenterCheckFirst();

                lowerFloorToCenterFirst();

                upperFloorsToCenterFirst(9, 2);
                break;


            //when he is to head for the left room 1 third floor
            case "RTL1":

                upperCenterCheckFirst(10);

                lowerCenterCheckFirst();

                lowerFloorToCenterFirst();

                upperFloorsToCenterFirst(10, 2);
                break;

            //when he is to head for the left room 2 third floor
            case "RTL2":

                upperCenterCheckFirst(11);

                lowerCenterCheckFirst();

                lowerFloorToCenterFirst();

                upperFloorsToCenterFirst(11, 2);
                break;


            //when he is to head for the right room 1 third floor
            case "RTR1":

                upperCenterCheckFirst(12);

                lowerCenterCheckFirst();

                lowerFloorToCenterFirst();

                upperFloorsToCenterFirst(12, 2);
                break;


            //when he is to head for the right room 2 third floor
            case "RTR2":

                upperCenterCheckFirst(13);

                lowerCenterCheckFirst();

                lowerFloorToCenterFirst();

                upperFloorsToCenterFirst(13, 2);
                break;


            default:
                destination = "";
                break;
        }
    }

    public void receptionCheckFirst(int newPath)
    {
        //if his position is the reception
        if (arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position == arranger.paths[0].transform.GetChild(0).position)
        {
            //Switch to the path leading to the Room
            arranger.currentPath = newPath;
            directionReversed = false;

        }
    }

    public void centerCheckFirst()
    {
        //if position is the upper floor center
        if (arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position == arranger.paths[0].transform.GetChild(1).position)
        {
            //go to the first floor center first
            currentWaypoint = 1;
            arranger.currentPath = 0;
            directionReversed = true;
        }
    }

    public void upperFloorToCenterFirst()
    {
        //if the current position on second, third floor or the corridor but not the center of the house
        if (arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position.y > arranger.paths[0].transform.GetChild(0).position.y + 1 &&
            arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position != arranger.paths[0].transform.GetChild(1).position)
        {
            //go to the center first
            directionReversed = true;
        }
    }

    //variables mark the current destination point
    public void firstFloorToReceptionFirst(int pathNumber, int waypointNumber)
    {
        //if the current position on first floor but not the destination or first floor center
        if (arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position.y < arranger.paths[0].transform.GetChild(1).position.y &&
            arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position != arranger.paths[0].transform.GetChild(0).position &&
            arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position != arranger.paths[pathNumber].transform.GetChild(waypointNumber).position)
        {
            //go to the reception first
            directionReversed = true;
        }
    }


    public void upperCenterCheckFirst(int newPath)
    {
        //if position is the upper floor center
        if (arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position == arranger.paths[0].transform.GetChild(1).position)
        {
            //Switch to the path leading to the Room
            arranger.currentPath = newPath;
            currentWaypoint = 0;
            directionReversed = false;

        }
    }


    public void lowerCenterCheckFirst()
    {
        //if position is the lower floor center
        if (arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position == arranger.paths[0].transform.GetChild(0).position)
        {
            //go to the upper center first
            arranger.currentPath = 0;
            directionReversed = false;
        }
    }

    public void lowerFloorToCenterFirst()
    {
        //if the current position on first floor but not the center
        if (arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position.y < arranger.paths[5].transform.GetChild(2).position.y &&
            arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position != arranger.paths[0].transform.GetChild(0).position)
        {
            //go to the center of the first floor first
            directionReversed = true;
        }
    }

    //variables mark the current destination point
    public void upperFloorsToCenterFirst(int pathNumber, int waypointNumber)
    {
        //if the current position on second, third floor or corridor but not the upper center & destination has not been reached yet
        if (arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position.y > arranger.paths[0].transform.GetChild(0).position.y &&
            arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position != arranger.paths[0].transform.GetChild(1).position &&
            arranger.paths[arranger.currentPath].transform.GetChild(currentWaypoint).position != arranger.paths[pathNumber].transform.GetChild(waypointNumber).position &&
            currentWaypoint + 1 == arranger.paths[arranger.currentPath].transform.childCount)
        {
            //go to the center of the upper floors first
            directionReversed = true;
        }
    }
}
