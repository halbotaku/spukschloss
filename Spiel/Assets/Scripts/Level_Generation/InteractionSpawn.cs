﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSpawn : MonoBehaviour {

    //GameObject Array holding the spawnable interaction Objects
    public GameObject[] spawnPool;

    //GameObject Array holding the spawnPosition-Objects
    public GameObject[] spawnPositions;

    //private variable for generating random numbers
    System.Random rnd = new System.Random();

    //private variable remembering the randomly picked object
    private GameObject pickedInteraction;

    // Use this for initialization
    public void Awake() {

        //go through all the spawn Positions
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            //reference the current spawn position
            RoomPositionInteraction info = spawnPositions[i].GetComponent<RoomPositionInteraction>();

            //pick a random spawnable object out of the info list
            int number = rnd.Next(0, info.spawnableObjects.Length);

            //get the name we want to compare the object to
            string compareName = info.spawnableObjects[number];

            int index = 0;
            InteractionList list = spawnPool[index].GetComponent<InteractionList>();

            while (compareName != list.myName)
            {
                index++;
                list = spawnPool[index].GetComponent<InteractionList>();
            }

            pickedInteraction = spawnPool[index];

            if (pickedInteraction)
            {
                spawnPositions[i].transform.position = new Vector3(0, 0);

                //create the circle collider component and add the according properties
                spawnPositions[i].AddComponent<CircleCollider2D>();
                CircleCollider2D collider = spawnPositions[i].GetComponent<CircleCollider2D>();
                CircleCollider2D defaultCollider = pickedInteraction.GetComponent<CircleCollider2D>();

                collider.radius = defaultCollider.radius;
                collider.offset = defaultCollider.offset;

                //create the modescript component and add the according properties
                spawnPositions[i].AddComponent<ModeScript>();
                ModeScript mode = spawnPositions[i].GetComponent<ModeScript>();
                ModeScript defaultMode = pickedInteraction.GetComponent<ModeScript>();

                mode.pickUpMode = defaultMode.pickUpMode;
                mode.interactionMode = defaultMode.interactionMode;
                mode.pickUpGlow = defaultMode.pickUpGlow;
                mode.interactionGlow = defaultMode.interactionGlow;

                //create the interaction list component and add the according properties
                spawnPositions[i].AddComponent<InteractionList>();
                InteractionList spawnList = spawnPositions[i].GetComponent<InteractionList>();
                InteractionList defaultList = pickedInteraction.GetComponent<InteractionList>();

                spawnList.position = info.myPosition;

                spawnList.myName = defaultList.myName;
                spawnList.hotelOwner = defaultList.hotelOwner;
                spawnList.reactingGuest = info.myReactingGuest;

                spawnList.pickUpList = new string[defaultList.pickUpList.Length];
                spawnList.animationList = new string[defaultList.pickUpList.Length];
                spawnList.gradeOfDamage = new string[defaultList.pickUpList.Length];
                spawnList.inRoomWaitingList = new float[defaultList.pickUpList.Length];
                spawnList.atReceptionWaitingList = new float[defaultList.pickUpList.Length];
                spawnList.repairTime = new float[defaultList.pickUpList.Length];

                index = 0;
                while (index < defaultList.pickUpList.Length)
                {
                    spawnList.pickUpList[index] = defaultList.pickUpList[index];
                    spawnList.animationList[index] = defaultList.animationList[index];
                    spawnList.gradeOfDamage[index] = defaultList.gradeOfDamage[index];
                    spawnList.inRoomWaitingList[index] = defaultList.inRoomWaitingList[index];
                    spawnList.atReceptionWaitingList[index] = defaultList.atReceptionWaitingList[index];
                    spawnList.repairTime[index] = defaultList.repairTime[index];

                    index++;
                }

                //add other properties
                spawnList.hasBeenInteractedWith = defaultList.hasBeenInteractedWith;
                spawnList.index = defaultList.index;

                //create the Sprite & add it as a child to the pickUp
                GameObject sprite = new GameObject();
                sprite.transform.parent = spawnPositions[i].transform;

                //Create Spriterenderer and add properties
                sprite.AddComponent<SpriteRenderer>();
                GameObject defaultSprite = pickedInteraction.transform.GetChild(0).gameObject;

                SpriteRenderer spriteRenderer = sprite.GetComponent<SpriteRenderer>();
                SpriteRenderer defaultSpriteRenderer = defaultSprite.GetComponent<SpriteRenderer>();

                spriteRenderer.sprite = defaultSpriteRenderer.sprite;
                spriteRenderer.material = defaultSpriteRenderer.material;
                spriteRenderer.sortingLayerID = defaultSpriteRenderer.sortingLayerID;
                spriteRenderer.sortingLayerName = defaultSpriteRenderer.sortingLayerName;
                spriteRenderer.sortingOrder = defaultSpriteRenderer.sortingOrder;

                if (info.flipSprite)
                {
                    spriteRenderer.flipX = true;
                }

                //create the animator and add properties
                sprite.AddComponent<Animator>();
                Animator anim = sprite.GetComponent<Animator>();
                Animator defaultAnim = defaultSprite.GetComponent<Animator>();

                anim.runtimeAnimatorController = defaultAnim.runtimeAnimatorController as RuntimeAnimatorController;

                //create the Glow & add it as a child to the pickUp
                GameObject glow = new GameObject();
                glow.transform.parent = spawnPositions[i].transform;

                //Create Spriterenderer and add properties
                glow.AddComponent<SpriteRenderer>();
                GameObject defaultGlow = pickedInteraction.transform.GetChild(1).gameObject;

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

                //reset the item's position
                spawnPositions[i].transform.position = new Vector3(info.posx, info.posy);
            }
        }
	}
}