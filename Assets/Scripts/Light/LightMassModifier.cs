using UnityEngine;

/// <summary>
/// Changes the GameObject's mass based on its current amount of light energy.
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class LightMassModifier : LightEnergyListener
{
    [Tooltip("Theamount of energy points required to have a 1.0 mass")]
    [SerializeField]
    private float lightToMassRatio = 0.1f;

    [Tooltip("The minimum amount of mass this GameObject can have")]
    [SerializeField]
    private float minMass;

    private new Rigidbody rigidbody; // Always access through Rigidbody property

    /// <summary>
    /// Implementation of OnLightChanged() from LightEnergyListener
    /// Scales the rigidbody's mass based on its current amount of light
    /// </summary>
    /// <param name="currentLight"></param>
    public override void OnLightChanged(float currentLight)
    {
        Rigidbody.mass = currentLight * lightToMassRatio;
        Rigidbody.mass = Mathf.Max(minMass, Rigidbody.mass);
    }

    private Rigidbody Rigidbody
    {
        get
        {
            if (rigidbody == null) { rigidbody = GetComponent<Rigidbody>(); }
            return rigidbody;
        }
    }
}