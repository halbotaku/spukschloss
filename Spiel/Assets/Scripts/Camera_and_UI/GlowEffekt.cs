using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowEffekt : MonoBehaviour {

    //countdown timer
    private float timerA;
    private float timerB;

    //referencing the GlowSprite
    private SpriteRenderer sprite;

    // Use this for initialization
    void Start () {
        timerA = 0;
        timerB = 1;

       sprite = this.gameObject.GetComponent<SpriteRenderer>();
	}

    void Update()
    {
        if (timerA > 0)
        {
            timerA = timerA - Time.deltaTime;
            sprite.color = new Color(1f, 1f, 1f, (timerA/1)+0.2f);
        }

        if (timerA < 0)
        {
            timerA = 0;
            timerB = 2;
        }

        if (timerB > 0)
        {
            timerB = timerB - Time.deltaTime;
            sprite.color = new Color(1f, 1f, 1f, (1-timerB)+0.2f);
        }

        if (timerB < 0)
        {
            timerA = 1;
            timerB = 0;
        }
    }
}
