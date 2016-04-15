using UnityEngine;

/// <summary>
/// Scales the GameObject based on its current amount of light energy
/// </summary>
public class LightScaleModifier : LightEnergyListener
{
    [Tooltip("The larger the value, the bigger the scale of the GameObject per light unit")]
    [SerializeField]
    private float lightToScaleRatio = 0.1f;

    /// <summary>
    /// Called by LightEnergy.cs when the amount of light energy owned by the
    /// GameObject changes.
    /// </summary>
    public override void OnLightChanged(float currentLight)
    {
        // Update the GameObject's scale based on its current amount of energy
        float newScale = currentLight * lightToScaleRatio;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}