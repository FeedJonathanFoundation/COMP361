using UnityEngine;
using System.Collections;

/// <summary>
/// Modifies the range of a Light component based on the amount of light energy
/// stored in a LightEnergy component.
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
public class LightRangeModifier : LightEnergyListener
{
        
    [Tooltip("The higher the value, the larger the range of light per unit of light energy")]    
    [SerializeField]
    private float lightToRangeRatio;
    
    [SerializeField]
    private float maxIntensity;
    
    [SerializeField]
    private float minIntensity;
    
    [SerializeField]
    private float lightToIntensityRatio = 0.05f;
    
    [Tooltip("The speed at which the light's range changes when the light is turned on")]
    [SerializeField]
    private float rangeChangeSpeed = 0.5f;
    [Tooltip("The speed at which the light's intensity changes when the light is turned on")]
    [SerializeField]
    private float intensityChangeSpeed = 0.5f;
   
    private float percentRange; //The percentage of range of the lights. 0 = zero range. 1 = normal range
    private float targetRange;
    private float targetIntensity;
    private new Light light;
    
    protected override void Start()
    {
        base.Start();        
        TurnOffLightImmediate();
    }
     
    protected void Update()
    {
        if (!ActiveLights) { TurnOffLight(); }
        Light.range = Mathf.Lerp(Light.range, targetRange, rangeChangeSpeed * Time.deltaTime);
        Light.intensity = Mathf.Lerp(Light.intensity, targetIntensity, intensityChangeSpeed * Time.deltaTime);
    }
    
    /// <summary>
    /// Implementation of OnLightChanged() from LightEnergyListener.
    /// Modifies the range of the attached light component based on the current 
    /// amount of light energy.
    /// </summary>
    /// <param name="currentLight"></param>
    public override void OnLightChanged(float currentLight)
    {        
        float newRange = currentLight * lightToRangeRatio * percentRange;
        float newIntensity = maxIntensity * lightToIntensityRatio * currentLight * percentRange;
       
        // Cap the light's intensity
        if (newIntensity >= maxIntensity) 
        {
             newIntensity = maxIntensity; 
        }
        else 
        {
            newIntensity = minIntensity;
        }
        
        targetRange = newRange;
        targetIntensity = newIntensity;
    }
    
    /// <summary>
    /// Set player's light range
    /// </summary>
    /// <param name="range"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    private IEnumerator SetRange(float range, float speed)
    {
        while (Mathf.Abs(Light.range - range) > 0.01f)
        {
            Light.range = Mathf.Lerp(Light.range, targetRange, speed * Time.deltaTime);
            yield return null;
        }
    }
    
    /// <summary>
    /// Set player's light intensity
    /// </summary>
    /// <param name="intensity"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    private IEnumerator SetIntensity(float intensity, float speed)
    {
        while (Mathf.Abs(Light.intensity - intensity) > 0.01f)
        {
            Light.intensity = Mathf.Lerp(Light.intensity, targetIntensity, speed * Time.deltaTime);
            yield return null;
        }
    }
    
    /// <summary>
    /// Turn off player's lights
    /// </summary>
    public void TurnOffLight()
    {
        targetRange = 0;
        targetIntensity = 0;
    }
    
    /// <summary>
    /// Turn off lights of player's child components
    /// </summary>
    private void TurnOffLightImmediate()
    {
        Light.range = 0;
        Light.intensity = 0;
    }
    
    public float PercentRange
    {
        get { return percentRange;   }
        set {  percentRange = value; }
    }
        
    public bool ActiveLights
    {
        //If false, the light's intensity is set to zero. Otherwise, the light behaves normally
        get; set;
    }

    private Light Light
    {
        get
        {
            if (light == null) { light = GetComponent<Light>(); }
            return light;
        }
    }
}
