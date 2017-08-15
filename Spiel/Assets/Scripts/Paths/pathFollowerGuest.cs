using System.Collections;
using System.Collections.Generic;
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

    //boolean controlling the accouncement of the special guest
    [HideInInspector] public bool isSpecial;
    public bool notReacting;
    private float warningCountdown;
    private float silenceCountdown;

    private float recepCountdown = 5;

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

    //Animation Controller
    private Animator myAnimator;

    //referencing the specialItems Script for reacting to the scream
    private SpecialPickUp specialItemScream;
    private bool cryOut;
    private bool leave;
    private bool dummy;
    private bool cry;

    //check out controls
    public GameObject fakeHotelOwner;

    //sprite of the guest
    private GameObject sprite;

    //reference your SoundManager
    private HotelGuestSound sounds;
    private AudioSource audioSource;
    private static System.Random rnd;

    //variables remembering if figure has moved
    private float tempXmove;
    private float tempYmove;

    static pathFollowerGuest()
    {
        rnd = new System.Random();
    }


    // This function is called just one time by Unity the moment the game loads
    private void Start()
    {
        tempXmove = transform.position.x;
        tempYmove = transform.position.y;

        // get a reference to the SpriteRenderer component on this gameObject (Flipping the Sprite)
        sprite = this.gameObject.transform.GetChild(2).gameObject;
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        //save the speed value into a variable for switching between idling and going speed
        tempSpeed = speed;

        // get a reference to the Animator for controlling the states of the animation
        myAnimator = GetComponentInChildren<Animator>();

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

        notReacting = true;

        isSpecial = this.gameObject.GetComponentInChildren<PortraitHandler>().isSpecialGuest;
        silenceCountdown = 15;
        
        //reference the according icon GameObject
        reactionIcon = this.gameObject.transform.GetChild(1).gameObject;

        if (isSpecial == true)
        {

            //reference the guest Spawn for the hated object
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            GuestSpawn guests = camera.GetComponent<GuestSpawn>();
            ItemSpawn items = camera.GetComponent<ItemSpawn>();

            //go through the pick up possibilities and find the one the special guest hates
            for (int i = 0; i < items.pickUpItemList.Length; i++)
            {
                //if the name matches the hated name
                if (guests.hateObject == items.pickUpItemList[i].GetComponent<PickUpInfo>().myName)
                {

                    //set the reaction Icon to the hated object image
                    reactionIcon.GetComponent<SpriteRenderer>().sprite = items.pickUpItemList[i].GetComponent<PickUpInfo>().reactionIcon;
                }
            }
        }

        //referencing the scream
        specialItemScream = GameObject.FindGameObjectWithTag("Player").GetComponent<SpecialPickUp>();
        cryOut = false;
        dummy = false;

        //reference the audio management
        sounds = gameObject.GetComponentInChildren<HotelGuestSound>();
        audioSource = gameObject.GetComponentInChildren<AudioSource>();
}

    // Update is called once per frame
    void Update()
    {
        tempXmove = transform.position.x;
        tempYmove = transform.position.y;

        //control the effect of going through the walls
        checkWalls();

        if (isSpecial == true && notReacting == true)
        {
            if (warningCountdown > 0)
            {
                if (reactionIcon.activeInHierarchy == false)
                {
                    reactionIcon.SetActive(true);
                }

                warningCountdown = warningCountdown - Time.deltaTime;
            }

            if (warningCountdown < 0)
            {
                warningCountdown = 0;
                silenceCountdown = 15;
            }

            if (silenceCountdown > 0)
            {
                if (reactionIcon.activeInHierarchy == true)
                {
                    reactionIcon.SetActive(false);
                }

                silenceCountdown = silenceCountdown - Time.deltaTime;
            }

            if (silenceCountdown < 0)
            {
                warningCountdown = 10;
                silenceCountdown = 0;
            }
        }

        if (isSpecial == false && notReacting == true)
        {
            //if you are not unafraid of ghosts and the chosen victim for the scream item
            if (specialItemScream.victim == room || dummy == true)
            {
                if (room != null)
                {
                    room = null;
                    dummy = true;
                    goingToReception = true;

                    directionReversed = false;
                    idleInRoom = false;

                    speed = 10;

                    arranger.currentPath = 1;
                    currentWaypoint = 0;

                    cryOut = true;
                }

                if (cryOut == true)
                {
                    if (currentWaypoint == arranger.paths[1].transform.childCount - 2)
                    {
                        cryOut = false;
                        goingToReception = false;
                        arranger.currentPath = 2;
                        currentWaypoint = 0;
                        leave = true;
                    }
                }

                if (leave == true && arranger.currentPath == 2 && currentWaypoint==0)
                {
                    leave = false;
                    currentWaypoint++;

                    cry = true;
                }

                if (cry == true && arranger.currentPath == 2 && currentWaypoint == 1 && gameObject.transform.position.y < -6.25)
                {
                    startLeaving();
                    cry = false;
                }
            }
        }

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
            //only start the play animation when you have stood before, otherweise avoid a reload
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("stand") || transform.position.x != tempXmove && transform.position.y != tempYmove)
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
                //When hotelguest has reached the side of the room
                if (currentWaypoint + 1 == arranger.paths[arranger.currentPath].transform.childCount)
                {
                    if (idleInRoom)
                    {
                        if (idleCountdown > 0)
                        {
                            //only stop the play animation when you have walked before, otherweise avoid a reload
                            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("walk") && tempXmove == gameObject.transform.position.x && tempYmove == gameObject.transform.position.y)
                            {
                                myAnimator.Play("stand");
                            }

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
                            //only stop the play animation when you have walked before, otherweise avoid a reload
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
            //only stop the play animation when you have walked before, otherweise avoid a reload
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("walk") && transform.position.x == tempXmove && transform.position.y == tempYmove)
            {
                myAnimator.Play("stand");
            }

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
        if (isReturningToIdling == true)
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
        if (isWaitingAtReception == true && tempXmove == gameObject.transform.position.x && tempYmove == gameObject.transform.position.y)
        {
            //only stop the play animation when you have walked before, otherweise avoid a reload
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
            {
                myAnimator.Play("stand");
                reactionIcon.SetActive(true);
            }

            if (fakeHotelOwner.GetComponent<CheckOut>().isCheckingOut == true)
            {
                //start the checkout duration
                receptionWaitingCountdown = receptionWaitingCountdown - Time.deltaTime;

                //when the ghost triggered the checkout
                if (receptionWaitingCountdown < 0)
                {
                    //stop waiting and go to reception
                    stopWaitingAtReception();
                }
            }
            else
            {
                if (recepCountdown > 0)
                {
                    recepCountdown = recepCountdown - Time.deltaTime;
                }

                if (recepCountdown <= 0)
                {
                    if (hasBeenRepairedYet(brokenObject))
                    {
                        //return back to idling in your room
                        isReturningToIdling = true;
                    }
                    else {
                        //stop waiting and go back
                        isWaitingAtReception = false;
                        isReturningToRoom = true;
                        recepCountdown = 5;
                    }
                }
            }

            if (fakeHotelOwner.GetComponent<CheckOut>().isCheckingOut == false && fakeHotelOwner.GetComponent<CheckOut>().hotelOwner.GetComponent<pathFollower>().destination == "reception")
            {
                if (hasBeenRepairedYet(brokenObject))
                {
                    //stop waiting and go back
                    isWaitingAtReception = false;
                    isReturningToRoom = true;
                }
                else
                {
                    goingToReception = true;
                    recepCountdown = 5;
                }
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

        if (reachedRoom == true && tempXmove == gameObject.transform.position.x && tempYmove == gameObject.transform.position.y)
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

        if (reachedOutside == true && tempXmove == gameObject.transform.position.x && tempYmove == gameObject.transform.position.y)
        {
            goingToReception = false;
            reachedOutside = false;
            startLeaving();
        }

        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("walk") && transform.position.x == tempXmove && transform.position.y == tempYmove)
        {
            myAnimator.Play("stand");
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
        notReacting = false;

        //remember which object caused the reaction
        brokenObject = interactionObject;

        //set the reaction Icon Sprite to the according one
        SpriteRenderer renderer = reactionIcon.GetComponent<SpriteRenderer>();

        //reference the pickup Item
        GameObject pickupItem = pickupObject;

        PickUpInfo info = pickupItem.GetComponent<PickUpInfo>();

        renderer.sprite = info.reactionIcon;

        //call the time the guest is willing to wait until repair
        InteractionList list = interactionObject.GetComponent<InteractionList>();
        waitUntilReception = list.inRoomWaitingList[list.index];
        waitUntilLeave = list.atReceptionWaitingList[list.index];

        //Set the Countdown to the waiting times
        roomWaitingCountdown = waitUntilReception;
        receptionWaitingCountdown = 1;

        //calculate the speed with which the animation needs to be played according to the waiting time
        float animationSpeed = (4/3) / waitUntilReception;

        if (animationSpeed - 0.05f > 0)
        {
            animationSpeed -= 0.05f;
        }

        //Assign the animator
        patienceCounter = this.gameObject.transform.GetChild(0).gameObject;

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
        /*
         * Actual Reaction follows now, Countdown is handled in the Update function
         */

        //hotel guest reacts to damage
        reactionIcon.SetActive(true);

        //assign yourself to waiting at reception once you reached it
        isWaitingAtReception = true;
        goingToReception = false;
    }


    private void stopWaitingAtReception()
    {
        //hotel guest reacts to damage
        reactionIcon.SetActive(false);

        //stop waiting
        isWaitingAtReception = false;

            //pick the way leading out of the hotel
            currentWaypoint = 0;
            arranger.currentPath = 2;
            directionReversed = false;
            goingToReception = true;
    }

    private void returnToRoom()
    {
        //return back to idling in your room
        arranger.currentPath = 0;
        directionReversed = false;
        goingToReception = false;
        idleInRoom = true;

        notReacting = true;

        receptionWaitingCountdown = waitUntilLeave;
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
                patienceCounterAnim = "hardest_time_animation";
                break;
        }

        return patienceCounterAnim;
    }

    private void checkWalls()
    {
        if (transform.position.y > -5 && transform.position.y < -1.55 || transform.position.y > -0.9 && transform.position.y < 0.4 )
        {
            if (room == "" || room == "RTL1" || room == "RTL2" || room == "RTR1" || room == "RTR2" || room == "pool" && arranger.currentPath == 1 || room == "RL" && arranger.currentPath == 1 ||
                room == "RC" && arranger.currentPath == 1 || room == "RR" && arranger.currentPath == 1 || room == "kitchen" && arranger.currentPath == 1)
            {
                sprite.SetActive(false);
            }
            else
            {
                sprite.SetActive(true);
            }
        }
        else if (room == "RL1" && transform.position.x < -3 && transform.position.x > -5 || room == "RR2" && transform.position.x > 2.9 && transform.position.x < 5.9 ||
        room == "RL2" && transform.position.x < -3 && transform.position.x > -3.5 || room == "RR1" && transform.position.x > 2.9 && transform.position.x < 3.4)
        {
            sprite.SetActive(false);
        }
        else
        {
            sprite.SetActive(true);
        }
    }

    public void playScream()
    {
        int number = rnd.Next(0, 4);

        switch (number)
        {
            case 0:
                audioSource.clip = sounds.screamFemaleA;
                break;
            case 1:
                audioSource.clip = sounds.screamFemaleB;
            break;
            case 2:
                audioSource.clip = sounds.screamMaleA;
                break;
            case 3:
                audioSource.clip = sounds.screamMaleB;
                break;
        }

        audioSource.Play();
    }
}