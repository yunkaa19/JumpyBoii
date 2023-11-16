using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class ScrollingText : MonoBehaviour
{
    [Header("TextSettings")] 
    [SerializeField] float scrollSpeed = 100.0f;
    private float textPosStart = -2612f;
    private float textPosEnd = 1133f;

    private RectTransform TextTransform;
    


    [Header("UI Elements")] 
    [SerializeField] Image infoText;



    private void Start()
    {
        TextTransform = gameObject.GetComponent<RectTransform>();
        StartCoroutine(AutoScrollText());
    }

    private IEnumerator AutoScrollText()
    {
        while (TextTransform.localPosition.y < textPosEnd)
        {
            TextTransform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
