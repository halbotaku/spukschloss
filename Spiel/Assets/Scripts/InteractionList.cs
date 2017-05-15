using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionList : MonoBehaviour {

    //List containing possible combination pickUp Objects
    public GameObject[] pickUpList;

    //create variable holding the position of the interactionObject
    public string position;

    //List containing states of Animation (depending on pickUp Item)
    public string[] animationList;

    //List containing the time hotel Guest is willing to wait until leaving for reception
    public float[] inRoomWaitingList;

    //List containing the time hotel guest is willing to wait until leave of the hotel
    public float[] atReceptionWaitingList;

    //boolean to check whether it has been interacted with befor
    private bool hasBeenInteractedWith = false;

    //variable to save the index of the interaction point
    public int index;

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

        //create an instance of pathFollowerGuest
        pathFollowerGuest startReactionGuest = reactingGuest.GetComponent<pathFollowerGuest>();

        startReactionGuest.letGuestReact(gameObject);

    }
}
