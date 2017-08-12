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
    private GameObject hoverItem = null;
    private Boolean isHovering = false;

    private SpecialItem item;

    //countdown for special slime item effects
    private Color origColor;
    private float slimeCount;
    private bool isSliming;
    private Vector2 tempSpeed;
    private Vector2 newSpeed;
    private SpriteRenderer sprite;
    private SpriteRenderer spriteA;
    private SpriteRenderer spriteB;
    private SpriteRenderer spriteC;
    private GameObject shadowA;
    private GameObject shadowB;
    private GameObject shadowC;
    private float counterA;
    private float counterB;
    private float counterC;

    //countdown for special banana item effects
    private bool isBananaing;
    private pathFollower hotelOwner;
    private float bananaCount;

    //countdown for special scream item effects
    private bool isScreaming;
    private float screamCount;
    private float temp;
    private float pulsingEffect;
    private int number = 0;
    public string victim;


    // Use this for initialization
    void Start()
    {
        counterA = 1;
        counterB = counterC = 0;

        player = GetComponent<PlayerScript>();
        tempSpeed = player.speed;
        isSliming = false;

        sprite = GetComponentInChildren<SpriteRenderer>();

        shadowA = new GameObject();
        shadowA.AddComponent<SpriteRenderer>();
        spriteA = shadowA.GetComponent<SpriteRenderer>();
        spriteA.sprite = sprite.sprite;
        spriteA.sortingLayerID = sprite.sortingLayerID;
        spriteA.sortingLayerName = sprite.sortingLayerName;
        spriteA.sortingOrder = sprite.sortingOrder - 1;
        shadowA.SetActive(false);

        shadowB = new GameObject();
        shadowB.AddComponent<SpriteRenderer>();
        spriteB = shadowB.GetComponent<SpriteRenderer>();
        spriteB.sprite = sprite.sprite;
        spriteB.sortingLayerID = sprite.sortingLayerID;
        spriteB.sortingLayerName = sprite.sortingLayerName;
        spriteB.sortingOrder = sprite.sortingOrder - 2;
        shadowB.SetActive(false);

        shadowC = new GameObject();
        shadowC.AddComponent<SpriteRenderer>();
        spriteC = shadowB.GetComponent<SpriteRenderer>();
        spriteC.sprite = sprite.sprite;
        spriteC.sortingLayerID = sprite.sortingLayerID;
        spriteC.sortingLayerName = sprite.sortingLayerName;
        spriteC.sortingOrder = sprite.sortingOrder - 2;
        shadowC.SetActive(false);

        //reference the hotelowner
        hotelOwner = GameObject.FindGameObjectWithTag("hotelOwner").GetComponent<pathFollower>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (floatingItem != null)
        {
            //set hovering to false
            isHovering = false;

            if (floatingItem != null)
            {
                item = floatingItem.GetComponent<SpecialItem>();

                slimeCount = item.speedDuration;

                if (item.myName == "slime")
                {
                    isSliming = true;
                }

                if (item.myName == "banana")
                {
                    isBananaing = true;
                }

                if (item.myName == "scream")
                {
                    isScreaming = true;
                }
            }
        }

        if (isSliming == true)
        {
            slime();
        }

        if (isBananaing == true)
        {
            banana();
        }

        if (isScreaming == true)
        {
            scream();
        }
    }

    //Method for picking up pickUpObjects when player is colliding with them
    void OnTriggerEnter2D(Collider2D other)
    {
        //if the object is of the tag pickup Object
        if (other.gameObject.CompareTag("specialObject") && other.gameObject != followPlayer)
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

    private void slime()
    {
        Destroy(floatingItem);

        sprite.color = new Color(0.5f, 0.7f, 1f, 1f);

        player.speed = item.newSpeed;

        if (counterA > 0)
        {
            if (shadowA.activeInHierarchy == false)
            {
                shadowA.SetActive(true);
                shadowA.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            }

            counterA = counterA - 8 * Time.deltaTime;
            spriteA.color = new Color(0.5f, 0.7f, 1f, counterA);
        }

        if (counterA < 0 && shadowA.activeInHierarchy == true)
        {
            shadowA.SetActive(false);
            counterB = 1;
            counterA = 0;
            counterC = 0;
        }

        if (counterB > 0)
        {
            if (shadowB.activeInHierarchy == false)
            {
                shadowB.SetActive(true);
                shadowB.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            }

            counterB = counterB - 8 * Time.deltaTime;
            spriteB.color = new Color(0.5f, 0.7f, 1f, counterB);
        }

        if (counterB < 0 && shadowB.activeInHierarchy == true)
        {
            shadowB.SetActive(false);
            counterC = 1;
            counterA = 0;
            counterB = 0;
        }

        if (counterC > 0)
        {
            if (shadowC.activeInHierarchy == false)
            {
                shadowC.SetActive(true);
                shadowC.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            }

            counterC = counterC - 8 * Time.deltaTime;
            spriteC.color = new Color(0.5f, 0.7f, 1f, counterC);
        }

        if (counterC < 0 && shadowC.activeInHierarchy == true)
        {
            shadowC.SetActive(false);
            counterA = 1;
            counterB = 0;
            counterC = 0;
        }


        if (slimeCount > 0)
        {
            slimeCount = slimeCount - Time.deltaTime;
        }

        if (slimeCount < 0)
        {
            player.speed = tempSpeed;
            isSliming = false;
            sprite.color = new Color(1f, 1f, 1f, 1f);

            shadowA.SetActive(false);
            shadowB.SetActive(false);
            shadowC.SetActive(false);
        }

        if (player.movement.x > 0)
        {
            spriteA.flipX = false;
            spriteB.flipX = false;
            spriteC.flipX = false;
        }
        else
        {
            spriteA.flipX = true;
            spriteB.flipX = true;
            spriteC.flipX = true;
        }
    }

    private void banana()
    {
        sprite.color = new Color(1f, 0.6f, 0.4f, 1f);
        if (hotelOwner.isSlipping == false)
        {
            hotelOwner.isSlipping = true;
            bananaCount = floatingItem.GetComponent<SpecialItem>().slipRecoveryDuration;
            Destroy(floatingItem);
        }

        if (bananaCount > 0)
        {
            bananaCount = bananaCount - Time.deltaTime;
        }

        if (bananaCount < 0)
        {
            hotelOwner.isSlipping = false;
            isBananaing = false;
        }
    }

    private void scream()
    {

        if (floatingItem != null)
        {
           temp = floatingItem.GetComponent<SpecialItem>().timeUntilScreamExplodes;
        }

        if (screamCount == 0)
        {
            screamCount = temp;
            Destroy(floatingItem);
        }

        sprite.color = new Color(1f, 1f, 0.5f, 1f);

        if (screamCount>0)
        {
            screamCount = screamCount - Time.deltaTime;

            float scale = (1 + (1 - (screamCount / temp)))/1.3f;

            if (scale < 1)
            {
                scale = 1;            }

            switch (number)
            {
                case 0:
                    pulsingEffect = 1.1f;
                    number++;
                    break;
                case 1:
                    number++;
                    break;
                case 2:
                    number++;
                    break;
                case 3:
                    number++;
                    break;
                case 4:
                    number++;
                    break;
                case 5:
                    number++;
                    break;
                case 6:
                    number++;
                    break;
                case 7:
                    number++;
                    break;
                case 8:
                    number++;
                    break;
                case 9:
                    pulsingEffect = 1.0f;
                    number++;
                    break;
                case 10:
                    number++;
                    break;
                case 11:
                    number=0;
                    break;
                case 12:
                    number++;
                    break;
                case 13:
                    number++;
                    break;
                case 14:
                    number++;
                    break;
                case 15:
                    number++;
                    break;
                case 16:
                    number++;
                    break;
                case 17:
                    number=0;
                    break;
            }

            gameObject.transform.localScale = new Vector2(scale * pulsingEffect,scale * pulsingEffect);
        }

        if (screamCount < 0)
        {
            sprite.color = new Color(1f, 0.7f, 0.0f, 1f);

            //determine your position in the house
            if (transform.position.x >= -8.496668 && transform.position.x <= -4.601481 && transform.position.y <= 1.985401 && transform.position.y >= 0.5680488)
            {
                victim = "RTL1";
            }

            if (transform.position.x >= -4.264231 && transform.position.x <= -0.2650651 && transform.position.y <= 1.985401 && transform.position.y >= 0.5680488)
            {
                victim = "RTL2";
            }
            if (transform.position.x >= 0.04048658 && transform.position.x <= 3.863564 && transform.position.y <= 1.985401 && transform.position.y >= 0.5680488)
            {
                victim = "RTR1";
            }
            if (transform.position.x >= 4.067644 && transform.position.x <= 8.207177 && transform.position.y <= 1.985401 && transform.position.y >= 0.5680488)
            {
                victim = "RTR1";
            }
            if (transform.position.x >= -8.506353 && transform.position.x <= -3.603068 && transform.position.y <= -1.798505 && transform.position.y >= -3.638129)
            {
                victim = "pool";
            }
            if (transform.position.x >= -3.265077 && transform.position.x <= -1.407769 && transform.position.y <= -1.798505 && transform.position.y >= -3.638129)
            {
                victim = "RL";
            }
            if (transform.position.x >= -1.069247 && transform.position.x <= 0.744544 && transform.position.y <= -1.798505 && transform.position.y >= -3.638129)
            {
                victim = "RC";
            }
            if (transform.position.x >= 1.105385 && transform.position.x <= 2.900892 && transform.position.y <= -1.798505 && transform.position.y >= -3.638129)
            {
                victim = "RR";
            }
            if (transform.position.x >= 3.347824 && transform.position.x <= 8.102373 && transform.position.y <= -1.798505 && transform.position.y >= -3.638129)
            {
                victim = "kitchen";
            }
            if (transform.position.x >= -3.265077 && transform.position.x <= -6.141697 && transform.position.y <= -4.070966 && transform.position.y >= -5.938351)
            {
                victim = "RL1";
            }
            if (transform.position.x >= -5.725357 && transform.position.x <= -3.569612 && transform.position.y <= -4.070966 && transform.position.y >= -5.938351)
            {
                victim = "RL2";
            }
            if (transform.position.x >= 3.364027 && transform.position.x <= -3.569612 && transform.position.y <= -4.070966 && transform.position.y >= -5.938351)
            {
                victim = "RR1";
            }
            if (transform.position.x >= 5.891629 && transform.position.x <= 8.20326 && transform.position.y <= -4.070966 && transform.position.y >= -5.938351)
            {
                victim = "RR2";
            }
            isScreaming = false;
            gameObject.transform.localScale = new Vector2(1, 1);
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}