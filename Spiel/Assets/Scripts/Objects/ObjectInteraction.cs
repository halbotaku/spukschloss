using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour {

    //create ObjectPickUp instance to check whether player is holding an item
    private ObjectPickUp pickUp;

    //create GameObject for the current interaction object
    private GameObject currentInteraction = null;

    //reference the Player AudioSource
    private AudioSource audio;
    private PlayerSound audioManager;

        // Use this for initialization
        void Start () {
        pickUp = GetComponent<ObjectPickUp>();

        //reference the sound controller
        audio = this.gameObject.GetComponent<AudioSource>();
        audioManager = this.gameObject.GetComponent<PlayerSound>();
    }
	
	// Update is called once per frame
	void Update () {
        //check, when an item is held and the player is hovering over an interaction object (-> currentInteraction), if the player activates the interaction
        if (currentInteraction!=null && Input.GetButtonDown("pickUp"))
        {
            //trigger the interaction
            audio.clip = audioManager.combine;
            audio.loop = false;
            audio.Play();
            currentInteraction.GetComponent<InteractionList>().combine(pickUp.followPlayer);

            //delete the used pick up item
            Destroy(pickUp.followPlayer);
        }
		
	}

    //Method for detecting the interaction object
    void OnTriggerEnter2D(Collider2D other)
    {
        //if the object is of the tag pickup Object
        if (other.gameObject.CompareTag("interactionObject") && pickUp.followPlayer != null )
        {
            //create instance of interactionList
            InteractionList list = other.GetComponent<InteractionList>();

            if (list!=null && list.isCombinable(pickUp.followPlayer))
            {
                currentInteraction = other.gameObject;
            }

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //if the object is of the tag pickup Object
        if (other.gameObject.CompareTag("interactionObject"))
        {
            currentInteraction = null;
        }
    }

}
