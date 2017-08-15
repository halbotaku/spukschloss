using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOut : MonoBehaviour
{

    //SpriteRenderer controlling the fake hotel owner
    private GameObject spriteObject;
    private SpriteRenderer sprite;
    GlowEffekt glow;

    //booloeans controlling the checkout visual effects
    private bool isHovering;
    [HideInInspector] public bool isCheckingOut;

    //referencing the player and hotelowner
    public GameObject player;
    public GameObject hotelOwner;

    private float checkOutCounter = 0;

    // Use this for initialization
    void Start()
    {
        //reference the sprite properties
        spriteObject = gameObject.transform.GetChild(0).gameObject;
        sprite = spriteObject.GetComponent<SpriteRenderer>();


        //set the fake hotelOwner to invisible
        spriteObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (checkOutCounter > 0)
        {
            checkOutCounter = checkOutCounter - Time.deltaTime;
        }

        //when the ghost hovers over the area TO DO: while the hotelowner is gone
        if (isHovering && hotelOwner.transform.position.x < -3.158993 || isHovering && hotelOwner.transform.position.x > 3.218313 ||
            isHovering && hotelOwner.transform.position.y > -4.008584 || isHovering && hotelOwner.transform.position.y < -6.038051)
        {
            sprite.color = new Color(0.3f, 0.8f, 1f, 0.7f);
            spriteObject.SetActive(true);

            if (Input.GetButtonDown("pickUp"))
            {
                isCheckingOut = true;
                checkOutCounter = 5;
            }
        }

        if (checkOutCounter <= 0)
        {
            isCheckingOut = false;
        }

        if (!isHovering && !isCheckingOut)
        {
            spriteObject.SetActive(false);
        }

        if (isCheckingOut)
        {
            sprite.color = new Color(0.3f, 0.7f, 1f, 1f);
            spriteObject.SetActive(true);
        }

        //when the hotelOwner returns
        if (hotelOwner.transform.position.x > -3.158993 && hotelOwner.transform.position.x < 3.218313 &&
            hotelOwner.transform.position.y < -4.008584 && hotelOwner.transform.position.y > -6.038051)
        {
            isCheckingOut = false;
        }
    }

    //when beginning to hover over the reception area
    void OnTriggerEnter2D(Collider2D other)
    {
            isHovering = true;
    }

    //when staying on the reception area
    void OnTriggerStay2D(Collider2D other)
    {
        {
                isHovering = true;
        }
    }

    //when leaving the reception area
    void OnTriggerExit2D(Collider2D other)
    {
        {
                isHovering = false;
        }
    }
}
