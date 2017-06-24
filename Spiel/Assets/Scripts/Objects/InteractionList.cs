using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionList : MonoBehaviour {

    //create variable holding the position of the interactionObject
    public string position;

    //List containing possible combination pickUp Objects
    public GameObject[] pickUpList;

    //List containing states of Animation (depending on pickUp Item)
    public string[] animationList;

    //List containing the time hotel Guest is willing to wait until leaving for reception
    public float[] inRoomWaitingList;

    //List containing the time hotel guest is willing to wait until leave of the hotel
    public float[] atReceptionWaitingList;

    //List containing the necessery times for repairing the occured problem (according to other sequences)
    public float[] repairTime;

    //boolean to check whether it has been interacted with befor
    public bool hasBeenInteractedWith = false;

    //variable to save the index of the interaction point
    public int index;

    //gameObject referring to the hotel Owner
    public GameObject hotelOwner;

    //gameObject remembering the reacting guest
    private GameObject reactingGuest;


    public bool isCombinable(GameObject pickUp)
    {
        return !hasBeenInteractedWith && pickUpList.Contains(pickUp);
    }

    public void combine(GameObject pickUp)
    {
        //get index of current object in pickUpList
        index = Array.IndexOf(pickUpList, pickUp);

        Animator animator = GetComponentInChildren<Animator>();

        //pick the rightful animation out of the animationList
        animator.Play(animationList[index]);
        hasBeenInteractedWith = true;

        //add the object and Room the the hotel Owner's TO DO List
        RepairScript repairScript = hotelOwner.GetComponent<RepairScript>();

        repairScript.roomRepairList.Add(position);
        repairScript.objectRepairList.Add(this.name);


        //gather all guests
        GameObject[] guests = GameObject.FindGameObjectsWithTag("guest");

        //select the one matching the current interaction object room
        foreach (GameObject guest in guests)
        {
            if (guest.GetComponent<pathFollowerGuest>().room == position)
            {
                reactingGuest = guest;
            }
        }

        if (reactingGuest)
        {
            //create an instance of pathFollowerGuest
            pathFollowerGuest startReactionGuest = reactingGuest.GetComponent<pathFollowerGuest>();

            //Start the proces of the guests' Reaction
            startReactionGuest.letGuestReact(gameObject);
        }

    }
}
