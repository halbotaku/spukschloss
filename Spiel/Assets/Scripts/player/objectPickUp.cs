using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPickUp : MonoBehaviour {

    //stop/begin following the player when picked up
    public bool followPlayer = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Method for picking up pickUpObjects when player is colliding with them
    void OnTriggerEnter2D(Collider2D other)
    {
        //if the object is of the tag pickup Object
        if (other.gameObject.CompareTag("pickUpObject"))
        {
            followPlayer = true;
        }
    }
}
