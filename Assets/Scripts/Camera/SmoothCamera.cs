using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// The Smooth Camera class applies a smooth motion to the player's camera.
///
/// @author - Jonathan L.A
/// @author - Alex I.
/// @version - 1.0.0
///
/// </summary>
public class SmoothCamera : NetworkBehaviour
{

    [SerializeField]
    [Tooltip("The higher the value, the slower the camera moves.")]
    private float dampTime = 0.15f;
    [SerializeField]
    [Tooltip("The camera will not follow the target if the target this close to the camera's center")]
    private float deadzoneRadius;
    [SerializeField]
    [Tooltip("The higher this value, the slower the camera follows the target in the deadzone")]
    private float deadzoneDampTime = 0.5f;
    [SerializeField]
    [Tooltip("The object that the camera follows.")]
    private Transform target;
    [SerializeField]
    [Tooltip("The camera's default z-value.")]
    private float zPosition;
    private float deadzoneRadiusSquared;
    private new Transform transform;
    private Vector2 velocity = Vector2.zero;
    private Rigidbody playerRigidbody;
    private Current current;
    private bool initialized;
    private string particleDirection = "";
    private static SmoothCamera cameraInstance;
    private Zoom zoomManager;

    /// <summary>
    /// Intiailizes the camera,
    /// and ensures that there is only once camera instance per player
    /// </summary>
    public void Init()
    {
        GameObject currentObject = GameObject.FindGameObjectWithTag("Current");
        if (currentObject != null)
        {
            current = currentObject.GetComponent<Current>();
        }

        zoomManager = GetComponent<Zoom>();

        transform = GetComponent<Transform>();
        Vector3 position = transform.position;
        position.z = zPosition;
        transform.position = position;
        deadzoneRadiusSquared = deadzoneRadius * deadzoneRadius;
        
        if (isLocalPlayer)
        {
            if (cameraInstance != null && cameraInstance != this)
            {
                GameObject.Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
                cameraInstance = this;
            }
        }
        
        initialized = true;
    }
    
    /// <summary>
    /// If player is not in currents, do the default camera movement
    /// Else apply the boundary camera movement
    /// </summary>
    void FixedUpdate()
    {
        if (!initialized) { return; }

        if (target == null)
        {
            InitializeTarget();
        }
        if (target)
        {
            if (!current.PlayerInCurrents())
            {
                SmoothCameraMovement();
            }
            else
            {
                ApplyCameraBoundary();
            }
        }
    }
    
    /// <summary>
    /// Default camera movement
    /// </summary>
    private void SmoothCameraMovement()
    {
        Vector3 newPosition = Vector3.zero;
        float dampTime = this.dampTime;
        Vector3 targetPosition = Vector2.zero;
    
        float distanceFromTarget = ((Vector2)(target.position - transform.position)).sqrMagnitude;   
        // Choose a different damping time based on whether or not the target is in the deadzone
        if (distanceFromTarget <= deadzoneRadiusSquared)
        {
            dampTime = deadzoneDampTime;
            targetPosition = target.position;
        }
        // Else, if the target isn't in the deadzone
        else
        {
            // Compute the target position of the camera
            Vector3 distanceFromPlayer = target.position - transform.position;
            targetPosition = target.position - distanceFromPlayer.SetMagnitude(deadzoneRadius);
        }
        // Move the camera to its target smoothly.
        newPosition = Vector2.SmoothDamp(transform.position, (Vector2)targetPosition, ref velocity, dampTime);
        // Lock the camera's depth
        newPosition.z = transform.position.z;
        
        transform.position = newPosition;
    }
    
    /// <summary>
    /// Makes the camera static when it hits the currents
    /// </summary>
    private void ApplyCameraBoundary()
    {
        Vector3 newPosition = Vector3.zero;
        particleDirection = current.CurrentParticleDirection();
        if (particleDirection == "") { return; }
        if(particleDirection == "downCurrent" || particleDirection == "upCurrent")
        {
            newPosition = this.transform.position;
            newPosition.x = target.transform.position.x;
        }
        if(particleDirection == "leftCurrent" || particleDirection == "rightCurrent")
        {
            newPosition = this.transform.position;
            newPosition.y = target.transform.position.y;
        }
        transform.position = newPosition;
    }
    
    /// <summary>
    /// Initializes the transform for the camera to target
    /// </summary>
    private void InitializeTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            target = player.GetComponent<Transform>();
        }
    }
    
    /// <summary>
    /// Triggers the camera to zoom out to the maximum value
    /// </summary>
    public void MaxZoomOut()
    {
        zoomManager.MaxZoomOut();
    }
    
    /// <summary>
    /// Resets the zoom timer
    /// </summary>
    public void ResetTimer()
    {
        zoomManager.ResetTimer();
    }
    
    /// <summary>
    /// The transform to follow
    /// </summary>
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }
    
    /// <summary>
    /// The camera target's rigidbody
    /// </summary>
    public Rigidbody PlayerRigidbody
    {
        get 
        {
            if (playerRigidbody == null ) { playerRigidbody =  target.GetComponent<Rigidbody>(); } 
            return playerRigidbody; 
        }
        set { playerRigidbody = value; }
    }
    
    /// <summary>
    /// The camera's z position
    /// </summary>
    public float ZPosition
    {
        get { return zPosition; }
    }
    
}