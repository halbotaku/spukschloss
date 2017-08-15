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

    //spawnpositions of the items
    public GameObject spawnPos;

    //arrays remembering the spawned objects
    private ArrayList spiderS, purseS, towelS, seedS, flyerS, eggS, matS, underwearS, ringS;

    // Use this for initialization
    void Start()
    {
        //intialize the counting arrayLists
        spiderS = new ArrayList();
        purseS = new ArrayList();
        towelS = new ArrayList();
        seedS = new ArrayList();
        flyerS = new ArrayList();
        eggS = new ArrayList();
        matS = new ArrayList();
        underwearS = new ArrayList();
        ringS = new ArrayList();

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
        pickUpInfo.myName = defaultPickUpInfo.myName;

        //create the ModeScript component and add the according properties
        pickUp.AddComponent<ModeScript>();
        ModeScript modeScript = pickUp.GetComponent<ModeScript>();
        ModeScript defaultModeScript = spawnList[number].GetComponent<ModeScript>();

        modeScript.pickUpMode = defaultModeScript.pickUpMode;
        modeScript.interactionMode = defaultModeScript.interactionMode;
        modeScript.pickUpGlow = defaultModeScript.pickUpGlow;
        modeScript.interactionGlow = defaultModeScript.interactionGlow;


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

        //create the Glow & add it as a child to the pickUp
        GameObject glow = new GameObject();
        glow.transform.parent = pickUp.transform;

        //Create Spriterenderer and add properties
        glow.AddComponent<SpriteRenderer>();
        GameObject defaultGlow = spawnList[number].transform.GetChild(1).gameObject;

        SpriteRenderer glowRenderer = glow.GetComponent<SpriteRenderer>();
        SpriteRenderer defaultGlowRenderer = defaultGlow.GetComponent<SpriteRenderer>();

        glowRenderer.sprite = defaultGlowRenderer.sprite;

        glowRenderer.material = defaultGlowRenderer.material;
        glowRenderer.sortingLayerID = defaultGlowRenderer.sortingLayerID;
        glowRenderer.sortingLayerName = defaultGlowRenderer.sortingLayerName;
        glowRenderer.sortingOrder = defaultGlowRenderer.sortingOrder;
        glow.transform.localPosition = defaultGlow.transform.localPosition;
        glow.transform.localScale = defaultGlow.transform.localScale;

        //create the glowScript and add properties
        glow.AddComponent<GlowEffekt>();
        GlowEffekt glowing = glow.GetComponent<GlowEffekt>();

        //set the position of the spawned item
        switch (pickUpInfo.myName)
        {
            case "spider":
                spawnSpider(pickUp);
                break;

            case "purse":
                spawnPurTelCanBell(pickUp);
                break;

            case "towel":
                spawnTowel(pickUp);
                break;

            case "telephone":
                spawnPurTelCanBell(pickUp);
                break;

            case "seed":
                spawnSeed(pickUp);
                break;

            case "candle":
                spawnPurTelCanBell(pickUp);
                break;

            case "bell":
                spawnPurTelCanBell(pickUp);
                break;

            case "flyer":
                spawnFlyer(pickUp);
                break;

            case "mat":
                spawnMatt(pickUp);
                break;

            case "eggs":
                spawnEggsPanTurkey(pickUp);
                break;

            case "pan":
                spawnEggsPanTurkey(pickUp);
                break;

            case "turkey":
                spawnEggsPanTurkey(pickUp);
                break;

            case "underwear":
                spawnUnderwear(pickUp);
                break;

            case "lifesaverring":
                spawnRingMegaphone(pickUp);
                break;

            case "megaphone":
                spawnRingMegaphone(pickUp);
                break;

            default:
                Debug.Log("my name nicht gefunden????");
                break;

        }
    }

    private void spawnSpider(GameObject pickUp)
    {
        //get the spider spawn positions
        GameObject spiderPos = spawnPos.transform.GetChild(0).gameObject;

        //pick one of the spawn positions
        int pick;

        do { pick = rnd.Next(0, spiderPos.transform.childCount); }
        while (spiderS.Contains(pick));

            //set the position to the chosen spawn point
            switch (pick)
            {
                case 0:
                    pickUp.transform.position = spiderPos.transform.GetChild(0).position;
                    spiderS.Add(pick);
                    break;

                case 1:
                    pickUp.transform.position = spiderPos.transform.GetChild(1).position;
                    spiderS.Add(pick);
                    break;

                case 2:
                    pickUp.transform.position = spiderPos.transform.GetChild(2).position;
                    spiderS.Add(pick);
                    break;

                case 3:
                    pickUp.transform.position = spiderPos.transform.GetChild(3).position;
                    spiderS.Add(pick);
                    break;

                case 4:
                    pickUp.transform.position = spiderPos.transform.GetChild(4).position;
                    spiderS.Add(pick);
                    break;

                case 5:
                    pickUp.transform.position = spiderPos.transform.GetChild(5).position;
                    spiderS.Add(pick);
                    break;

                case 6:
                    pickUp.transform.position = spiderPos.transform.GetChild(6).position;
                    spiderS.Add(pick);
                    break;

                case 7:
                    pickUp.transform.position = spiderPos.transform.GetChild(7).position;
                    spiderS.Add(pick);
                    break;

                case 8:
                    pickUp.transform.position = spiderPos.transform.GetChild(8).position;
                    spiderS.Add(pick);
                    break;

                case 9:
                    pickUp.transform.position = spiderPos.transform.GetChild(9).position;
                    spiderS.Add(pick);
                    break;

            default:
                    Debug.Log("Beim Spawn der Spinne ist etwas schief gelaufen!");
                    break;
            }
    }

    private void spawnPurTelCanBell(GameObject pickUp)
    {
        //get the spider spawn positions
        GameObject newSpawnPos = spawnPos.transform.GetChild(1).gameObject;

        //pick one of the spawn positions
        int pick;

        do { pick = rnd.Next(0, newSpawnPos.transform.childCount); }
        while (purseS.Contains(pick));

        //set the position to the chosen spawn point
        switch (pick)
        {
            case 0:
                pickUp.transform.position = newSpawnPos.transform.GetChild(0).position;
                purseS.Add(pick);
                break;

            case 1:
                pickUp.transform.position = newSpawnPos.transform.GetChild(1).position;
                purseS.Add(pick);
                break;

            case 2:
                pickUp.transform.position = newSpawnPos.transform.GetChild(2).position;
                purseS.Add(pick);
                break;

            case 3:
                pickUp.transform.position = newSpawnPos.transform.GetChild(3).position;
                purseS.Add(pick);
                break;

            case 4:
                pickUp.transform.position = newSpawnPos.transform.GetChild(4).position;
                purseS.Add(pick);
                break;

            case 5:
                pickUp.transform.position = newSpawnPos.transform.GetChild(5).position;
                purseS.Add(pick);
                break;

            case 6:
                pickUp.transform.position = newSpawnPos.transform.GetChild(6).position;
                purseS.Add(pick);
                break;

            case 7:
                pickUp.transform.position = newSpawnPos.transform.GetChild(7).position;
                purseS.Add(pick);
                break;

            case 8:
                pickUp.transform.position = newSpawnPos.transform.GetChild(8).position;
                purseS.Add(pick);
                break;

            case 9:
                pickUp.transform.position = newSpawnPos.transform.GetChild(9).position;
                purseS.Add(pick);
                break;

            case 10:
                pickUp.transform.position = newSpawnPos.transform.GetChild(10).position;
                purseS.Add(pick);
                break;

            case 11:
                pickUp.transform.position = newSpawnPos.transform.GetChild(11).position;
                purseS.Add(pick);
                break;

            case 12:
                pickUp.transform.position = newSpawnPos.transform.GetChild(12).position;
                purseS.Add(pick);
                break;

            case 13:
                pickUp.transform.position = newSpawnPos.transform.GetChild(13).position;
                purseS.Add(pick);
                break;

            case 14:
                pickUp.transform.position = newSpawnPos.transform.GetChild(14).position;
                purseS.Add(pick);
                break;

            case 15:
                pickUp.transform.position = newSpawnPos.transform.GetChild(15).position;
                purseS.Add(pick);
                break;

            case 16:
                pickUp.transform.position = newSpawnPos.transform.GetChild(16).position;
                purseS.Add(pick);
                break;

            case 17:
                pickUp.transform.position = newSpawnPos.transform.GetChild(17).position;
                purseS.Add(pick);
                break;

            case 18:
                pickUp.transform.position = newSpawnPos.transform.GetChild(18).position;
                purseS.Add(pick);
                break;

            case 19:
                pickUp.transform.position = newSpawnPos.transform.GetChild(19).position;
                purseS.Add(pick);
                break;

            case 20:
                pickUp.transform.position = newSpawnPos.transform.GetChild(20).position;
                purseS.Add(pick);
                break;

            case 21:
                pickUp.transform.position = newSpawnPos.transform.GetChild(21).position;
                purseS.Add(pick);
                break;

            case 22:
                pickUp.transform.position = newSpawnPos.transform.GetChild(22).position;
                purseS.Add(pick);
                break;

            case 23:
                pickUp.transform.position = newSpawnPos.transform.GetChild(23).position;
                purseS.Add(pick);
                break;

            case 24:
                pickUp.transform.position = newSpawnPos.transform.GetChild(24).position;
                purseS.Add(pick);
                break;

            case 25:
                pickUp.transform.position = newSpawnPos.transform.GetChild(25).position;
                purseS.Add(pick);
                break;

            case 26:
                pickUp.transform.position = newSpawnPos.transform.GetChild(26).position;
                purseS.Add(pick);
                break;

            case 27:
                pickUp.transform.position = newSpawnPos.transform.GetChild(27).position;
                purseS.Add(pick);
                break;

            case 28:
                pickUp.transform.position = newSpawnPos.transform.GetChild(28).position;
                purseS.Add(pick);
                break;

            case 29:
                pickUp.transform.position = newSpawnPos.transform.GetChild(29).position;
                purseS.Add(pick);
                break;

            case 30:
                pickUp.transform.position = newSpawnPos.transform.GetChild(30).position;
                purseS.Add(pick);
                break;

            default:
                Debug.Log("Beim Spawn der von Klingel, Telefon, Kerze oder Tasche ist etwas schief gelaufen!");
                break;
        }
    }


    private void spawnTowel(GameObject pickUp)
    {
        //get the spider spawn positions
        GameObject newSpawnPos = spawnPos.transform.GetChild(2).gameObject;

        //pick one of the spawn positions
        int pick;

        do { pick = rnd.Next(0, newSpawnPos.transform.childCount); }
        while (towelS.Contains(pick));

        //set the position to the chosen spawn point
        switch (pick)
        {
            case 0:
                pickUp.transform.position = newSpawnPos.transform.GetChild(0).position;
                towelS.Add(pick);
                break;

            case 1:
                pickUp.transform.position = newSpawnPos.transform.GetChild(1).position;
                towelS.Add(pick);
                break;

            case 2:
                pickUp.transform.position = newSpawnPos.transform.GetChild(2).position;
                towelS.Add(pick);
                break;

            case 3:
                pickUp.transform.position = newSpawnPos.transform.GetChild(3).position;
                towelS.Add(pick);
                break;

            case 4:
                pickUp.transform.position = newSpawnPos.transform.GetChild(4).position;
                towelS.Add(pick);
                break;

            case 5:
                pickUp.transform.position = newSpawnPos.transform.GetChild(5).position;
                towelS.Add(pick);
                break;

            case 6:
                pickUp.transform.position = newSpawnPos.transform.GetChild(6).position;
                towelS.Add(pick);
                break;

            case 7:
                pickUp.transform.position = newSpawnPos.transform.GetChild(7).position;
                towelS.Add(pick);
                break;

            case 8:
                pickUp.transform.position = newSpawnPos.transform.GetChild(8).position;
                towelS.Add(pick);
                break;

            case 9:
                pickUp.transform.position = newSpawnPos.transform.GetChild(9).position;
                towelS.Add(pick);
                break;

            case 10:
                pickUp.transform.position = newSpawnPos.transform.GetChild(10).position;
                towelS.Add(pick);
                break;

            case 11:
                pickUp.transform.position = newSpawnPos.transform.GetChild(11).position;
                towelS.Add(pick);
                break;

            default:
                Debug.Log("Beim Spawn des Handtuchs ist etwas schief gelaufen!");
                break;
        }
    }

    private void spawnSeed(GameObject pickUp)
    {
        //get the spider spawn positions
        GameObject newSpawnPos = spawnPos.transform.GetChild(3).gameObject;

        //pick one of the spawn positions
        int pick;

        do { pick = rnd.Next(0, newSpawnPos.transform.childCount); }
        while (seedS.Contains(pick));

        //set the position to the chosen spawn point
        switch (pick)
        {
            case 0:
                pickUp.transform.position = newSpawnPos.transform.GetChild(0).position;
                seedS.Add(pick);
                break;

            case 1:
                pickUp.transform.position = newSpawnPos.transform.GetChild(1).position;
                seedS.Add(pick);
                break;

            case 2:
                pickUp.transform.position = newSpawnPos.transform.GetChild(2).position;
                seedS.Add(pick);
                break;

            case 3:
                pickUp.transform.position = newSpawnPos.transform.GetChild(3).position;
                seedS.Add(pick);
                break;

            case 4:
                pickUp.transform.position = newSpawnPos.transform.GetChild(4).position;
                seedS.Add(pick);
                break;

            case 5:
                pickUp.transform.position = newSpawnPos.transform.GetChild(5).position;
                seedS.Add(pick);
                break;

            case 6:
                pickUp.transform.position = newSpawnPos.transform.GetChild(6).position;
                seedS.Add(pick);
                break;

            case 7:
                pickUp.transform.position = newSpawnPos.transform.GetChild(7).position;
                seedS.Add(pick);
                break;

            case 8:
                pickUp.transform.position = newSpawnPos.transform.GetChild(8).position;
                seedS.Add(pick);
                break;

            case 9:
                pickUp.transform.position = newSpawnPos.transform.GetChild(9).position;
                seedS.Add(pick);
                break;

            default:
                Debug.Log("Beim Spawn des Samens ist etwas schief gelaufen!");
                break;
        }
    }

    private void spawnFlyer(GameObject pickUp)
    {
        //get the spider spawn positions
        GameObject newSpawnPos = spawnPos.transform.GetChild(4).gameObject;

        //pick one of the spawn positions
        int pick;

        do { pick = rnd.Next(0, newSpawnPos.transform.childCount); }
        while (flyerS.Contains(pick));

        //set the position to the chosen spawn point
        switch (pick)
        {
            case 0:
                pickUp.transform.position = newSpawnPos.transform.GetChild(0).position;
                flyerS.Add(pick);
                break;

            case 1:
                pickUp.transform.position = newSpawnPos.transform.GetChild(1).position;
                flyerS.Add(pick);
                break;

            case 2:
                pickUp.transform.position = newSpawnPos.transform.GetChild(2).position;
                flyerS.Add(pick);
                break;

            case 3:
                pickUp.transform.position = newSpawnPos.transform.GetChild(3).position;
                flyerS.Add(pick);
                break;

            case 4:
                pickUp.transform.position = newSpawnPos.transform.GetChild(4).position;
                flyerS.Add(pick);
                break;

            case 5:
                pickUp.transform.position = newSpawnPos.transform.GetChild(5).position;
                flyerS.Add(pick);
                break;

            case 6:
                pickUp.transform.position = newSpawnPos.transform.GetChild(6).position;
                flyerS.Add(pick);
                break;

            case 7:
                pickUp.transform.position = newSpawnPos.transform.GetChild(7).position;
                flyerS.Add(pick);
                break;

            case 8:
                pickUp.transform.position = newSpawnPos.transform.GetChild(8).position;
                flyerS.Add(pick);
                break;

            case 9:
                pickUp.transform.position = newSpawnPos.transform.GetChild(9).position;
                flyerS.Add(pick);
                break;

            default:
                Debug.Log("Beim Spawn des Flyers ist etwas schief gelaufen!");
                break;
        }
    }


    private void spawnEggsPanTurkey(GameObject pickUp)
    {
        //get the spider spawn positions
        GameObject newSpawnPos = spawnPos.transform.GetChild(5).gameObject;

        //pick one of the spawn positions
        int pick;

        do { pick = rnd.Next(0, newSpawnPos.transform.childCount); }
        while (flyerS.Contains(pick));

        //set the position to the chosen spawn point
        switch (pick)
        {
            case 0:
                pickUp.transform.position = newSpawnPos.transform.GetChild(0).position;
                eggS.Add(pick);
                break;

            case 1:
                pickUp.transform.position = newSpawnPos.transform.GetChild(1).position;
                eggS.Add(pick);
                break;

            case 2:
                pickUp.transform.position = newSpawnPos.transform.GetChild(2).position;
                eggS.Add(pick);
                break;

            case 3:
                pickUp.transform.position = newSpawnPos.transform.GetChild(3).position;
                eggS.Add(pick);
                break;

            case 4:
                pickUp.transform.position = newSpawnPos.transform.GetChild(4).position;
                eggS.Add(pick);
                break;

            default:
                Debug.Log("Beim Spawn ist etwas schief gelaufen!");
                break;
        }
    }

    private void spawnMatt(GameObject pickUp)
    {
        //get the spider spawn positions
        GameObject newSpawnPos = spawnPos.transform.GetChild(6).gameObject;

        //pick one of the spawn positions
        int pick;

        do { pick = rnd.Next(0, newSpawnPos.transform.childCount); }
        while (matS.Contains(pick));

        //set the position to the chosen spawn point
        switch (pick)
        {
            case 0:
                pickUp.transform.position = newSpawnPos.transform.GetChild(0).position;
                matS.Add(pick);
                break;

            case 1:
                pickUp.transform.position = newSpawnPos.transform.GetChild(1).position;
                matS.Add(pick);
                break;

            case 2:
                pickUp.transform.position = newSpawnPos.transform.GetChild(2).position;
                matS.Add(pick);
                break;

            case 3:
                pickUp.transform.position = newSpawnPos.transform.GetChild(3).position;
                matS.Add(pick);
                break;

            case 4:
                pickUp.transform.position = newSpawnPos.transform.GetChild(4).position;
                matS.Add(pick);
                break;

            case 5:
                pickUp.transform.position = newSpawnPos.transform.GetChild(5).position;
                matS.Add(pick);
                break;

            case 6:
                pickUp.transform.position = newSpawnPos.transform.GetChild(6).position;
                matS.Add(pick);
                break;

            case 7:
                pickUp.transform.position = newSpawnPos.transform.GetChild(7).position;
                matS.Add(pick);
                break;

            case 8:
                pickUp.transform.position = newSpawnPos.transform.GetChild(8).position;
                matS.Add(pick);
                break;

            case 9:
                pickUp.transform.position = newSpawnPos.transform.GetChild(9).position;
                matS.Add(pick);
                break;

            default:
                Debug.Log("Beim Spawn der Matte ist etwas schief gelaufen!");
                break;
        }
    }

    private void spawnUnderwear(GameObject pickUp)
    {
        //get the spider spawn positions
        GameObject newSpawnPos = spawnPos.transform.GetChild(7).gameObject;

        //pick one of the spawn positions
        int pick;

        do { pick = rnd.Next(0, newSpawnPos.transform.childCount); }
        while (underwearS.Contains(pick));

        //set the position to the chosen spawn point
        switch (pick)
        {
            case 0:
                pickUp.transform.position = newSpawnPos.transform.GetChild(0).position;
                underwearS.Add(pick);
                break;

            case 1:
                pickUp.transform.position = newSpawnPos.transform.GetChild(1).position;
                underwearS.Add(pick);
                break;

            case 2:
                pickUp.transform.position = newSpawnPos.transform.GetChild(2).position;
                underwearS.Add(pick);
                break;

            case 3:
                pickUp.transform.position = newSpawnPos.transform.GetChild(3).position;
                underwearS.Add(pick);
                break;

            case 4:
                pickUp.transform.position = newSpawnPos.transform.GetChild(4).position;
                underwearS.Add(pick);
                break;

            case 5:
                pickUp.transform.position = newSpawnPos.transform.GetChild(5).position;
                underwearS.Add(pick);
                break;

            case 6:
                pickUp.transform.position = newSpawnPos.transform.GetChild(6).position;
                underwearS.Add(pick);
                break;

            case 7:
                pickUp.transform.position = newSpawnPos.transform.GetChild(7).position;
                underwearS.Add(pick);
                break;

            case 8:
                pickUp.transform.position = newSpawnPos.transform.GetChild(8).position;
                underwearS.Add(pick);
                break;

            case 9:
                pickUp.transform.position = newSpawnPos.transform.GetChild(9).position;
                underwearS.Add(pick);
                break;

            case 10:
                pickUp.transform.position = newSpawnPos.transform.GetChild(10).position;
                underwearS.Add(pick);
                break;

            default:
                Debug.Log("Beim Spawn der Unterwäsche ist etwas schief gelaufen!");
                break;
        }
    }

    private void spawnRingMegaphone(GameObject pickUp)
    {
        //get the spider spawn positions
        GameObject newSpawnPos = spawnPos.transform.GetChild(8).gameObject;

        //pick one of the spawn positions
        int pick;

        do { pick = rnd.Next(0, newSpawnPos.transform.childCount); }
        while (seedS.Contains(pick));

        //set the position to the chosen spawn point
        switch (pick)
        {
            case 0:
                pickUp.transform.position = newSpawnPos.transform.GetChild(0).position;
                ringS.Add(pick);
                break;

            case 1:
                pickUp.transform.position = newSpawnPos.transform.GetChild(1).position;
                ringS.Add(pick);
                break;

            default:
                Debug.Log("Beim Spawn des Rings und Megaphons ist etwas schief gelaufen!");
                break;
        }
    }
}