using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    public Vector3 leftDoorOpenPosition = new Vector3(-6.020361f, -1.144409e-05f, -2.747852f);
    public Vector3 rightDoorOpenPosition = new Vector3(-5.988956f, -1.144409e-05f, 2.759985f);

    public Quaternion leftDoorOpenRotation = Quaternion.Euler(0, -156.244f, 0);
    public Quaternion rightDoorOpenRotation = Quaternion.Euler(0, 157.22f, 0);
    
    private bool isDoorHit = false;
    
    public float openingDuration = 2f; // Duration of the door opening

    
    
    void Update()
    {
        if (isDoorHit)
        {
            OpenDoors();
            
        }
    }
    
    
    public void OpenDoors()
    {
        StartCoroutine(OpenDoor(leftDoor, leftDoorOpenPosition, leftDoorOpenRotation));
        StartCoroutine(OpenDoor(rightDoor, rightDoorOpenPosition, rightDoorOpenRotation));
    }

    
    
    
    private IEnumerator OpenDoor(Transform door, Vector3 openPosition, Quaternion openRotation)
    {
        float elapsedTime = 0;

        Vector3 startingPos = door.position;
        Quaternion startingRot = door.rotation;

        while (elapsedTime < openingDuration)
        {
            door.localPosition = Vector3.Lerp(startingPos, openPosition, elapsedTime / openingDuration);
            door.localRotation = Quaternion.Lerp(startingRot, openRotation, elapsedTime / openingDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.localPosition = openPosition;
        door.localRotation = openRotation;
        Destroy(gameObject);
    }
}
