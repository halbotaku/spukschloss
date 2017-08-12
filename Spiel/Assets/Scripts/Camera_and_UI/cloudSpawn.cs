using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudSpawn : MonoBehaviour {

    private SpriteRenderer myRenderer;

    public Sprite cloud1;
    public Sprite cloud2;
    public Sprite cloud3;

    private float speed = 0.5f;

    public int cloudAmount;

    private static System.Random rnd;
    static cloudSpawn()
    {
        rnd = new System.Random();
    }

    // Use this for initialization
    void Awake()
    {
        for (int i = 0; i < cloudAmount; i++)
        {
            int sprite = rnd.Next(1, 4);
            int scale = rnd.Next(1, 6);
            int flipx = rnd.Next(1, 3);
            int flipy = rnd.Next(1, 3);

            float firstdigitx = rnd.Next(-15, 14);
            float seconddigitx = rnd.Next(0, 10);

            float xpos = float.Parse(firstdigitx + "." + seconddigitx);

            float firstdigity = rnd.Next(0, 9);
            float seconddigity = rnd.Next(0, 10);

            float ypos = float.Parse(firstdigity + "." + seconddigity);

            spawn(sprite, scale, flipx, flipy, xpos, ypos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < cloudAmount; i++)
        {
            //move the cloud accordning to the speed
            GameObject cloud = this.gameObject.transform.GetChild(i).gameObject;

            cloud.transform.Translate(speed * Time.deltaTime, 0, 0);

            if (cloud.transform.position.x > 15)
            {
                int sprite = rnd.Next(1, 4);
                int scale = rnd.Next(1, 6);
                int flipx = rnd.Next(1, 3);
                int flipy = rnd.Next(1, 3);

                float firstdigitx = rnd.Next(-15, -12);
                float seconddigitx = rnd.Next(0, 10);

                float xpos = float.Parse(firstdigitx + "." + seconddigitx);

                float firstdigity = rnd.Next(0, 9);
                float seconddigity = rnd.Next(0, 10);

                float ypos = float.Parse(firstdigity + "." + seconddigity);

                //destroy this cloud
                reset(i, sprite, scale, flipx, flipy, xpos, ypos);
            }
        }
    }

    private void spawn(int sprite, int scale, int flipx, int flipy, float xpos, float ypos)
    {
        GameObject cloud = new GameObject();
        cloud.transform.parent = this.gameObject.transform;

        cloud.tag = "cloud";

        //Create Spriterenderer
        cloud.AddComponent<SpriteRenderer>();

        myRenderer = cloud.GetComponent<SpriteRenderer>();

        myRenderer.sortingLayerName = "Background";
        myRenderer.sortingOrder = -5;

        //pick a sprite
        switch (sprite)
        {
            case 1:
                myRenderer.sprite = cloud1;
                break;
            case 2:
                myRenderer.sprite = cloud2;
                break;
            case 3:
                myRenderer.sprite = cloud3;
                break;
        }

        //pick a random scale
        switch (scale)
        {
            case 1:
                cloud.transform.localScale = new Vector2(1, 1);
                break;
            case 2:
                cloud.transform.localScale = new Vector2(0.75f, 0.75f);
                break;
            case 3:
                cloud.transform.localScale = new Vector2(1.75f, 1.75f);
                break;
            case 4:
                cloud.transform.localScale = new Vector2(1.25f, 1.25f);
                break;
            case 5:
                cloud.transform.localScale = new Vector2(1.5f, 1.5f);
                break;
        }

        //decide whether the Sprite is flipped on x or not
        if (flipx == 1)
        {
            myRenderer.flipX = true;
        }

        //decide whether the Sprite is flipped on y or not
        if (flipy == 1)
        {
            myRenderer.flipY = true;
        }

        //spawn somewhere in the sky
        cloud.transform.position = new Vector2(xpos, ypos);
    }

    private void reset(int i, int sprite, int scale, int flipx, int flipy, float xpos, float ypos)
    {
        GameObject cloud = this.gameObject.transform.GetChild(i).gameObject;

        myRenderer = cloud.GetComponent<SpriteRenderer>();

        //pick a sprite
        switch (sprite)
        {
            case 1:
                myRenderer.sprite = cloud1;
                break;
            case 2:
                myRenderer.sprite = cloud2;
                break;
            case 3:
                myRenderer.sprite = cloud3;
                break;
        }

        //pick a random scale
        switch (scale)
        {
            case 1:
                cloud.transform.localScale = new Vector2(1, 1);
                break;
            case 2:
                cloud.transform.localScale = new Vector2(0.75f, 0.75f);
                break;
            case 3:
                cloud.transform.localScale = new Vector2(1.75f, 1.75f);
                break;
            case 4:
                cloud.transform.localScale = new Vector2(1.25f, 1.25f);
                break;
            case 5:
                cloud.transform.localScale = new Vector2(1.5f, 1.5f);
                break;
        }

                //decide whether the Sprite is flipped on x or not
                if (flipx == 1)
        {
            myRenderer.flipX = true;
        }

        //decide whether the Sprite is flipped on y or not
        if (flipy == 1)
        {
            myRenderer.flipY = true;
        }

        //spawn somewhere in the sky
        cloud.transform.position = new Vector2(xpos, ypos);
    }
}
