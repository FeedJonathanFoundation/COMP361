using UnityEngine;

/// <summary>
/// Changes the GameObject's sphere collider properties
/// 
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class LightSphereColliderModifier : LightEnergyListener
{
    [Tooltip("The amount of light energy required to have a SphereCollider radius of 1")]
    [SerializeField]
    private float lightToRadiusRatio;

    [Tooltip("The collider's radius is multiplied by this constant if the player is thrusting.")]
    [SerializeField]
    private float thrustRadiusMultiplier;
    
    private SphereCollider sphereCollider;
    private Player player;
    private PlayerLightToggle playerLightToggle;

    protected override void Start()
    {
        base.Start();
        sphereCollider = GetComponent<SphereCollider>();
        // Move the collider somewhere no fish will ever see on game start
        sphereCollider.center = new Vector3(1000000, 1000000, 100000);
        if (lightSource is Player) { player = (Player)lightSource; }
    }

    protected void Update()
    {
        // Compute the sphere collider's radius based on the parent light source's energy
        float colliderRadius = lightSource.LightEnergy.CurrentEnergy * lightToRadiusRatio;
        Vector3 colliderCenter = Vector2.zero;

        if (player != null)
        {
            // If the player's lights are turned off, disable the sphere. Fish should not be able to detect the player.
            if (!player.IsDetectable)
            {
                colliderRadius = 0;
                // Move the collider somewhere no fish will ever see
                colliderCenter = new Vector3(1000000, 1000000, 100000);
            }
            if (player.Movement.Thrusting) { colliderRadius *= thrustRadiusMultiplier; }
        }

        sphereCollider.radius = colliderRadius;
        sphereCollider.center = colliderCenter;
    }
    
    /// <summary>
    /// Implementation of OnLightChanged() from LightEnergyListener
    /// </summary>
    /// <param name="currentLight"></param>
    public override void OnLightChanged(float currentLight)
    {
        sphereCollider.radius = currentLight * lightToRadiusRatio;
    }
    
}