using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionList : MonoBehaviour {

    //create variable holding the position of the interactionObject
    public string position;

    //create variable holding the name of the interactionObject
    public string myName;

    //List containing possible combination pickUp Objects
    public string[] pickUpList;

    //List containing states of Animation (depending on pickUp Item)
    public string[] animationList;

    //List containing the graveness of the damage done
    public string[] gradeOfDamage;

    //List containing the time hotel Guest is willing to wait until leaving for reception
    public float[] inRoomWaitingList;

    //List containing the time hotel guest is willing to wait until leave of the hotel
    public float[] atReceptionWaitingList;

    //List containing the necessery times for repairing the occured problem (according to other sequences)
    public float[] repairTime;

    //boolean to check whether it has been interacted with befor
    [HideInInspector]public bool hasBeenInteractedWith = false;

    //variable to save the index of the interaction point
    [HideInInspector] public int index;

    //gameObject referring to the hotel Owner
    [HideInInspector] public GameObject hotelOwner;

    //gameObject remembering the reacting guest
    [HideInInspector] public GameObject reactingGuest;

    //boolean regulating whether an object is found in the according pick-up list
    private bool objectCombinable;

    private string hateObject;


    public void Start()
    {
        //reference the hotelOwner
        hotelOwner = GameObject.FindGameObjectWithTag("hotelOwner");

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        hateObject = camera.GetComponent<GuestSpawn>().hateObject;
    }

    public bool isCombinable(GameObject pickUp)
    {
        PickUpInfo infoList = pickUp.GetComponent<PickUpInfo>();

        for (int i = 0; i < pickUpList.Length; i++)
        {
            if (pickUpList[i].Contains(infoList.myName))
            {
                objectCombinable = true;
                break;
            }
        }

        return !hasBeenInteractedWith && objectCombinable;
    }

    public void combine(GameObject pickUp)
    {
        //get index of current object in pickUpList
        PickUpInfo infoPickUp = pickUp.GetComponent<PickUpInfo>();

        index = Array.IndexOf(pickUpList, infoPickUp.myName);

        Animator animator = GetComponentInChildren<Animator>();

        //pick the rightful animation out of the animationList
        Debug.Log(animationList[index]);
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

        if (reactingGuest && reactingGuest.GetComponent<pathFollowerGuest>().notReacting == true)
        {
            //create an instance of pathFollowerGuest
            pathFollowerGuest startReactionGuest = reactingGuest.GetComponent<pathFollowerGuest>();

            bool isSpecial = reactingGuest.GetComponent<pathFollowerGuest>().isSpecial;
            //Start the process of the guests' Reaction
            if (isSpecial == true && pickUp.GetComponent<PickUpInfo>().myName == hateObject || isSpecial == false)
            {
                startReactionGuest.playScream();
                startReactionGuest.letGuestReact(gameObject, pickUp);
            }
        }

    }
}
