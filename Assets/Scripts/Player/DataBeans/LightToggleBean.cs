using UnityEngine;

[System.Serializable]
public class LightToggleBean
{   
    [Tooltip("If true, the lights are enabled on scene start")]
    public bool defaultLightStatus = true;
    
    [Tooltip("Time interval for energy depletion while lights are on")]
    public float timeToDeplete = 0;
   
    [Tooltip("Amount of light lost while lights are turned on")]
    public float lightToggleEnergyCost = 0;
   
    [Tooltip("Energy needed to activate light and that light will turn off if reached")]
    public float minimalEnergyRestrictionToggleLights = 0;
   
    [Tooltip("The percent range of the players lights when propulsing with lights off")]
    public float propulsionLightRange = 0.3f;
    
}