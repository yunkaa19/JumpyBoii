using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMothInteraction : MonoBehaviour
{
    public MothBehaviour mothScript;

    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {   
            mothScript.WakeUp();
        }
    }
}
