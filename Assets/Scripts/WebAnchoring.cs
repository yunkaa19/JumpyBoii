using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class WebAnchoring : MonoBehaviour
{

    #region Variables
    [Header("Web Anchoring Parameters")]
    public float tetherRetractSpeed = 5f;
    public LayerMask SilkableSurfaces;
    private Vector3 tetherStartPoint;
    public bool isUsingTether = false;
    public Transform spineret;
    private bool isRetractingTether = false;
    
    
    [Header("Web Visuals")]
    public LineRenderer tetherRenderer;
    
    [Header("UI References")]
    public Image tetherIcon; 
    public Sprite defaultIcon; 
    public Sprite tetheredIcon; 
    
    public Image detachTetherIcon;  
    public Sprite defaultDetachIcon;
    public Sprite activeDetachIcon; 
    
    #endregion



    #region UnityMethods
    private void Start()
    {
        tetherRenderer.enabled = false; // Initially hide the tether
        UpdateTetherIcon();
        UpdateDetachTetherIcon();

    }

    void Update()
    {
        // Visualize the tether if the spider is using the tether
        if (isUsingTether)
        {
            if (isUsingTether)
            {
                Vector3 localPlayerPos = tetherRenderer.transform.InverseTransformPoint(transform.position);
                Vector3 localTetherStartPoint = tetherRenderer.transform.InverseTransformPoint(tetherStartPoint);

                tetherRenderer.SetPosition(0, localPlayerPos);
                tetherRenderer.SetPosition(1, localTetherStartPoint);
            }
        }

        if (Input.GetButtonDown("WebAnchor")) // Respond to the "WebAnchor" input action
        {
            if (isUsingTether)
                isRetractingTether = !isRetractingTether;
            else
                ActivateTether();
        }

        if (Input.GetButtonDown("RemoveTether")) // Respond to the "RemoveTether" input action
        {
            isRetractingTether = false;
            DeactivateTether();
        }
        
        if (isUsingTether && isRetractingTether)
        {
            RetractTether();
        }
    }
    
    #endregion

    
    
    
    
    void ActivateTether()
    {
        //TODO: Make tether create new anchor points as it hits edges
        RaycastHit hit;
        float maxDistance = 2f; // You can adjust this value based on your needs

        if (Physics.Raycast(spineret.position, Vector3.down, out hit, maxDistance))
        {
            tetherStartPoint = hit.point;
        }
        isUsingTether = true;
        tetherRenderer.enabled = true; // Show the tether
        UpdateTetherIcon();
        UpdateDetachTetherIcon();
    }

    void DeactivateTether()
    {
        isUsingTether = false;
        isRetractingTether = false;
        tetherRenderer.enabled = false; // Hide the tether
        UpdateTetherIcon();
        UpdateDetachTetherIcon();
    }

    void RetractTether()
    {
        transform.position = Vector3.MoveTowards(transform.position, tetherStartPoint, tetherRetractSpeed * Time.deltaTime);
        if (transform.position == tetherStartPoint)
        {
            DeactivateTether();
        }
    }
    
    void UpdateTetherIcon()
    {
        if (tetherIcon) // Check if the tetherIcon is assigned
        {
            StartCoroutine(PulseEffect(tetherIcon,1f, 0.9f, 1.1f));  // Pulse for 1 second between 90% and 110% of original size

            tetherIcon.sprite = isUsingTether ? tetheredIcon : defaultIcon;
        }
    }
    
    void UpdateDetachTetherIcon()
    {
        if (detachTetherIcon)
        {
            // If the tether is active, the detach action is possible, so set the active icon
            detachTetherIcon.sprite = isUsingTether ? activeDetachIcon : defaultDetachIcon;

            // If transitioning to the active state, give a pulse effect
            if(isUsingTether) 
            {
                StartCoroutine(PulseEffect(detachTetherIcon, 1f, 0.9f, 1.1f));
            }
        }
    }
    
    IEnumerator PulseEffect(Image targetIcon, float duration, float minScale, float maxScale)  // Modify the PulseEffect to take an Image parameter
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float currentScale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(elapsedTime * Mathf.PI * 2 / duration) + 1) / 2);
            targetIcon.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetIcon.transform.localScale = Vector3.one;
    }
}