using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMoonSpawn : MonoBehaviour {

    public Sprite sun;
    public Sprite moon;

    public float speed;

	// Use this for initialization
	void Start () {
        this.GetComponent<SpriteRenderer>().sprite = sun;
	}
	
	// Update is called once per frame
	void Update () {

        if (this.transform.position.y < -4)
        {
            this.GetComponent<SpriteRenderer>().sprite = moon;
            speed = -speed;
        }

        this.gameObject.transform.Translate(0, speed * Time.deltaTime, 0);
	}
}
