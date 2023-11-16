using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatScare : MonoBehaviour
{
    public Animator goatAnimator;
    public LanternBehaviour lanternScript;
    public AudioSource goatSound;
    private bool isGoatAwake = false;
    
    public void WakeUp()
    {
        isGoatAwake = true;
        goatAnimator.Play("sit_to_stand");
        goatSound.Play();    
    }
    
    public void RunAndOpenDoor()
    {
        
        goatAnimator.Play("trot_forward");
        
    }
    
    
}
