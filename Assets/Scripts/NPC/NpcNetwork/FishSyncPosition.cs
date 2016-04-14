using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Syncs fish positions between the server and its clients.
///
/// @author - Stella L.
/// @version - 1.0.0
///
/// </summary>
public class FishSyncPosition : NetworkBehaviour
{

	[SyncVar(hook = "SyncPositionValues")]
    private Vector3 syncPosition;
    [SerializeField]
    private Transform objectTransform;
    private float lerpRate;
    [SerializeField]
    private float normalLerpRate = 15f;
    [SerializeField]
    private float fasterLerpRate = 25f;
    private Vector3 lastPosition;
    private float threshold = 0.5f;
    private List<Vector3> syncPositionList = new List<Vector3>();
    [SerializeField]
    private bool useSavedLerping = false;
    private float buffer = 0.1f;

    void Start()
    {
        lerpRate = normalLerpRate;
        if (objectTransform == null)
        {
            objectTransform = transform;
        }
    }

    void Update()
    {
        TransmitPosition();
        LerpPosition();
    }
    
    /// <summary>
    // Only used for other clients to smooth out movement
	/// </summary>
    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            if (useSavedLerping)
            {
                SavedLerping();
            }
            else
            {
                OrdinaryLerping();
            }
        }
    }
    
    /// <summary>
    // Send the position value to the serverserver
	/// </summary>
    [Command]
    void CmdProvidePositionToServer(Vector3 position)
    {
        syncPosition = position;
    }
    
    /// <summary>
    /// Transmits the position value to all clients.
    /// Only sends commands if the player has moved at least the threshold value.
	/// </summary>
    
    [ClientCallback]
    void TransmitPosition()
    {
        if (Vector3.Distance(objectTransform.position, lastPosition) > threshold)
        {
            CmdProvidePositionToServer(objectTransform.position);
            lastPosition = objectTransform.position;
        }
    }
    
    /// <summary>
	/// Saves position values to use in saved lerping.
	/// </summary>
    [Client]
    void SyncPositionValues(Vector3 latestPosition)
    {
        syncPosition = latestPosition;
        syncPositionList.Add(syncPosition);
    }
    
    /// <summary>
	/// Syncs real-time values without compensation.
	/// </summary>
    void OrdinaryLerping()
    {
        objectTransform.position = Vector3.Lerp(objectTransform.position, syncPosition, Time.deltaTime * lerpRate);
    }
    
    /// <summary>
	/// Smooths out position values by using historical positions.
	/// </summary>
    void SavedLerping()
    {
        if (syncPositionList.Count > 0)
        {
            objectTransform.position = Vector3.Lerp(objectTransform.position, syncPositionList[0], Time.deltaTime * lerpRate);
            if (Vector3.Distance(objectTransform.position, syncPositionList[0]) < buffer)
            {
                syncPositionList.RemoveAt(0);
            }
            if (syncPositionList.Count > 10)
            {
                lerpRate = fasterLerpRate;
            }
        }
    }
}
