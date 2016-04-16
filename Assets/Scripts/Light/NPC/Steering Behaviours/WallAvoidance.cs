using UnityEngine;
using System.Collections;

/// <summary>
/// If active, an NPC will avoid walls in the chosen 'obstacleLayer'
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class WallAvoidance : NPCActionable
{   
    [Tooltip("The amount of force applied in order to avoid the nearest obstacle.")]
    [SerializeField]
    private float avoidanceForce;
    
    [Tooltip("Only obstacles within 'maxViewDistance' meters of the steerable can be avoided")]
    [SerializeField]
    private float maxViewDistance;
    
    [Tooltip("The layer which contains the colliders that can be avoided.")]
    [SerializeField]
    private LayerMask obstacleLayer;
    
    public WallAvoidance(int priority, string id) : base(priority, id)
    {
    }
    
    /// <summary>
    /// Executes this action on the given steerable, making him avoid obstacles
    /// on the chosen 'obstacleLayer'.
    /// </summary>
	public override void Execute(Steerable steerable) 
    {
        base.Execute(steerable);
        
        steerable.AddWallAvoidanceForce(avoidanceForce, maxViewDistance, obstacleLayer, strengthMultiplier);
    }
    
    /// <summary>
    /// The amount of force applied in order to avoid the nearest obstacle
    /// </summary>
    public float AvoidanceForce
    {
        get { return avoidanceForce; }
        set { avoidanceForce = value; } 
    }
    
    /// <summary>
    /// Only obstacles within 'maxViewDistance' meters of the steerable can be avoided
    /// </summary>
    public float MaxViewDistance
    {
        get { return maxViewDistance; }
        set { maxViewDistance = value; }
    }
    
    /// <summary>
    /// The layer which contains the colliders that can be avoided
    /// </summary>
    public LayerMask ObstacleLayer
    {
        get { return obstacleLayer; }
        set { obstacleLayer = value; }
    }
    
}
