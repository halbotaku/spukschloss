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
    public GameObject[] pickUpList;

    //List containing states of Animation (depending on pickUp Item)
    public string[] animationList;

    //List containing the graveness of the damage done
    public string[] gradeOfDamage;

    //List containing the time hotel Guest is willing to wait until leaving for reception
    [NonSerialized] public float[] inRoomWaitingList;

    //List containing the time hotel guest is willing to wait until leave of the hotel
    [NonSerialized] public float[] atReceptionWaitingList;

    //List containing the necessery times for repairing the occured problem (according to other sequences)
    [NonSerialized] public float[] repairTime;

    //boolean to check whether it has been interacted with befor
    [NonSerialized] public bool hasBeenInteractedWith = false;

    //variable to save the index of the interaction point
    [NonSerialized] public int index;

    //gameObject referring to the hotel Owner
    private GameObject hotelOwner;

    //gameObject remembering the reacting guest
    private GameObject reactingGuest;

    public void Start()
    {
        //reference the hotelOwner
        hotelOwner = GameObject.FindGameObjectWithTag("hotelOwner");

        //reference the main Camera for the existing pick-Up-Object List
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        ItemSpawn pickUpPool = mainCamera.GetComponent<ItemSpawn>();
        InteractionSpawn interactionPool = mainCamera.GetComponent<InteractionSpawn>();

        //define the size of the array & fill it
        pickUpList = new GameObject[pickUpPool.pickUpItemList.Length];

        for (int i = 0; i < pickUpPool.pickUpItemList.Length; i++)
        {
            pickUpList[i] = pickUpPool.pickUpItemList[i];
        }

        inRoomWaitingList = new float[pickUpList.Length];

        //now check for the waiting times & set them according to the intensity of the damage (at room)
        for (int i = 0; i < pickUpList.Length; i++)
        {
            inRoomWaitingList[i] = checkIntensityRoom(gradeOfDamage[i], interactionPool);
        }

        atReceptionWaitingList = new float[pickUpList.Length];

        //now check for the waiting times & set them according to the intensity of the damage (at reception)
        for (int i = 0; i < pickUpList.Length; i++)
        {
            atReceptionWaitingList[i] = checkIntensityReception(gradeOfDamage[i], interactionPool);
        }

        repairTime = new float[pickUpList.Length];

        //now check for the waiting times & set them according to the intensity of the damage (for repair)
        for (int i = 0; i < pickUpList.Length; i++)
        {
            repairTime[i] = checkIntensityReception(gradeOfDamage[i], interactionPool);
        }
    }

    private float checkIntensityRoom(string intensity, InteractionSpawn interactionPool)
    {
        float result;

        switch (intensity)
        {
            case "light":
                result = interactionPool.roomWaitingTimes[0];
                break;
            case "middle":
                result = interactionPool.roomWaitingTimes[1];
                break;
            case "heavy":
                result = interactionPool.roomWaitingTimes[2];
                break;
            case "important":
                result = interactionPool.roomWaitingTimes[3];
                break;
            default:
                result = interactionPool.roomWaitingTimes[0];
                break;
        }

        return result;
    }

    private float checkIntensityReception(string intensity, InteractionSpawn interactionPool)
    {
        float result;

        switch (intensity)
        {
            case "light":
                result = interactionPool.receptionWaitingTimes[0];
                break;
            case "middle":
                result = interactionPool.receptionWaitingTimes[1];
                break;
            case "heavy":
                result = interactionPool.receptionWaitingTimes[2];
                break;
            case "important":
                result = interactionPool.receptionWaitingTimes[3];
                break;
            default:
                result = interactionPool.receptionWaitingTimes[0];
                break;
        }

        return result;
    }

    private float checkIntensityRepair(string intensity, InteractionSpawn interactionPool)
    {
        float result;

        switch (intensity)
        {
            case "light":
                result = interactionPool.repairTimes[0];
                break;
            case "middle":
                result = interactionPool.repairTimes[1];
                break;
            case "heavy":
                result = interactionPool.repairTimes[2];
                break;
            case "important":
                result = interactionPool.repairTimes[3];
                break;
            default:
                result = interactionPool.repairTimes[0];
                break;
        }

        return result;
    }

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
            startReactionGuest.letGuestReact(gameObject, pickUp);
        }

    }
}
