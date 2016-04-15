using UnityEngine;

/// <summary>
/// Stores the light toggle related variables imported from Unity Editor in a 
/// single self-contained class. Primitives are initialized with default values, 
/// that are overwriten from Unity Editor.
///
/// @author - Alex I.
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class LightToggleBean
{   
    [Tooltip("If true, the lights are enabled on scene start")]
    [SerializeField]
    private bool defaultLightStatus = true;
    
    [Tooltip("Time interval for energy depletion while lights are on")]
    [SerializeField]
    private float timeToDeplete = 0;
   
    [Tooltip("Amount of light lost while lights are turned on")]
    [SerializeField]
    private float lightToggleEnergyCost = 0;
   
    [Tooltip("Energy needed to activate light and that light will turn off if reached")]
    [SerializeField]
    private float minimalEnergy = 0;
   
    [Tooltip("The percent range of the players lights when propulsing with lights off")]
    [SerializeField]
    private float propulsionLightRange = 0.3f;
    
    public bool DefaultLightStatus
    {
        get { return defaultLightStatus; }
    }

    public float TimeToDeplete
    {
        get { return timeToDeplete; }
    }
    
    public float LightToggleEnergyCost
    {
        get { return lightToggleEnergyCost; }
    }
    
    public float MinimalEnergy
    {
        get { return minimalEnergy; }
    }
    
    public float PropulsionLightRange
    {
        get { return propulsionLightRange; }
    }
    
}