using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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
        double distanceTop = 5.3961 - player.transform.position.y - 0.51*height;
        double distanceBottom = player.transform.position.y - 0.51*height + 5.3961;
        double distanceRight = 9.5961 - player.transform.position.x - 0.51*width;
        double distanceLeft = player.transform.position.x - 0.51 * width + 9.5961;

        //when the border of the camera does not cross the background picture's border & movement is upwards
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
