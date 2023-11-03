using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldown : MonoBehaviour
{
    public Image cooldownOverlay; // Drag the cooldown overlay image here in the inspector
    public float cooldownDuration; // Set the cooldown duration for each skill
    public MouseButton triggerButton; // Set the mouse button that activates the skill

    private float nextReadyTime;
    private float cooldownTimeLeft;
    private bool isReady = true;
    
    private void Update()
    {
        // Check for mouse button input
        if (Input.GetMouseButtonDown((int)triggerButton) && isReady)
        {
            TriggerCooldown();
            isReady = false;
        }

        bool isCooldown = (Time.time < nextReadyTime);
        if (isCooldown)
        {
            cooldownTimeLeft = nextReadyTime - Time.time;
            float fillAmount = cooldownTimeLeft / cooldownDuration;
            cooldownOverlay.fillAmount = fillAmount;
        }
        else
        {
            cooldownOverlay.fillAmount = 0;
            isReady = true;
        }
    }

    public void TriggerCooldown()
    {
        nextReadyTime = Time.time + cooldownDuration;
    }

    // Enum to represent mouse buttons for clarity
    public enum MouseButton
    {
        LeftClick = 0,
        RightClick = 1,
        MiddleClick = 2
    }
}