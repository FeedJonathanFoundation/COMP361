using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/// <summary>
/// Assigns a unique ID to NPCs so that the server can distinguish 
/// between the NPCs that are instantiated and destroyed.
///
/// @author - Stella L.
/// @version - 1.0.0
///
/// </summary>
public class NpcID : NetworkBehaviour
{

    [SyncVar]
    private string npcID;
    private Transform npcTransform;
    [SerializeField]
    private string npcPrefabName = "Network";
    private bool identified = false;

    void Start()
    {
        npcTransform = transform;
    }

	void Update()
    {
        if (!identified)
        {
            SetIdentity();
        }
    }
    
    /// <summary>
    /// Assign NPC Name
    /// </summary>
    void SetIdentity()
    {
        if (npcTransform.name == "" || npcTransform.name.Contains(npcPrefabName))
        {
            if (npcID != null && npcID != "")
            {
                npcTransform.name = npcID;
            }
        }
        identified = true;
    }
    
    public string ID
    {
        get { return npcID;  }
        set { npcID = value; }
    }  
    
}