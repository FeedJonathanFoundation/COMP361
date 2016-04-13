using UnityEngine;
using System.Collections;

/// <summary>
/// Triggers the zoom manager to apply camera zoom upon collision.
///
/// @author - Karl C.
/// @version - 1.0.0
///
/// </summary>
public class ZoomZones : MonoBehaviour 
{
    private Zoom zoomManager;
    
    /// <summary>
    /// Initializes the camera target
    /// </summary>
    void Start () 
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        if (mainCamera != null)
        {
            this.zoomManager = mainCamera.GetComponent<Zoom>();
        }
    }
    
    /// <summary>
    /// Set zoom zone
    /// </summary>
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            this.zoomManager.SetZoomInZone(true);
        }
    }
    
    /// <summary>
    /// Reset zoom zone
    /// </summary>
    void OnTriggerExit(Collider col) 
    {
        if(col.CompareTag("Player"))
        {
            this.zoomManager.SetZoomInZone(false);
        }
    }
}
