using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItem : MonoBehaviour {

    private float floatingSpeed;
    private float floatingDistance;

    //variables maintaining infos
    public string myName;
    public Vector2 newSpeed;
    public float speedDuration;
    public float slipRecoveryDuration;
    public float timeUntilScreamExplodes;

	// Use this for initialization
	void Start () {
        //reference the offset
        SpecialPickUp player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpecialPickUp>();
        floatingSpeed = player.floatingSpeed;
        floatingDistance = player.floatingDistance-0.5f;

	}
	
	// Update is called once per frame
	void Update () {
        Vector2 goal;
        
            /* Trigonometry Calculations: Set the position of the floating Object to
             * a variable offset and let it float up and down via a sinus curve
             */

            goal = transform.position + Mathf.Sin(Time.time * floatingSpeed) * Vector3.up * floatingDistance;

            this.gameObject.transform.position = Vector2.Lerp(this.gameObject.transform.position, goal, Time.deltaTime);
    }
}
