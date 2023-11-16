using UnityEngine;
using UnityEngine.UI;

public class JumpChargeMeter : MonoBehaviour
{
    [Header("Precision Jumping Color Feedback")]
    public Gradient chargeGradient;
    public PlayerMovement player;
    public Image chargeMeter;
    public Slider chargeSlider;

    private void Update()
        {



            if (player.isPreparingJump)
            {
                float currentCharge = player.GetJumpCharge();
                chargeSlider.value = currentCharge;
                chargeMeter.color = chargeGradient.Evaluate(currentCharge);
            }

            else
            {
                ResetChargeMeter();
            }
        
    }

    private void ResetChargeMeter()
    {
        chargeSlider.value = 0;
        chargeMeter.color = chargeGradient.Evaluate(0);
    }
}