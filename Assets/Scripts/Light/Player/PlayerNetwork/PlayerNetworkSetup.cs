using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Instantiates a local camera for each player.
///
/// @author - Stella L.
/// @version - 1.0.0
///
/// </summary>
public class PlayerNetworkSetup : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerCamera;
    
    void Start()
    {
	    if (isLocalPlayer)
        {
            DisableSceneCamera();
            GetComponent<Player>().enabled = true;
            InstantiateCamera();
        }
	}
    
    /// <summary>
    /// Disables the unused scene camera
    /// </summary>
    private void DisableSceneCamera()
    {
        GameObject sceneCamera = GameObject.Find("Scene Camera");
        if (sceneCamera != null)
        {
            sceneCamera.SetActive(false);
        }
    }
    
    /// <summary>
    /// Instantiates a camera fr the local player
    /// </summary>
    private void InstantiateCamera()
    {
        SmoothCamera camera = ((GameObject)GameObject.Instantiate(playerCamera, transform.position, Quaternion.Euler(0,0,0))).GetComponent<SmoothCamera>();
        camera.Target = transform;
        camera.Init();
    }

}
