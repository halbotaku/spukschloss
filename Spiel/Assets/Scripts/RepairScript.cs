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

	// Use this for initialization
	void Start () {

        //Reference the pathFollower belonging to the current GameObject
		hotelOwner = GetComponent<pathFollower>();
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
                //TO DO: WAIT UNTIL DESTINATION HAS BEEN REACHED, THEN TAKE CARE OF REPAIR
                Debug.Log("Ich muss etwas reparieren!");

                //set Repair-Boolean to true
                isRepairing = true;

                //wait for a second before reacting to the called repair
                StartCoroutine(goToRepair());

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
        }
    }
}