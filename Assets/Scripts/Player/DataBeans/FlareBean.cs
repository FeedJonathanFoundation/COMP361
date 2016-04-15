using UnityEngine;

/// <summary>
/// Stores the flare related variables imported from Unity Editor in a single
/// self-contained class. Primitives are initialized with default values, 
/// that are overwriten from Unity Editor.
///
/// @author - Alex I.
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class FlareBean
{
    [Tooltip("Time before another flare can be spawned")]
    [SerializeField]
    private float coolDown = 3f;
    
    [Tooltip("Energy cost to shoot a flare")]
    [SerializeField]
    private float flareEnergyCost = 5f;
    
    [Tooltip("Flare cost percentage")]
    [SerializeField]
    private float flareCostPercentage = 0.2f;
    
    [Tooltip("Recoil force")]
    [SerializeField]
    private float recoilForce = 1f;
    
    [Tooltip("Refers to the flare spawn zone")]
    [SerializeField]
    private Transform flareSpawnObject;

    public float CoolDown
    { 
        get { return coolDown;}
    }
    
    public float FlareEnergyCost
    { 
        get { return flareEnergyCost;}
    }
    
    public float FlareCostPercentage
    { 
        get { return flareCostPercentage;}
    }
    
    public float RecoilForce
    { 
        get { return recoilForce;}
    }
    
    public Transform FlareSpawnObject
    { 
        get { return flareSpawnObject;}
    }

}