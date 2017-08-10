using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpecialPickUp : MonoBehaviour
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

    //countdown for special item effects
    SpecialItem item;
    private Color origColor;
    private float slimeCount;
    private bool isSliming;
    private Vector2 tempSpeed;
    private Vector2 newSpeed;
    private SpriteRenderer sprite;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<PlayerScript>();
        tempSpeed = player.speed;
        isSliming = false;

        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (floatingItem != null)
        {
            //set hovering to false
            isHovering = false;

            item = floatingItem.GetComponent<SpecialItem>();

            slimeCount = item.speedDuration;

            if (item.myName == "slime")
            {
                isSliming = true;
            }
        }

        if (isSliming == true)
        {
            Destroy(floatingItem);

            sprite.color = Color.blue;

            player.speed = item.newSpeed;

            if (slimeCount > 0)
            {
                slimeCount = slimeCount - Time.deltaTime;
            }

            if (slimeCount < 0)
            {
                player.speed = tempSpeed;
                isSliming = false;
                sprite.color = new Color(1f, 1f, 1f, 1f);
            }
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
    }

    //Method for picking up pickUpObjects when player is colliding with them
    void OnTriggerEnter2D(Collider2D other)
    {
        //if the object is of the tag pickup Object
        if (other.gameObject.CompareTag("specialObject") && other.gameObject!=followPlayer)
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
        if (other.gameObject.CompareTag("specialObject") && other.gameObject != followPlayer)
        {
            floatingItem = other.gameObject;

            //remember the item you're hovering above
            hoverItem = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //if the object is of the tag pickup Object
        if (other.gameObject.CompareTag("specialObject") && other.gameObject != followPlayer)
        {
            floatingItem = null;

            //stop the hovering
            hoverItem = null;
            isHovering = false;
        }
    }

}