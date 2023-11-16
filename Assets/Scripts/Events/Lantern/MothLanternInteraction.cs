using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothLanternInteraction : MonoBehaviour
{
    public MothBehaviour mothScript;
    public LanternBehaviour lanternScript;
    public GoatScare goatScript;
    private bool playedSound = false;
    
    public AudioSource audioSource;
    
    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Objective"))
        {   
            mothScript.HitLantern();
            lanternScript.Hit();
        }

        if (other.CompareTag("Reach") && !playedSound)
        {
            playedSound = true;
            audioSource.Play();
            goatScript.WakeUp();
            
            
        }
    }
}
