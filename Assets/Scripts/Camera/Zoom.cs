using UnityEngine;
using System.Collections;

/// <summary>
/// The zoom class calculates and applies camera zoom.
///
/// @author - Karl C.
/// @version - 1.0.0
///
/// </summary>
[RequireComponent(typeof(SmoothCamera))]
public class Zoom : MonoBehaviour
{
    
    [SerializeField]
    [Tooltip("The small speed value")]
    private float speedZoomSmall;
    [SerializeField]
    [Tooltip("The medium speed value")]
    private float speedZoomMedium;
    [SerializeField]
    [Tooltip("Z value for camera when player has small speed")]
    private float smallZoomValue;
    [SerializeField]
    [Tooltip("Z value for camera when player has medium speed")]
    private float mediumZoomValue;
    [SerializeField]
    [Tooltip("Z value for camera on flare launch")]
    private float maxZoomValue;
    [SerializeField]
    [Tooltip("Z value for camera in zoom zones")]
    private float zoomZonesValue;
    [SerializeField]
    [Tooltip("Amount of time before camera zooms back into the player")]
    private float timeBeforeZoomIn;
    [SerializeField]
    [Tooltip("Camera zoom in speed")]
    private float zoomInSpeed;
    [SerializeField]
    [Tooltip("Camera zoom out speed")]
    private float zoomOutSpeed;
    private bool acquiredZoom;
    private float zoomTimer;
    private bool maxZoomOut = false;
    private bool zoomInZone = false;
    private Current current;
    [SerializeField]
    [Tooltip("The object that the camera follows.")]
    private Transform target;
    private SmoothCamera smoothCamera;

    /// <summary>
    /// Initializes the zoom parameters
    /// </summary>
    void Start()
    {
        RevertTimer();

        GameObject currentObject = GameObject.FindGameObjectWithTag("Current");
        if (currentObject != null)
        {
            current = currentObject.GetComponent<Current>();
        }

        smoothCamera = this.GetComponent<SmoothCamera>();
    }
	
	/// <summary>
    /// Checks what zoom state should be applied
    /// </summary>
	void FixedUpdate()
    {
        float playerVelocity = smoothCamera.PlayerRigidbody.velocity.sqrMagnitude;

        if (playerVelocity == 0f) { return; }
        
        Vector3 newPosition = Vector3.zero;
        acquiredZoom = false;
             
        if (zoomInZone)
        {
            if (EnterZoomZone() != Vector3.zero)
            {
                newPosition = EnterZoomZone();
            }
        }
        if (maxZoomOut && !acquiredZoom)
        {
            if(maxZoomValue != newPosition.z)
            {
                newPosition.z = CameraZoom(zoomOutSpeed, maxZoomValue);
                acquiredZoom = true;
            }
            else
            {
                maxZoomOut = false;
            }
        }
        if (zoomTimer < timeBeforeZoomIn && !maxZoomOut)
        {
            zoomTimer += Time.deltaTime;
            acquiredZoom = true;
        } 
        if (ApplyZoom(playerVelocity) != Vector3.zero)
        {
            ApplyZoom(playerVelocity);
        }
        transform.position = newPosition;
	}
    
    /// <summary>
    /// The camera target's rigidbody
    /// </summary>
    private Vector3 EnterZoomZone()
    {
        Vector3 newPosition = Vector3.zero;
        if (!current.PlayerInCurrents())
        {
            if(zoomZonesValue != newPosition.z)
            {
                newPosition.z = CameraZoom((newPosition.z > zoomZonesValue? zoomOutSpeed : zoomInSpeed), zoomZonesValue);
            }
            acquiredZoom = true;
        }
        return newPosition;
    }
    
    /// <summary>
    /// Sets the camera zoom settings
    /// </summary>
    private Vector3 ApplyZoom(float playerVelocity)
    {
        float mediumSpeed = speedZoomMedium * speedZoomMedium;
        float smallSpeed = speedZoomSmall * speedZoomSmall;
        Vector3 newPosition = Vector3.zero;
        if (!acquiredZoom)
        {
            Vector3 compare = Vector3.zero;
            if (playerVelocity > mediumSpeed)
            {
                if (smoothCamera.ZPosition != newPosition.z)
                {
                    newPosition.z = CameraZoom((newPosition.z > mediumZoomValue ? zoomOutSpeed : zoomInSpeed), mediumZoomValue);
                }
            }
            else if (playerVelocity > smallSpeed) 
            {
                if (smallZoomValue != newPosition.z)
                {
                    newPosition.z = CameraZoom((newPosition.z > smallZoomValue ? zoomOutSpeed : zoomInSpeed), smallZoomValue);
                }
            }
            else // less than small speed
            {
                if (mediumZoomValue != newPosition.z)
                {
                    newPosition.z = CameraZoom(zoomInSpeed, smoothCamera.ZPosition);
                }
            }
            acquiredZoom = true;
        }
        return newPosition;

    }
    
    /// <summary>
    /// Calculates the new camera position
    /// </summary>
    public float CameraZoom(float zoomSpeed, float zoomToValue)
    {
        float zValue = Mathf.Lerp(this.transform.position.z, zoomToValue, Time.deltaTime * zoomSpeed);
        float roundedValue = Mathf.Round(zValue * 100f) / 100f;
        if (roundedValue == (Mathf.Round(zoomToValue * 100f) / 100f))
        {
            return zoomToValue;
        }
        else
        {
            return zValue;
        }  
    }
    
    public void MaxZoomOut()
    {
        //need to check that we aren't in a zoomInZone
        if(!zoomInZone && !current.PlayerInCurrents())
        {
            this.maxZoomOut = true;
        }
    }
    
    public void ResetTimer()
    {
        this.zoomTimer = 0.0f;
        this.maxZoomOut = false;
    }
    
    public void RevertTimer()
    {
        this.zoomTimer = timeBeforeZoomIn;
        this.maxZoomOut = false;
    }
    
    public void SetZoomInZone(bool isZoom)
    {
        this.zoomInZone = isZoom;
        // Need to reset if flare was shot before entering zoomInZone
        RevertTimer();
    }

}
