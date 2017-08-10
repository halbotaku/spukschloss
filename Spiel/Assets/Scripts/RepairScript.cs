using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RepairScript : MonoBehaviour {

    //Variable for referencing the current GameObject Skript pathFollower
    private pathFollower hotelOwner;
    private bool isRepairing;

    //ArrayList remebering the destinations & according Objects
    public List<string> roomRepairList = new List<string>();
    public List<string> objectRepairList = new List<string>();

    //boolean evaluating whether hotelOwner has reached the destination yet
    private bool reachedDestination;

    //Variables for calling the animatior of the repair timer
    private GameObject repairTimer;
    private Animator repairTimeAnimator;

    //booleans controlling the waiting times
    private bool goingToRepair = false;

    // Use this for initialization
    void Start () {

        //Reference the pathFollower belonging to the current GameObject
		hotelOwner = GetComponent<pathFollower>();

        //reference the timer GameObject & Animator
        repairTimer = gameObject.transform.GetChild(1).gameObject;
        repairTimeAnimator = repairTimer.GetComponent<Animator>();
        repairTimer.SetActive(false);
    }
	
	// Update is called once per frame
	void LateUpdate () {

        //check for current destination
        if (!isRepairing)
        {
            if (roomRepairList.Count == 0 && objectRepairList.Count == 0)
            {
                hotelOwner.destination = "reception";
            }
            else
            {
                hotelOwner.destination = roomRepairList[0];
            }

            //check the current destination
            if (hotelOwner.destination == "reception")
            {
                //TO DO: CHECK FOR CALMING HOTELGUESTS WANTING TO CHECK OUT
                Debug.Log("Ich habe nichts zu tun!");
            }
            else
            {
                //set Repair-Boolean to true
                isRepairing = true;
            }
        }
		
	}

    IEnumerator goToRepair()
    {
        
        //wait for a second before reacting to the called repair
        yield return new WaitForSeconds(1);

        //activate check whether end has been reached
        reachedDestination = hotelOwner.currentWaypoint + 1 == hotelOwner.arranger.paths[hotelOwner.arranger.currentPath].transform.childCount;

        while (reachedDestination == false)
        {
            yield return null;
            reachedDestination = hotelOwner.currentWaypoint + 1 == hotelOwner.arranger.paths[hotelOwner.arranger.currentPath].transform.childCount;
        }

        //find the listed object whithin the specified room
        GameObject toRepair = GameObject.Find(objectRepairList[0]);

        //get the InteractionList Component
        InteractionList interactionList = toRepair.GetComponent<InteractionList>();

        if (interactionList.position == roomRepairList[0])
        {
            //calculate the grade of damage
            string damage = interactionList.gradeOfDamage[interactionList.index];
            string evaluatedDamage = checkGradeOfDamage(damage);

            //calculate the speed with which the animation needs to be played according to the waiting time
            float animationSpeed = (4 / 3) / interactionList.repairTime[interactionList.index];


            if (animationSpeed - 0.05f > 0)
            {
                animationSpeed -= 0.05f;
            }

            repairTimeAnimator.speed = animationSpeed;

            //set the Repair-Time-Animator to true & play the according timer
            repairTimer.SetActive(true);
            repairTimeAnimator.Play(evaluatedDamage);

            repairTimer.SetActive(true);

            Animator animator = interactionList.GetComponentInChildren<Animator>();

            //wait for the needed repair time length
            yield return new WaitForSeconds(interactionList.repairTime[interactionList.index]);

            //pick the rightful animation out of the animationList
            animator.Play("repaired");
            interactionList.hasBeenInteractedWith = false;

            //remove the first entries of room and repair object lists
            roomRepairList.RemoveAt(0);
            objectRepairList.RemoveAt(0);

            //set Repair-Boolean to false
            isRepairing = false;

            //deactivate the timer
            repairTimer.SetActive(false);
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