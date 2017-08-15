using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RepairScript : MonoBehaviour {

    //boolean remembering the repair process
    private bool isRepairing;

    //ArrayList remembering the destinations & according Objects
    public List<string> roomRepairList;
    public List<string> objectRepairList;

    //Variables remembering the object and acccording animator
    private InteractionList interactionList;
    private Animator animator;

    //variable controlling the countdown of the repairtime
    private float countdown;

    //Variables for calling the animatior of the repair timer
    private GameObject repairTimer;
    private Animator repairTimeAnimator;

    private AudioSource audio;

    void Awake()
    {
        roomRepairList = new List<string>();
        objectRepairList = new List<string>();
    }

    void Start() {
        audio = this.gameObject.GetComponent<AudioSource>();

        //hotelOwner is not repairing in the beginning
        isRepairing = false;

        //reference the timer GameObject & Animator
        repairTimer = gameObject.transform.GetChild(1).gameObject;
        repairTimeAnimator = repairTimer.GetComponent<Animator>();
        repairTimer.SetActive(false);
    }

    void Update() {

        if (isRepairing == true)
        {
            //handle the countdown of the repair time
            if (countdown > 0)
            {
                //reduce the countdown
                countdown = countdown - Time.deltaTime;
            }
            else
            {
                //continue after the waiting time
                isRepairing = false;
                handleRepairFinish();
            }
        }
    }

    public void repair()
    {
        //find the listed object whithin the specified room
        GameObject toRepair = GameObject.Find(objectRepairList[0]);

        //get the InteractionList Component
        interactionList = toRepair.GetComponent<InteractionList>();

        if (interactionList.position == roomRepairList[0])
        {
            //calculate the grade of damage
            string damage = interactionList.gradeOfDamage[interactionList.index];
            string evaluatedDamage = checkGradeOfDamage(damage);

            //calculate the speed with which the animation needs to be played according to the waiting time
            float animationSpeed = (4 / 3) / interactionList.repairTime[interactionList.index];


            if (animationSpeed - 0.05f > 0)
            {
                animationSpeed -= 0.05f;
            }

            repairTimeAnimator.speed = animationSpeed;

            //set the Repair-Time-Animator to true & play the according timer
            repairTimer.SetActive(true);
            repairTimeAnimator.Play(evaluatedDamage);

            repairTimer.SetActive(true);

            animator = interactionList.GetComponentInChildren<Animator>();

            //handle waiting for the repairtime length
            countdown = interactionList.repairTime[interactionList.index];
            isRepairing = true;
            audio.Play();
        }
    }

    private void handleRepairFinish() {
        //pick the rightful animation out of the animationList
        animator.Play("repaired");
        interactionList.hasBeenInteractedWith = false;

        //remove the first entries of room and repair object lists
        roomRepairList.RemoveAt(0);
        objectRepairList.RemoveAt(0);

        //deactivate the timer
        repairTimer.SetActive(false);

        this.gameObject.GetComponent<pathFollower>().isRepairing = false;
    }


    public string checkGradeOfDamage(string damage)
    {
        string patienceCounterAnim;

        switch (damage)
        {
            case "light":
                patienceCounterAnim = "light_time_animation";
                break;
            case "middle":
                patienceCounterAnim = "middle_time_animation";
                break;
            case "heavy":
                patienceCounterAnim = "heavy_time_animation";
                break;
            default:
                patienceCounterAnim = "light_time_animation";
                break;
        }

        return patienceCounterAnim;
    }
}