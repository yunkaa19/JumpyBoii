using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatScare : MonoBehaviour
{
    public DoorBehaviour doorScript;
    public Animator goatAnimator;
    public Transform door; // Assign the door's transform here
    public Transform nextTarget;
    public AudioSource goatSound;
    public GoatAtDoors goatAtDoors;
    public float speed = 5f; // Speed at which the goat moves
    private bool isGoatAwake = false;
    private bool isMovingTowardsDoor = false;
    private bool isMovingAway = false;
    public void WakeUp()
    {
        isGoatAwake = true;
        goatAnimator.Play("sit_to_stand");
        isMovingTowardsDoor = true; // Start moving towards the door
        StartCoroutine(PlayGoatSoundWithDelay(1f));
    }

    void Update()
    {
        
        if (isMovingTowardsDoor)
        {
            MoveTowardsDoor();
        }
        else if (isMovingAway)
        {
            MoveAway();
        }
    }

    void MoveTowardsDoor()
    {
        goatAnimator.Play("trot_forward");
        transform.position = Vector3.MoveTowards(transform.position, door.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, door.position) < 1f) // Adjust the distance as needed
        {
            goatAtDoors.doorEvents();
            isMovingAway = true;
            isMovingTowardsDoor = false;
            RunAndOpenDoor();
        }
    }

    void MoveAway()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextTarget.position, speed * Time.deltaTime);
    }

    public void RunAndOpenDoor()
    {
        goatAnimator.Play("trot_forward");
        // Trigger the door's opening animation here
        doorScript.OpenDoors();
        goatAnimator.Play("turn_90_R");
        // Start coroutine to run forward and then destroy the goat
        StartCoroutine(RunForwardAndDestroy());
    }

    IEnumerator RunForwardAndDestroy()
    {
        // Wait for a few seconds while the goat runs forward
        yield return new WaitForSeconds(5); // Adjust the time as needed

        // Destroy the goat object
        Destroy(gameObject);
    }
    
    IEnumerator PlayGoatSoundWithDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Play the goat sound
        goatSound.Play();
    }
}
    
    

