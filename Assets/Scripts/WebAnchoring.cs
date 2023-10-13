using UnityEngine;

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
    private PlayerMovement pm;
    #endregion



    #region UnityMethods
    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        tetherRenderer.enabled = false; // Initially hide the tether
    }

    void Update()
    {
        // Visualize the tether if the spider is using the tether
        if (isUsingTether)
        {
            tetherRenderer.SetPosition(0, transform.position);
            tetherRenderer.SetPosition(1, tetherStartPoint);
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

    // public void StartJump()
    // {
    //     isJumping = true;
    //     tetherRenderer.enabled = true; // Show the tether
    // }
    //
    // public void EndJump()
    // {
    //     isJumping = false;
    //     if (!isUsingTether)
    //     {
    //         tetherRenderer.enabled = false; // Hide the tether if not using it
    //     }
    // }
    
    
    
    
    void ActivateTether()
    {
        RaycastHit hit;
        float maxDistance = 2f; // You can adjust this value based on your needs

        if (Physics.Raycast(spineret.position, Vector3.down, out hit, maxDistance))
        {
            tetherStartPoint = hit.point;
        }
        isUsingTether = true;
        tetherRenderer.enabled = true; // Show the tether
    }

    void DeactivateTether()
    {
        isUsingTether = false;
        isRetractingTether = false;
        tetherRenderer.enabled = false; // Hide the tether
    }

    void RetractTether()
    {
        transform.position = Vector3.MoveTowards(transform.position, tetherStartPoint, tetherRetractSpeed * Time.deltaTime);
        if (transform.position == tetherStartPoint)
        {
            DeactivateTether();
        }
    }
}