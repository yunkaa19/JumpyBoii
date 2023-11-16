using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBehaviour : MonoBehaviour
{
    private bool isLanternHit = false;
    private Rigidbody lanternRb;
    
    public void Hit()
    {
        isLanternHit = true;
    }


    private void Start()
    {
        lanternRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isLanternHit)
        {
            LanternFall();
        }
    }
    
    void LanternFall()
    {
        lanternRb.isKinematic = false;
    }
}
