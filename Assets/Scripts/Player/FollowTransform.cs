using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;   

    public float positionSmoothTime = 0.001f; 
    public float rotationSmoothTime = 0.3f; 

    private Vector3 velocity = Vector3.zero; 

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, positionSmoothTime);

    }
}
