using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.UI;

public class VisualAcuity : MonoBehaviour
{
    [Header("Player Movement Reference")]
    public PlayerMovement playerMovement;

    [Header("Visual Acuity Parameters")]
    public Volume focusVolume;
    public float focusDuration = 5f;
    private float focusTimer = 0f;
    public AudioSource focusSound;
    public float cooldownDuration = 5f;

    [Header("Cooldown UI")]
    public Image cooldownOverlay;

    [Header("Time Control Parameters")]
    public float focusTimeScale = 0.5f;
    private float originalTimeScale;

    private bool isFocusing = false;
    private float nextReadyTime;
    private float cooldownTimeLeft;

    [Header("Volume Fade Parameters")]
    public float fadeDuration = 1f;

    void Update()
    {
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
        }

        if (Input.GetButtonDown("SpiderSense") && !isFocusing && !isCooldown)
        {
            ActivateFocus();
            nextReadyTime = Time.time + cooldownDuration; // Start the cooldown after activating focus
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

        StartCoroutine(FadeVolumeTo(1f));
        StartCoroutine(PulseEffect(0.5f, 1.1f, 0.9f));

        if (focusSound)
        {
            focusSound.Play();
        }

        originalTimeScale = Time.timeScale;
        Time.timeScale = focusTimeScale;
        if (playerMovement)
            playerMovement.SetFocusMultiplier(2f);
    }

    void DeactivateFocus()
    {
        Time.timeScale = originalTimeScale;
        isFocusing = false;

        StartCoroutine(FadeVolumeTo(0f));

        if (focusSound && focusSound.isPlaying)
        {
            focusSound.Stop();
        }
        if (playerMovement)
            playerMovement.SetFocusMultiplier(1f);
    }

    IEnumerator FadeVolumeTo(float targetWeight)
    {
        float startWeight = focusVolume.weight;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newWeight = Mathf.Lerp(startWeight, targetWeight, elapsedTime / fadeDuration);
            focusVolume.weight = newWeight;
            yield return null;
        }

        focusVolume.weight = targetWeight;
    }
    
    IEnumerator PulseEffect(float duration, float maxScale, float minScale)
    {
        float elapsedTime = 0f;
        Vector3 originalScale = cooldownOverlay.transform.localScale;

        while (elapsedTime < duration)
        {
            float currentScale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(elapsedTime * Mathf.PI * 2 / duration) + 1) / 2);
            cooldownOverlay.transform.localScale = originalScale * currentScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cooldownOverlay.transform.localScale = originalScale;
    }
}