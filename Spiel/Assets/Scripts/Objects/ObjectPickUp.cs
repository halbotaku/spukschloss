using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour
{

    //stop/begin following the player when picked up
    [NonSerialized]public GameObject followPlayer = null;

    //Vector for creating distance of player and picked up Object
    public Vector2 followOffset;

    //variable for influencing the speed of the floating movement
    public float floatingSpeed;

    //variable for influencing the floating range (amplitude) of the following Object
    public float floatingDistance;

    //instancing a player object
    private PlayerScript player = null;

    //checking the flip direction of the floating object
    private bool isFlipped;

    //variable for remebering the item
    private GameObject floatingItem;

    //variable for movement speed of flipping sides
    public float flipFloatMovement;

    //variable remembering the item currently hovered over
    private GameObject hoverItem;
    private Boolean isHovering;

    //Spriterenderer for displaying the magnifying Icons
    public GameObject iconObject;
    private SpriteRenderer spriteRenderer;

    //Variable changing the distance of the PickUp/Interaction Mode
    public float modeRadius;

    // Use this for initialization
    void Start()
    {

        player = GetComponent<PlayerScript>();

        spriteRenderer = iconObject.GetComponent<SpriteRenderer>();

}

    // Update is called once per frame
    void Update()
    {
        //If an Item has been picked up already and the player picks up another one
        if (floatingItem != null && Input.GetButtonDown("pickUp"))
        {
            GameObject temp = followPlayer;
            followPlayer = floatingItem;
            floatingItem = temp;

            //set hovering to false
            isHovering = false;
        }
        
        //When the Item has been picked up to follow the player
        if (followPlayer != null)
        {

            //When movement is going to the left
            if (player.movement.x < 0)
            {
                isFlipped = true;

            }
            else if (player.movement.x > 0)
            {
                isFlipped = false;
            }

            Vector2 goal;

            if (isFlipped)
            {
                // flip the floating object to the right of the player

                /* Trigonometry Calculations: Set the position of the floating Object to the one of the Player including
                 * a variable offset and let it float up and down via a sinus curve
                 */
                Vector2 reverseVector = followOffset;
                reverseVector.Scale(new Vector2(-1, 1));

                goal = transform.position + (Vector3)reverseVector + Mathf.Sin(Time.time * floatingSpeed) * Vector3.up * floatingDistance;
            }
            else
            {
                // revert the position to the left of the player

                goal = transform.position + (Vector3)followOffset + Mathf.Sin(Time.time * floatingSpeed) * Vector3.up * floatingDistance;
            }

            followPlayer.transform.position = Vector2.Lerp(followPlayer.transform.position, goal, Time.deltaTime * flipFloatMovement);
        }



            //Checking whether player is hovering over a pickup item
            if (isHovering == true && hoverItem != null)
            {
                //display the according magifying icon close to the Item
                PickUpInfo info = hoverItem.GetComponent<PickUpInfo>();

                iconObject.transform.position = hoverItem.transform.position;

                Vector3 heightOffset = new Vector3(0, 0.5f, 0);
                Vector3 widthOffset = new Vector3(0.5f, 0, 0);

            if (player.transform.position.y >= hoverItem.transform.position.y)
            {
                iconObject.transform.position -= heightOffset;
            }
            else
            {
                iconObject.transform.position += heightOffset;
            }

            if (player.transform.position.x >= hoverItem.transform.position.x)
            {
                iconObject.transform.position -= widthOffset;
            }
            else
            {
                iconObject.transform.position += widthOffset;
            }

            spriteRenderer.sprite = info.magnifyIcon;
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }

    }

    //Method for picking up pickUpObjects when player is colliding with them
    void OnTriggerEnter2D(Collider2D other)
    {
        //if the object is of the tag pickup Object
        if (other.gameObject.CompareTag("pickUpObject") && other.gameObject!=followPlayer)
        {
            floatingItem = other.gameObject;

            //remember the item you're hovering above
            hoverItem = other.gameObject;
            isHovering = true;
        }
    }

    //Method for picking up pickUpObjects when player is colliding with them
    void OnTriggerStay2D(Collider2D other)
    {
        //if the object is of the tag pickup Object
        if (other.gameObject.CompareTag("pickUpObject") && other.gameObject != followPlayer)
        {
            floatingItem = other.gameObject;

            //remember the item you're hovering above
            hoverItem = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //if the object is of the tag pickup Object
        if (other.gameObject.CompareTag("pickUpObject") && other.gameObject != followPlayer)
        {
            floatingItem = null;

            //stop the hovering
            hoverItem = null;
            isHovering = false;
        }
    }

}
