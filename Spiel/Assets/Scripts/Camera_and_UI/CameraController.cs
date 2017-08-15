using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour {

    //create variables for referencing Player and Camera Position
    public GameObject player;

    float posx;
    float posy;

void start()
    {
        //get the position of the player
        posx = player.transform.position.x;
        posy = player.transform.position.y;
    }

	// Update is called once per frame
	void LateUpdate () {

        //save value of camera in variables
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        //check the distances of background borders to camera borders
        double distanceTop = 19.52 - (player.transform.position.y + 1.77*height);
        double distanceBottom = -19.52 + (player.transform.position.y + 2.125*height);
        double distanceRight = 32.9 - (player.transform.position.x + 1.64 * width);
        double distanceLeft = -32.9 + (player.transform.position.x + 1.98 * width);

        //when the border of the camera crosses the background picture's border & movement is upwards/downwards
        if (distanceTop >= 0 && posy < player.transform.position.y || distanceBottom >=0 && posy > player.transform.position.y)
        {
            //position Camera according to player movement
            transform.position = new Vector3(transform.position.x, player.transform.position.y, 0);
        }

        if (distanceLeft >= 0 && posx > player.transform.position.x || distanceRight >= 0 && posx < player.transform.position.x)
        {
            //position Camera according to player movement
            transform.position = new Vector3(player.transform.position.x, transform.position.y, 0);
        }

    }
}
