using UnityEngine;
using UnityEngine.Rendering; // For the new Post Processing system

public class VisualAcuity : MonoBehaviour
{
    [Header("Visual Acuity Parameters")]
    public Volume focusVolume; // Assign a Volume with desired effects for focus mode
    public float focusDuration = 5f; // Duration of focus mode
    private float focusTimer = 0f;
    public AudioSource focusSound; // Sound effect for entering focus mode

    private bool isFocusing = false;

    void Update()
    {
        if (Input.GetButtonDown("SpiderSense") && !isFocusing)
        {
            ActivateFocus();
        }
        else if (isFocusing)
        {
            focusTimer -= Time.deltaTime;
            if (focusTimer <= 0)
            {
                DeactivateFocus();
            }
        }
    }

    void ActivateFocus()
    {
        isFocusing = true;
        focusTimer = focusDuration;
        focusVolume.weight = 1; // Activate post-processing effects
        if (focusSound)
        {
            focusSound.Play();
        }
        // Any other logic for revealing hidden platforms or clues
    }

    void DeactivateFocus()
    {
        isFocusing = false;
        focusVolume.weight = 0; // Deactivate post-processing effects
        // Any other logic for hiding platforms or clues again
    }
}