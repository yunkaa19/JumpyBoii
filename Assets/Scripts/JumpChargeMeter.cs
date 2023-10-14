using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class JumpChargeMeter : MonoBehaviour
{
    [Header("Precision Jumping Color Feedback")]
    public Gradient chargeGradient; // Gradient for the charge meter
    public PlayerMovement player; // Reference to the player's movement script
    public Image chargeMeter; // Reference to the Image component on this object
    public Slider chargeSlider; // Reference to the Slider component on this object
    private void Update()
    {
        // Assuming your PlayerMovement script has a public method or variable that returns the current charge level
        float currentCharge = player.GetJumpCharge();
        chargeSlider.value = currentCharge;
        chargeMeter.color = chargeGradient.Evaluate(currentCharge);
    }
}