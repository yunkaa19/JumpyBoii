using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
 
public class WebAnchoring : MonoBehaviour
{
    [Header("Web Anchoring Parameters")]
    public float maxRopeLength = 50f;
    public float segmentLength = 0.5f;
    public float tetherRetractSpeed = 5f;
    public Transform spineret;
    private Vector3 anchorPoint;
    public List<Vector3> ropePositions;
    private List<Vector3> previousPositions;
    private List<bool> segmentCollided; // List to track collision status of each segment
    public bool isUsingTether = false;
    private bool isRetractingTether = false;
    private float gravityScale = 0.1f; // Adjust this value to reduce the gravity effect
    private float damping = 0.95f; // Damping factor, adjust as needed
 
    [Header("Web Visuals")]
    public LineRenderer tetherRenderer;
 
    [Header("UI References")]
    public Image tetherIcon; 
    public Sprite defaultIcon; 
    public Sprite tetheredIcon; 
    public Image detachTetherIcon;  
    public Sprite defaultDetachIcon;
    public Sprite activeDetachIcon; 
 
    [Header("Collision Parameters")]
    public LayerMask groundLayer; // Assign this in the inspector
    public float collisionCheckDistance = 0.1f; // Distance to check for ground collision
 
    void Start()
    {
        ropePositions = new List<Vector3>();
        previousPositions = new List<Vector3>();
        segmentCollided = new List<bool>(); // Initialize the collision list
        tetherRenderer.enabled = false;
        tetherRenderer.useWorldSpace = true;
        
    }
 
    void Update()
    {
        if (isUsingTether)
        {
            // Check the total length of the rope before adding a new segment
            float totalRopeLength = CalculateTotalRopeLength();
            if (totalRopeLength < maxRopeLength)
            {
                Vector3 lastSegment = ropePositions[ropePositions.Count - 1];
                if (Vector3.Distance(transform.position, lastSegment) > segmentLength)
                {
                    // Add a new segment only if it doesn't exceed the max rope length
                    ropePositions.Add(transform.position);
                    previousPositions.Add(transform.position);
                    segmentCollided.Add(false);
                }
            }
            else
            {
                // Ensure the last segment is attached to the spider's position
                ropePositions[ropePositions.Count - 1] = transform.position;
            }
 
            UpdateRopePhysics();
            UpdateRopeVisual();
 
            if (isRetractingTether)
            {
                RetractTether();
            }
        }
 
        if (Input.GetButtonDown("WebAnchor"))
        {
            if (!isUsingTether)
            {
                ShootWeb();
            }
            else
            {
                isRetractingTether = !isRetractingTether; // Toggle retracting state
            }
        }
 
        if (Input.GetButtonDown("RemoveTether"))
        {
            RemoveTether();
        }
    }
    
    float CalculateTotalRopeLength()
    {
        float totalLength = 0f;
        for (int i = 0; i < ropePositions.Count - 1; i++)
        {
            totalLength += Vector3.Distance(ropePositions[i], ropePositions[i + 1]);
        }
        return totalLength;
    }
    
    void ShootWeb()
    {
        RaycastHit hit;
        if (Physics.Raycast(spineret.position, Vector3.down, out hit, maxRopeLength))
        {
            anchorPoint = hit.point;
            InitializeRope(anchorPoint);
            isUsingTether = true;
            tetherRenderer.enabled = true;
            
            UpdateTetherIcon();
            UpdateDetachTetherIcon();
        }
    }
 
    void InitializeRope(Vector3 anchorPoint)
    {
        ropePositions = new List<Vector3> { anchorPoint };
        previousPositions = new List<Vector3> { anchorPoint };
        segmentCollided = new List<bool> { false }; // Initialize with a single false value
    }
 
    void UpdateRopePhysics()
    {
        for (int i = 1; i < ropePositions.Count; i++)
        {
            Vector3 currentPos = ropePositions[i];
            Vector3 velocity = currentPos - previousPositions[i];
            velocity *= damping; // Apply damping to the velocity
 
            Vector3 newPos = currentPos + velocity;
            if (!segmentCollided[i]) // Only apply gravity if not collided
            {
                newPos += Physics.gravity * gravityScale * Time.deltaTime * Time.deltaTime;
            }
 
            // Perform a raycast to check for ground collision
            RaycastHit hit;
            if (Physics.Raycast(currentPos, Vector3.down, out hit, collisionCheckDistance, groundLayer))
            {
                newPos = hit.point + (Vector3.up * 0.1f);
                segmentCollided[i] = true; // Mark this segment as collided
            }
 
            previousPositions[i] = currentPos;
            ropePositions[i] = newPos;
        }
 
        for (int i = 0; i < ropePositions.Count - 1; i++)
        {
            Vector3 direction = ropePositions[i + 1] - ropePositions[i];
            float distance = direction.magnitude;
            float error = distance - segmentLength;
            Vector3 changeDir = direction.normalized * error;
 
            if (i != 0)
            {
                ropePositions[i] += changeDir * 0.5f;
                ropePositions[i + 1] -= changeDir * 0.5f;
            }
            else
            {
                ropePositions[i + 1] -= changeDir;
            }
        }
    }
 
    void UpdateRopeVisual()
    {
        tetherRenderer.positionCount = ropePositions.Count;
        tetherRenderer.SetPositions(ropePositions.ToArray());
    }
    
    void RetractTether()
    {
        if (ropePositions.Count > 1)
        {
            Vector3 nextPoint = ropePositions[ropePositions.Count - 2];
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, tetherRetractSpeed * Time.deltaTime);
 
            if (Vector3.Distance(transform.position, nextPoint) < segmentLength)
            {
                ropePositions.RemoveAt(ropePositions.Count - 1);
                previousPositions.RemoveAt(previousPositions.Count - 1);
                segmentCollided.RemoveAt(segmentCollided.Count - 1); // Remove the corresponding collision status
            }
        }
        else
        {
            RemoveTether();
        }
    }
 
    void RemoveTether()
    {
        isUsingTether = false;
        isRetractingTether = false;
        tetherRenderer.enabled = false;
        ropePositions.Clear();
        previousPositions.Clear();
        segmentCollided.Clear(); // Clear the collision list
        
        UpdateDetachTetherIcon();
        UpdateTetherIcon();
    }
 
 
    #region UI
 
    
 
    void UpdateTetherIcon()
    {
        if (tetherIcon)
        {
            tetherIcon.sprite = isUsingTether ? tetheredIcon : defaultIcon;
            if (isUsingTether)
            {
                StartCoroutine(PulseEffect(tetherIcon, 1f, 0.9f, 1.1f));
            }
        }
    }
    
    void UpdateDetachTetherIcon()
    {
        if (detachTetherIcon)
        {
            detachTetherIcon.sprite = isUsingTether ? activeDetachIcon : defaultDetachIcon;
            if (isUsingTether)
            {
                StartCoroutine(PulseEffect(detachTetherIcon, 1f, 0.9f, 1.1f));
            }
        }
    }
    
    IEnumerator PulseEffect(Image targetIcon, float duration, float minScale, float maxScale)
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
    #endregion
}