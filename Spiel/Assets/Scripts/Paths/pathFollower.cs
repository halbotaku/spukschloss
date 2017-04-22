using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathFollower : MonoBehaviour {

    //Create Array for saving the Waypoints
    public GameObject[] waypoints;

    //Create variable for remembering the current waypoint
    public int num = 0;

    //minimum Distance
    public float minDist;
    //Movementspeed
    public float speed;

    //Create booleans for start/stop of Movement
    public bool rand = false;
    public bool go = true;

    // variable to hold a reference to our SpriteRenderer component (Flipping the Sprite)
    private SpriteRenderer mySpriteRenderer;

    // This function is called just one time by Unity the moment the game loads
    private void Awake()
    {
        // get a reference to the SpriteRenderer component on this gameObject (Flipping the Sprite)
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Check the distance of the object to the next waypoint
        float dist = Vector3.Distance(gameObject.transform.position, waypoints[num].transform.position);

        //Check if Object is set to Movement
        if (go)
        {
            //Check if minimum Distance is achieved, if so switch to next waypoint
            if (dist > minDist)
            {
                //Move the Object to next waypoint
                Move();
            }
            else
            {
                //Check if movement is set on random
                if (!rand)
                {
                    //When the last waypoint has been reached
                    if (num + 1 == waypoints.Length)
                    {
                        //stop the movement and give out a sign on the console
                        System.Console.WriteLine("Ziel Erreicht!");
                    }
                    else
                    {
                        //increase the counting value to move to the next waypoint
                        num++;
                    }
                }
                else
                {
                    //choose a random point of the waypointsystem to head for next
                    num = Random.Range(0, waypoints.Length);
                }
            }
        }
		
	}

    public void Move()
    {
        //Get the difference of the current position to the next waypoint
        Vector3 difference = waypoints[num].transform.position-transform.position;

        //calculate the normal vector
        difference.Normalize();

        //move the object from current to next waypoint
        gameObject.transform.position += difference * speed * Time.deltaTime;


        // if the variable isn't empty (we have a reference to our SpriteRenderer)
        if (mySpriteRenderer != null)
        {
            //When movement is going to the left
            if (difference.x < 0)
            {
                // flip the sprite
                mySpriteRenderer.flipX = true;
            }
            else if (difference.x > 0)
            {
                // revert the sprite to normal
                mySpriteRenderer.flipX = false;
            }
        }

    }
}
