using UnityEngine;

/// <summary>
/// Modifies the GameObject based on its current amount of light energy
///
/// @author - Jonathan L.A
/// @author - Alex I.
/// @version - 1.0.0
///
/// </summary>
public abstract class LightEnergyListener : MonoBehaviour
{
    [Tooltip("The LightEnergy component which modifies the desired attribute." + 
    "If none specified, the LightEnergy attached to this GameObject is used.")]
    public LightEnergy lightEnergyOverride;
    protected LightSource lightSource;  // The LightSource being listened to
    private LightEnergy lightEnergy;

    protected virtual void Start()
    {
        lightSource = GetComponentInParent<LightSource>();
        
        // Choose either the override (if assigned in the Inspector) or the component
        // attached to this GameObject.
        if (lightEnergyOverride != null)
        {
            this.lightEnergy = lightEnergyOverride;
        }
        else if (lightSource)
        {
            this.lightEnergy = lightSource.LightEnergy;
        }
        else
        {
            this.lightEnergy = null;
        }

        Subscribe();
        // Initialize the attribute to the light's initial energy
        OnLightChanged(lightEnergy.CurrentEnergy);
    }

    /// <summary>
    /// Subscribe to OnLightChanged() event to track whenever 
    /// the GameObject's amount of light energy changes.
    /// </summary>
    public void Subscribe()
    {
        if (this.lightEnergy != null)
        {
            this.lightEnergy.LightChanged += OnLightChanged;
        }
    }

    /// <summary>
    /// Unsubscribe from OnLightChanged event
    /// </summary>
    public void Unsubscribe()
    {
        if (this.lightEnergy != null)
        {
            this.lightEnergy.LightChanged -= OnLightChanged;
        }
    }

    /// <summary>
    /// Calls Unsubscribe() once player is out of light enegry
    /// </summary>
    public void OnLightDepleted()
    {
        Unsubscribe();
    }

    /// <summary>
    /// Called by LightEnergy.cs when the amount of light energy owned by the
    /// GameObject changes.
    /// </summary>
    public abstract void OnLightChanged(float currentLight);

}