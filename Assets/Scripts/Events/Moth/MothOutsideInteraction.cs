using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothOutsideInteraction : MonoBehaviour
{
    public MothBehaviour mothScript;

    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Finish"))
        {   
            mothScript.HitExit();
        }
    }
}
