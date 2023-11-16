using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatAtDoors : MonoBehaviour
{
    public ParticleSystem dustParticles;
    public AudioSource DoorSound;
    public  void doorEvents()
    {
            DoorSound.Play();
            dustParticles.Play();
            
    }

}
