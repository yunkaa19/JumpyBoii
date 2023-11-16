using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothBehaviour : MonoBehaviour
{
    public GameObject lantern; // Assign the lantern in the inspector
    public GameObject exit;
    public float speed = 5f;
    private bool isAwake = false;
    private bool isLanternHit = false;
    private bool isExitHit = false;
    

    public void WakeUp()
    {
        isAwake = true;
        FlyTowardsLantern();
    }
    
    public void HitLantern()
    {
        isLanternHit = true;
    }
    
    public void HitExit()
    {
        isExitHit = true;
    }

    void Update()
    {
        if (isAwake && !isLanternHit)
        {
            FlyTowardsLantern();
        }
        else if (isAwake && isLanternHit)
        {
            speed = 35f;
            FlyAway();
        }
        else if (isAwake && isExitHit)
        {
            Destroy(gameObject);
        }
    }
    

    void FlyTowardsLantern()
    {
        transform.position = Vector3.MoveTowards(transform.position, lantern.transform.position, speed * Time.deltaTime);
    }

    void FlyAway()
    {
        transform.position = Vector3.MoveTowards(transform.position, exit.transform.position, speed * Time.deltaTime);
    }
}
