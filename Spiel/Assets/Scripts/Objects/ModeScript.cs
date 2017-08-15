using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeScript : MonoBehaviour {

    //Booleans handling active PickUp & Interaction Mode
    public bool pickUpMode;
    public bool interactionMode;

    //Variables referencing the Pick-Up-Object & Shader
    private GameObject pickUpSprite;
    private GameObject glow;

    //referencing the player
    private GameObject player;
    private ObjectPickUp playerMovement;

    //referencing the sprite types
    public Sprite pickUpGlow;
    public Sprite interactionGlow;

    public void Start()
    {
        //reference the pickUp Object & Shader
        glow = gameObject.transform.GetChild(1).gameObject;
        SpriteRenderer glowSprite = glow.GetComponent<SpriteRenderer>();

        //disable shader
        glow.SetActive(false);

        if (gameObject.CompareTag("pickUpObject"))
        {
            glowSprite.sprite = pickUpGlow;
        }
        else
        {
            glowSprite.sprite = interactionGlow;
        }

        //remember who the player is
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<ObjectPickUp>();
    }

    public void Update()
    {
        //get your current position
        Vector3 itemPosition = gameObject.transform.position;
        Vector3 playerPosition = player.transform.position;

        //evaluate the distance between the positions
        float dist = Vector3.Distance(itemPosition, playerPosition);

        //if you are a pick-up-Object
        if (gameObject.CompareTag("pickUpObject"))
        {
            //if you are within the mode radius & not being carried by the player
            if (dist > playerMovement.modeRadius || this.gameObject == playerMovement.followPlayer)
            {
                pickUpMode = false;
            }
            else
            {
                pickUpMode = true;
            }

            //check if you are in pickUp-Mode
            if (pickUpMode == true)
            {
                glow.SetActive(true);
            }
            else
            {
                glow.SetActive(false);
            }

        }


        //if you are an interaction-object
        else
        { 
            //reference your interactionList
            InteractionList list = this.gameObject.GetComponent<InteractionList>();

            //if you are whithin the mode radius && combinable with the object being carried by the player
            if (playerMovement.followPlayer)
            {
                if (dist < playerMovement.modeRadius && list.isCombinable(playerMovement.followPlayer))
                {
                    interactionMode = true;
                }
                else
                {
                    interactionMode = false;        }
            }

            if (!playerMovement.followPlayer)
            {
                interactionMode = false;
            }

            //check if you are in interaction-Mode
            if (interactionMode == true)
            {
                glow.SetActive(true);
            }
            else
            {
                glow.SetActive(false);
            }
        }
    }


}
