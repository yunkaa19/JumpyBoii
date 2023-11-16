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
    private bool flyAway = false;
    

    public void WakeUp()
    {
        isAwake = true;
        FlyTowardsLantern();
    }
    
    public void HitLantern()
    {
        isLanternHit = true;
    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered:" + other.tag);
        if (other.CompareTag("Finish"))
        {
            Debug.Log("Triggered");
            
            isExitHit = true;
        }
    }

    void Update()
    {
        if (isAwake && !isLanternHit)
        {
            FlyTowardsLantern();
        }
        else if (isAwake && isExitHit)
        {
            Destroy(gameObject);
        }
        else if (isAwake && isLanternHit)
        {
            speed = 35f;
            FlyAway();
        }
    }
    

    void FlyTowardsLantern()
    {
        transform.position = Vector3.MoveTowards(transform.position, lantern.transform.position, speed * Time.deltaTime);
    }

    void FlyAway()
    {
        flyAway = true;
        transform.position = Vector3.MoveTowards(transform.position, exit.transform.position, speed * Time.deltaTime);
    }
}
