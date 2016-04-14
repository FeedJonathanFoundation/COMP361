using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
/// Assigns a unique player ID to each player game object.
///
/// @author - Stella L.
/// @version - 1.0.0
///
/// </summary>
public class PlayerID : NetworkBehaviour
{

    [SyncVar]
    private string playerUniqueIdentity;
    private NetworkInstanceId playerNetworkID;
    private Transform playerTransform;
    [Tooltip("What the game object is called when it is cloned.")]
    [SerializeField]
    private string playerPrefabCloneName = "Player(Clone)";

    public override void OnStartLocalPlayer()
    {
        GetNetIdentity();
        SetIdentity();
    }
    
    void Awake()
    {
        playerTransform = transform;
    }
    
    /// <summary>
    /// If player ID has not been set, set identity.
    /// </summary>
    void Update()
    {
        if (playerTransform.name == "" || playerTransform.name == playerPrefabCloneName)
        {
            SetIdentity();
        }
    }
    
    /// <summary>
    /// Retrieves and syncs the identity with the server.
    /// </summary>
    [Client]
    void GetNetIdentity()
    {
        playerNetworkID = GetComponent<NetworkIdentity>().netId;
        CmdTellServerMyIdentity(MakeUniqueIdentity());
    }

    /// <summary>
    /// Sets the player ID to a unique identity.
    /// </summary>
    [Client]
    void SetIdentity()
    {
        if (!isLocalPlayer)
        {
            playerTransform.name = playerUniqueIdentity;
        }
        else
        {
            playerTransform.name = MakeUniqueIdentity();
        }
    }

    /// <summary>
    /// Creates a unique identity.
    /// </summary>
    string MakeUniqueIdentity()
    {
        string uniqueIdentity = "Player" + playerNetworkID.ToString();
        return uniqueIdentity;
    }
    
    /// <summary>
    /// Syncs the player identity with the server.
    /// </summary>
    [Command]
    void CmdTellServerMyIdentity(string identity)
    {
        playerUniqueIdentity = identity;
    }

}
