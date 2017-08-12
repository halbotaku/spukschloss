using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawn : MonoBehaviour {

    private SpriteRenderer myRenderer;

    public Sprite star1;
    public Sprite star2;

    public int starAmount;

    private static System.Random rnd;
    private string spawnControl ="left";
    private int spawnxa;
    private int spawnxb;
    private int spawnya;
    private int spawnyb;


    static StarSpawn()
    {
        rnd = new System.Random();
    }


    // Use this for initialization
    void Awake () {
        for (int i = 0; i < starAmount; i++)
        {
            spawn();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void spawn()
    {
        GameObject cloud = new GameObject();
        cloud.transform.parent = this.gameObject.transform;

        cloud.tag = "star";

        //Create Spriterenderer
        cloud.AddComponent<SpriteRenderer>();

        myRenderer = cloud.GetComponent<SpriteRenderer>();

        myRenderer.sortingLayerName = "Background";
        myRenderer.sortingOrder = -5;

        int sprite = rnd.Next(1, 3);
        int scale = rnd.Next(1, 6);

        if (spawnControl == "left")
        {
            spawnxa = -11;
            spawnxb = -7;
            spawnya = -6;
            spawnyb = 7;

            spawnControl = "middle";
        } else if (spawnControl == "middle")
        {
            spawnxa = -7;
            spawnxb = 8;
            spawnya = 0;
            spawnyb = 7;

            spawnControl = "right";
        }else if (spawnControl == "right")
        {
            spawnxa = 7;
            spawnxb = 14;
            spawnya = -6;
            spawnyb = 7;

            spawnControl = "left";
        }

        float firstdigitx = rnd.Next(spawnxa, spawnxb);
        float seconddigitx = rnd.Next(0, 10);

        float xpos = float.Parse(firstdigitx + "." + seconddigitx);

        float firstdigity = rnd.Next(spawnya, spawnyb);
        float seconddigity = rnd.Next(0, 10);

        float ypos = float.Parse(firstdigity + "." + seconddigity);

        //pick a sprite
        switch (sprite)
        {
            case 1:
                myRenderer.sprite = star1;
                break;
            case 2:
                myRenderer.sprite = star2;
                break;
        }

        //pick a random scale
        switch (scale)
        {
            case 1:
                cloud.transform.localScale = new Vector2(1, 1);
                break;
            case 2:
                cloud.transform.localScale = new Vector2(0.25f, 0.25f);
                break;
            case 3:
                cloud.transform.localScale = new Vector2(0.5f, 0.5f);
                break;
            case 4:
                cloud.transform.localScale = new Vector2(1.25f, 1.25f);
                break;
            case 5:
                cloud.transform.localScale = new Vector2(1.5f, 1.5f);
                break;
        }

        //spawn somewhere in the sky
        cloud.transform.position = new Vector2(xpos, ypos);
    }
}
