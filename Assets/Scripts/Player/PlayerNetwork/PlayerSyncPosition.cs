﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

// MAY NEED TO UPDATE LERP RATE VALUES, BUFFER VALUE

[NetworkSettings (channel = 0, sendInterval = 0.033f)]
public class PlayerSyncPosition : NetworkBehaviour
{
    [SyncVar(hook = "SyncPositionValues")]
    private Vector3 syncPosition;
    [SerializeField]
    private Transform playerTransform;
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
    }

    void Update()
    {
        TransmitPosition();
        LerpPosition();
    }
    
    // Used to provide player's position
    // Only used for other clients to smooth out movement
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
    
    // Server receives this value
    [Command]
    void CmdProvidePositionToServer(Vector3 position)
    {
        syncPosition = position;
    }
    
    // Transmits this value to all clients
    // Only sends commands if the player has moved at least the threshold value
    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(playerTransform.position, lastPosition) > threshold)
        {
            CmdProvidePositionToServer(playerTransform.position);
            lastPosition = playerTransform.position;
        }
    }
    
    [Client]
    void SyncPositionValues(Vector3 latestPosition)
    {
        syncPosition = latestPosition;
        syncPositionList.Add(syncPosition);
    }
    
    void OrdinaryLerping()
    {
        playerTransform.position = Vector3.Lerp(playerTransform.position, syncPosition, Time.deltaTime * lerpRate);
    }
    
    void SavedLerping()
    {
        if (syncPositionList.Count > 0)
        {
            playerTransform.position = Vector3.Lerp(playerTransform.position, syncPositionList[0], Time.deltaTime * lerpRate);
            if (Vector3.Distance(playerTransform.position, syncPositionList[0]) < buffer)
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