using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{

    //List containing the Objects spawning at the start of the level
    private GameObject[] spawnList;

    //private variable for generating random numbers
    System.Random rnd = new System.Random();

    //List containing all the pickUpItems available
    public GameObject[] pickUpItemList;

    //amount of items the game shall offer the player in the beginning
    public int itemAmount;

    //size of the spawn-area
    public int spawnHeight;
    public int spawnWidth;

    // Use this for initialization
    void Start()
    {
        //count the amount of pickUp objects we have
        int amount = pickUpItemList.Length;

        //instanciate the GameObjectArray SpawnList
        spawnList = new GameObject[amount];

        //go through the pickUp Items
        for (int i = 0; i < amount; i++)
        {
            //add the item to the SpawnList
            spawnList[i] = pickUpItemList[i];
        }

        //if you want to spawn more items than different kinds of pickUp Objects
        if (amount < itemAmount || amount == itemAmount)
        {
            //Spawn each Item at least once
            for (int i = 0; i < amount; i++)
            {
                count(i);
            }

            int rest = itemAmount - amount;

            //spawn the rest of items randomly
            for (int i = 0; i < rest; i++)
            {
                //pick a random pick up from the list
                int number = rnd.Next(0, amount);

                count(number);
            }
        }
        else
        {
            //spawn all the items randomly
            for (int i = 0; i < itemAmount; i++)
            {
                //pick a random pick up from the list
                int number = rnd.Next(0, amount);

                count(number);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void count(int number)
    {
        //create the Object
        GameObject pickUp = new GameObject();

        //set the properties
        pickUp.tag = "pickUpObject";

        //create the collider component and add the according properties
        pickUp.AddComponent<CircleCollider2D>();
        CircleCollider2D pickUpCollider = pickUp.GetComponent<CircleCollider2D>();
        CircleCollider2D defaultCollider = spawnList[number].GetComponent<CircleCollider2D>();
        pickUpCollider.radius = defaultCollider.radius;

        //create the pickUpInfo component and add the according properties
        pickUp.AddComponent<PickUpInfo>();
        PickUpInfo pickUpInfo = pickUp.GetComponent<PickUpInfo>();
        PickUpInfo defaultPickUpInfo = spawnList[number].GetComponent<PickUpInfo>();

        pickUpInfo.magnifyIcon = defaultPickUpInfo.magnifyIcon;
        pickUpInfo.reactionIcon = defaultPickUpInfo.reactionIcon;

        //create the ModeScript component and add the according properties
        pickUp.AddComponent<ModeScript>();
        ModeScript modeScript = pickUp.GetComponent<ModeScript>();
        ModeScript defaultModeScript = spawnList[number].GetComponent<ModeScript>();

        modeScript.pickUpMode = defaultModeScript.pickUpMode;
        modeScript.interactionMode = defaultModeScript.interactionMode;


        //create the Sprite & add it as a child to the pickUp
        GameObject sprite = new GameObject();

        sprite.transform.parent = pickUp.transform;

        //create the SpriteRenderer & add properties
        sprite.AddComponent<SpriteRenderer>();

        GameObject defaultSprite = spawnList[number].transform.GetChild(0).gameObject;

        SpriteRenderer spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        SpriteRenderer defaultSpriteRenderer = defaultSprite.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = defaultSpriteRenderer.sprite;
        spriteRenderer.material = defaultSpriteRenderer.material;
        spriteRenderer.sortingLayerID = defaultSpriteRenderer.sortingLayerID;
        spriteRenderer.sortingLayerName = defaultSpriteRenderer.sortingLayerName;
        spriteRenderer.sortingOrder = defaultSpriteRenderer.sortingOrder;

        //create the SpriteOutline component & add properties
        sprite.AddComponent<SpriteOutline>();

        SpriteOutline defaultOutline = defaultSprite.GetComponent<SpriteOutline>();
        SpriteOutline outline = sprite.GetComponent<SpriteOutline>();

        outline.enabled = defaultOutline.enabled;
        outline.color = defaultOutline.color;
        outline.outlineSize = defaultOutline.outlineSize;

        //set the position of the spawned item
        pickUp.transform.position = new Vector3(rnd.Next(-spawnWidth, spawnWidth + 1), rnd.Next(- spawnHeight, spawnHeight + 1));
    }

}