using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionList : MonoBehaviour {

    //List containing possible combination pickUp Objects
    public GameObject[] pickUpList;

    //List containing states of Animation (depending on pickUp Item)
    public string[] animationList;

    //boolean to check whether it has been interacted with befor
    private bool hasBeenInteractedWith = false;

    public bool isCombinable(GameObject pickUp)
    {
        return !hasBeenInteractedWith && pickUpList.Contains(pickUp);
    }

    public void combine(GameObject pickUp)
    {
        //get index of current object in pickUpList
        int index = Array.IndexOf(pickUpList, pickUp);

        Animator animator = GetComponentInChildren<Animator>();

        //pick the rightful animation out of the animationList
        animator.Play(animationList[index]);
        hasBeenInteractedWith = true;
    }
}
