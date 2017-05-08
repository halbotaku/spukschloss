using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPickUp : MonoBehaviour
{

    //stop/begin following the player when picked up
    public GameObject followPlayer = null;

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

    // Use this for initialization
    void Start()
    {

        player = GetComponent<PlayerScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if (floatingItem != null && Input.GetButtonDown("pickUp"))
        {
            followPlayer = floatingItem;
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

            if (isFlipped)
            {
                // flip the floating object to the right of the player

                /* Trigonometry Calculations: Set the position of the floating Object to the one of the Player including
                 * a variable offset and let it float up and down via a sinus curve
                 */
                Vector2 reverseVector = followOffset;
                reverseVector.Scale(new Vector2(-1, 1));

                followPlayer.transform.position = transform.position + (Vector3)reverseVector + Mathf.Sin(Time.time * floatingSpeed) * Vector3.up * floatingDistance;
            }
            else
            {
                // revert the position to the left of the player

                followPlayer.transform.position = transform.position + (Vector3)followOffset + Mathf.Sin(Time.time * floatingSpeed) * Vector3.up * floatingDistance;
            }
        }

    }

    //Method for picking up pickUpObjects when player is colliding with them
    void OnTriggerEnter2D(Collider2D other)
    {
        //if the object is of the tag pickup Object
        if (other.gameObject.CompareTag("pickUpObject"))
        {
            floatingItem = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //if the object is of the tag pickup Object
        if (other.gameObject.CompareTag("pickUpObject"))
        {
            floatingItem = null;
        }
    }

}
