using System.Collections;
using UnityEngine;

public class pathFollowerGuest : MonoBehaviour
{

    //Create variable for remembering the current waypoint
    public int currentWaypoint = 0;

    //create variable remembering the brokenObject (for repair)
    private GameObject brokenObject;

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

    //variable for checking whether customer reached the reception already
    private bool reachedReception;

    //variables for counting down the waiting time at the room
    private float roomWaitingCountdown;
    private bool isWaitingInRoom = false;

    //variables for counting down the waiting time at the reception
    private float receptionWaitingCountdown;
    private bool isWaitingAtReception = false;

    //boolean handling whether guest has reached the outside
    private bool reachedOutside;
    private bool reachedRoom;

    //guestCounter for Game-Scor
    public GameObject guestCounter;

    //variable holding guest reaction Icon
    private GameObject reactionIcon;

    //Handling the Countdown/Animation part
    private GameObject patienceCounter;
    private Animator patienceCounterAnimator;

    //handling the way back
    private bool isReturningToIdling;
    private bool isReturningToRoom;

    //remembering the intitial hotelguest-position
    Vector3 position;
    float offset;
    public bool floorPositionTop;

    // This function is called just one time by Unity the moment the game loads
    private void Awake()
    {
        // get a reference to the SpriteRenderer component on this gameObject (Flipping the Sprite)
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        //save the speed value into a variable for switching between idling and going speed
        tempSpeed = speed;

        //Set idling time for counting down
        idleCountdown = idlingTime;

        //Set the position variables for the hotelguest
        position = transform.position;

        if (position.y > 0)
        {
            offset = 0.1f;
        }
        else
        {
            offset = - 0.1f;
        }
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

        //Handle the in-Room-Waiting Countdown
        if (isWaitingInRoom == true && roomWaitingCountdown > 0)
        {
            roomWaitingCountdown -= Time.deltaTime;

            if (roomWaitingCountdown <= 0)
            {
                //stop waiting and go to reception
                stopWaitingInRoom();
            }

            if (hasBeenRepairedYet(brokenObject))
            {
                isWaitingInRoom = false;
                isReturningToIdling = true;
            }
        }

        //Handle returning to idling at any point
        if (isReturningToIdling == true && isWaitingInRoom == false)
        {
            reactionIcon.SetActive(false);
            patienceCounter.SetActive(false);

            isReturningToIdling = false;

            //return back to idling in your room
            returnToRoom();
        }

        //Handle waiting until having reached the reception
        if (reachedReception == false && goingToReception == true && directionReversed == false && arranger.currentPath == 0 ||
            reachedReception == false && goingToReception == true && directionReversed == false && arranger.currentPath == 1)
        {
            //Keep on checking until you reached the reception
            reachedReception = currentWaypoint + 1 == arranger.paths[arranger.currentPath].transform.childCount;
        }

        if (reachedReception == true)
        {
            goingToReception = false;
            reachedReception = false;
            startWaitingAtReception();
        }


        //Handle the at-Reception-Waiting Countdown
        if (isWaitingAtReception == true && receptionWaitingCountdown > 0)
        {
            receptionWaitingCountdown -= Time.deltaTime;

            if (receptionWaitingCountdown <= 0)
            {
                //stop waiting and go to reception
                stopWaitingAtReception();
            }

            if (hasBeenRepairedYet(brokenObject))
            {
                //stop waiting and go back
                isWaitingAtReception = false;
                isReturningToRoom = true;
            }
        }

        //Handle returning to the room
        if (isReturningToRoom == true && isWaitingAtReception == false)
        {
            reactionIcon.SetActive(false);
            patienceCounter.SetActive(false);

            isReturningToRoom = false;

            //return back to your room
            directionReversed = true;
            isWaitingAtReception = false;
            goingToReception = true;
        }

        //Handle waiting until having reached the Room when going back
        if (directionReversed == true && goingToReception == true && reachedRoom == false && transform.position.y >= position.y - offset)
        {
            //Keep on checking until you reached the reception
            reachedRoom = currentWaypoint == 0;
        }

        if (reachedRoom == true)
        {
            goingToReception = false;
            reachedRoom = false;
            returnToRoom();
        }

        //Handle waiting until having reached the Outside
        if (reachedOutside == false && goingToReception == true && directionReversed == false)
        {
            //Keep on checking until you reached the reception
            reachedOutside = currentWaypoint + 1 == arranger.paths[arranger.currentPath].transform.childCount && this.gameObject.transform.position.y <= -6.00;
        }

        if (reachedOutside == true)
        {
            goingToReception = false;
            reachedOutside = false;
            startLeaving();
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

    public void letGuestReact(GameObject interactionObject, GameObject pickupObject)
    {
        //remember which object caused the reaction
        brokenObject = interactionObject;

        //get the according icon GameObject
        reactionIcon = this.gameObject.transform.GetChild(2).gameObject;

        //set the reaction Icon Sprite to the according one
        SpriteRenderer renderer = reactionIcon.GetComponent<SpriteRenderer>();

        //reference the pickup Item
        GameObject pickupItem = pickupObject;

        PickUpInfo info = pickupItem.GetComponent<PickUpInfo>();

        renderer.sprite = info.reactionIcon;
        
        //PickUpInfo info = brokenObject.GetComponent<PickUpInfo>();

        //call the time the guest is willing to wait until repair
        InteractionList list = interactionObject.GetComponent<InteractionList>();
        waitUntilReception = list.inRoomWaitingList[list.index];
        waitUntilLeave = list.atReceptionWaitingList[list.index];

        //Set the Countdown to the waiting times
        roomWaitingCountdown = waitUntilReception;
        receptionWaitingCountdown = waitUntilLeave;

        //calculate the speed with which the animation needs to be played according to the waiting time
        float animationSpeed = (4/3) / waitUntilReception;

        if (animationSpeed - 0.05f > 0)
        {
            animationSpeed -= 0.05f;
        }

        //Assign the animator
        patienceCounter = this.gameObject.transform.GetChild(1).gameObject;

        patienceCounterAnimator = patienceCounter.GetComponent<Animator>();
        patienceCounterAnimator.speed = animationSpeed;

        string gradeOfDamage = list.gradeOfDamage[list.index];

        //check for the grade of damage done
        string patienceCounterAnim = checkGradeOfDamage(gradeOfDamage);

        /*
         * Actual Rection follows now, Countdown is handled in the Update function
         */

        //hotel guest reacts to damage
        reactionIcon.SetActive(true);

        //Stop idling and start waiting
        idleInRoom = false;
        isWaitingInRoom = true;

        //set patienceCounter active
        patienceCounter.SetActive(true);

        //set the patience counter active and play it for the necessary waiting time
        patienceCounterAnimator.Play(patienceCounterAnim);
    }


    private void stopWaitingInRoom()
    {
        isWaitingInRoom = false;

        //set patienceCounter active
        patienceCounter.SetActive(false);

        //hotel guest reacts to damage
        reactionIcon.SetActive(false);

        //check whether damage has been repaired after waiting for so long
        if (hasBeenRepairedYet(brokenObject))
        {
            //return back to idling in your room
            isReturningToIdling = true;
        }
        else
        {
            //stop waiting
            isWaitingInRoom = false;

            //otherwise go to the reception
            speed = tempSpeed;

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
    }

    private void startWaitingAtReception()
    {
        //calculate the speed with which the animation needs to be played according to the waiting time
        float animationSpeed = (4 / 3) / waitUntilLeave;

        if (animationSpeed - 0.05f > 0)
        {
            animationSpeed -= 0.025f;
        }

        patienceCounterAnimator.speed = animationSpeed;

        //reference the brokenObject
        InteractionList list = brokenObject.GetComponent<InteractionList>();
        string gradeOfDamage = list.gradeOfDamage[list.index];

        //check for the grade of damage done
        string patienceCounterAnim = checkGradeOfDamage(gradeOfDamage);

        /*
         * Actual Reaction follows now, Countdown is handled in the Update function
         */

        //hotel guest reacts to damage
        reactionIcon.SetActive(true);

        //assign yourself to waiting at reception once you reached it
        isWaitingAtReception = true;
        goingToReception = false;

        //set patienceCounter active
        patienceCounter.SetActive(true);

        //set the patience counter active and play it for the necessary waiting time
        patienceCounterAnimator.Play(patienceCounterAnim);
    }


    private void stopWaitingAtReception()
    {
        //hotel guest reacts to damage
        reactionIcon.SetActive(false);

        //stop waiting
        isWaitingAtReception = false;

        //set patienceCounter active
        patienceCounter.SetActive(false);

        //check whether damage has been repaired yet after waiting for so long
        if (hasBeenRepairedYet(brokenObject))
        {
            isReturningToRoom = true;
        }
        else
        {
            //pick the way leading out of the hotel
            currentWaypoint = 0;
            arranger.currentPath = 2;
            directionReversed = false;
            goingToReception = true;
        }
    }

    private void returnToRoom()
    {
        //return back to idling in your room
        arranger.currentPath = 0;
        directionReversed = false;
        goingToReception = false;
        idleInRoom = true;
    }

    private void startLeaving()
    {
        //increase the Score Counter of the game
        TowerClock counter = guestCounter.GetComponent<TowerClock>();
        counter.guestCounter += 1;

        //reference your portrait
        PortraitHandler portrait = this.gameObject.GetComponent<PortraitHandler>();

        //Destroy the portrait
        portrait.destroy();

        //destroy the hotelguest GameObject
        GameObject.Destroy(gameObject);
    }

    private bool hasBeenRepairedYet(GameObject brokenObject)
    {
        InteractionList list = brokenObject.GetComponent<InteractionList>();

        if (list.hasBeenInteractedWith == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string checkGradeOfDamage(string damage)
    {
        string patienceCounterAnim;

        switch (damage)
        {
            case "light":
                patienceCounterAnim = "light_time_animation";
                break;
            case "middle":
                patienceCounterAnim = "middle_time_animation";
                break;
            case "heavy":
                patienceCounterAnim = "heavy_time_animation";
                break;
            default:
                patienceCounterAnim = "light_time_animation";
                break;
        }

        return patienceCounterAnim;
    }
}