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
        gameObject.transform.LookAt(waypoints[num].transform.position);
        gameObject.transform.position += gameObject.transform.forward * speed * Time.deltaTime;
    }
}
