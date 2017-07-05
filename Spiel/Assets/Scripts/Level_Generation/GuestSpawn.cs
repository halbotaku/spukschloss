﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestSpawn : MonoBehaviour {

    public GameObject[] guestTypeList;
    public GameObject[] guestPositionList;

    //private variable for generating random numbers
    System.Random rnd = new System.Random();

    //private variable generating the portrait positions
    private int offset;

    // Use this for initialization
    void Start () {

        int isSpecialGuest = rnd.Next(0, guestPositionList.Length);
        offset = 1;

        //assign a look to the hotelGuest position Types
        for (int i = 0; i < guestPositionList.Length; i++)
        {
            pathArranger arranger = guestPositionList[i].GetComponent<pathArranger>();

            int number = rnd.Next(0, guestTypeList.Length);

            //create the Sprite & add it as a child to the guestPosition
            GameObject sprite = new GameObject();
            sprite.transform.parent = guestPositionList[i].transform;

            sprite = guestPositionList[i].transform.GetChild(2).gameObject;

            sprite.transform.position = new Vector3(0, 0);

            //create the SpriteRenderer & add properties
            sprite.AddComponent<SpriteRenderer>();
            GameObject defaultSprite = guestTypeList[number].transform.GetChild(0).gameObject;

            SpriteRenderer spriteRenderer = sprite.GetComponent<SpriteRenderer>();
            SpriteRenderer defaultRenderer = defaultSprite.GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = defaultRenderer.sprite;
            spriteRenderer.sortingLayerID = defaultRenderer.sortingLayerID;
            spriteRenderer.sortingLayerName = defaultRenderer.sortingLayerName;
            spriteRenderer.sortingOrder = defaultRenderer.sortingOrder;


            //create the Portrait
            GameObject portrait = new GameObject();

            //get the defaultPortrait GameObject
            GameObject defaultPortrait = guestTypeList[number].transform.GetChild(1).gameObject;

            GameObject portraitSmall = new GameObject();
            portraitSmall.transform.parent = portrait.transform;

            //create the SpriteRenderer & add properties
            portraitSmall.AddComponent<SpriteRenderer>();
            GameObject defaultSprite2 = defaultPortrait.transform.GetChild(0).gameObject;

            SpriteRenderer spriteRenderer2 = portraitSmall.GetComponent<SpriteRenderer>();
            SpriteRenderer defaultRenderer2 = defaultSprite2.GetComponent<SpriteRenderer>();

            spriteRenderer2.sprite = defaultRenderer2.sprite;
            spriteRenderer2.sortingLayerID = defaultRenderer2.sortingLayerID;
            spriteRenderer2.sortingLayerName = defaultRenderer2.sortingLayerName;
            spriteRenderer2.sortingOrder = defaultRenderer2.sortingOrder;


            GameObject portraitBig = new GameObject();
            portraitBig.transform.parent = portrait.transform;

            //create the SpriteRenderer & add properties
            portraitBig.AddComponent<SpriteRenderer>();
            GameObject defaultSprite3 = defaultPortrait.transform.GetChild(1).gameObject;

            SpriteRenderer spriteRenderer3 = portraitBig.GetComponent<SpriteRenderer>();
            SpriteRenderer defaultRenderer3 = defaultSprite3.GetComponent<SpriteRenderer>();

            spriteRenderer3.sprite = defaultRenderer3.sprite;
            spriteRenderer3.sortingLayerID = defaultRenderer3.sortingLayerID;
            spriteRenderer3.sortingLayerName = defaultRenderer3.sortingLayerName;
            spriteRenderer3.sortingOrder = defaultRenderer3.sortingOrder;

            //create the PortraitHandler & add properties
            guestPositionList[i].AddComponent<PortraitHandler>();

            PortraitHandler handler = guestPositionList[i].GetComponent<PortraitHandler>();

            handler.portraits = portrait;

            if (guestPositionList[i] == guestPositionList[isSpecialGuest])
            {
                handler.isSpecialGuest = true;
            }

            //position the newly generated guest correctly
            guestPositionList[i].transform.position = new Vector3(arranger.xpos, arranger.ypos);

            //now position the according portrait handler

            if (handler.isSpecialGuest == true)
            {
                portrait.transform.position = new Vector3(0, -0.35f);
                portraitBig.SetActive(true);
                portraitSmall.SetActive(false);
            }
            else
            {
                float offsetNumber;

                switch (offset)
                {
                    case 1:
                        offsetNumber = -1f;
                        offset++;
                        break;
                    case 2:
                        offsetNumber = 1f;
                        offset++;
                        break;
                    case 3:
                        offsetNumber = -1.9f;
                        offset++;
                        break;
                    case 4:
                        offsetNumber = 1.9f;
                        offset++;
                        break;
                    case 5:
                        offsetNumber = -2.8f;
                        offset++;
                        break;
                    case 6:
                        offsetNumber = 2.8f;
                        offset++;
                        break;
                    case 7:
                        offsetNumber = -3.7f;
                        offset++;
                        break;
                    case 8:
                        offsetNumber = 3.7f;
                        offset++;
                        break;
                    case 9:
                        offsetNumber = -4.6f;
                        offset++;
                        break;
                    case 10:
                        offsetNumber = 4.6f;
                        offset++;
                        break;
                    case 11:
                        offsetNumber = -5.5f;
                        offset++;
                        break;
                    case 12:
                        offsetNumber = 5.5f;
                        offset++;
                        break;
                    default:
                        offsetNumber = 0.0f;
                        break;
                }

                portraitBig.SetActive(false);
                portraitSmall.SetActive(true);

                portrait.transform.position = new Vector3(offsetNumber, -0.4f);
            }
        }
    }
		
	
	// Update is called once per frame
	void Update () {
		
	}
}
