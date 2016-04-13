using UnityEngine;
using System.Collections;

/// <summary>
/// ???
///
/// @author - Karl C.
/// @version - 1.0.0
///
/// </summary>
public class ZoomZones : MonoBehaviour 
{
    private SmoothCamera smoothCamera;
    
    /// <summary>
    /// Initializes the camera target
    /// </summary>
    void Start () 
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        if (mainCamera != null)
        {
            this.smoothCamera = mainCamera.GetComponent<SmoothCamera>();
        }
    }
    
    /// <summary>
    /// Set zoom zone
    /// </summary>
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            this.smoothCamera.SetZoomInZone(true);
        }
    }
    
    /// <summary>
    /// Reset zoom zone
    /// </summary>
    void OnTriggerExit(Collider col) 
    {
        if(col.CompareTag("Player"))
        {
            this.smoothCamera.SetZoomInZone(false);
        }
    }
}
