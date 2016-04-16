using UnityEngine;


/// <summary>
/// Moves the boss fish to the closest waypoint when the
/// player is not in sight
///
/// @author - Karl C.
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class MoveClosestWaypoint : NPCActionable
{
    [Tooltip("The list of waypoints that the big fish can go when the player is in a safe zone")]
    [SerializeField]
    private GameObject waypointList;

    [Tooltip("Distance at which then big fish slows down before getting to waypoint")]
    [SerializeField]
    private float slowingRadius;
    
    /** Holds a reference to the boss fish to move */
    private Transform bigFish;

    [Tooltip("The steering behaviour which allows the steerable to avoid walls")]
    [SerializeField]
    private WallAvoidance wallAvoidance;

    private Transform bigFish;

    public MoveClosestWaypoint(int priority, string id, Transform boss) : base(priority, id)
    {
        this.SetPriority(priority);
        this.SetID(id);
        this.bigFish = boss;
    }
    
    /// <summary>
    /// Sets the priorities of each steering behaviour.
    /// This priority is used to index the behaviours in a priority dictionary
    /// </summary>
    public void SetPriority(int priority)
    {
        this.priority = priority;
        wallAvoidance.Priority = priority;
    }

    /// <summary>
    /// Updates the ID for each internal steering behaviour.
    /// Allows the action to be referenced by a unique index
    /// </summary>
    public void SetID(string id)
    {
        this.id = id;
        wallAvoidance.Id = id;
    }
    
    /// <summary>
    /// Sets the reference to the boss fish
    /// </summary>
    public void SetBigFish(Transform boss)
    {
        this.bigFish = boss;
    }

    
    /// <summary>
    /// Avoids the nearest obstacle in front of the object. This works for obstacles of any size or shape, unlike
    /// Obstacle Avoidance, which approximates obstacles as spheres. Formal name: "Containment" or "Generalized Obstacle Avoidance"
    /// </summary>
    /// <param name="avoidanceForce">The amount of force applied in order to avoid the nearest obstacle.</param>
    /// <param name="maxViewDistance">Only obstacles within 'maxViewDistance' meters of this steerable can be avoided.</param>
    /// <param name="obstacleLayer">The layer which contains the colliders that can be avoided.</param>
    public void SetWallAvoidanceProperties(float strengthMultiplier, float avoidanceForce, float maxViewDistance, LayerMask obstacleLayer)
    {
        wallAvoidance.strengthMultiplier = strengthMultiplier;
        wallAvoidance.AvoidanceForce = avoidanceForce;
        wallAvoidance.MaxViewDistance = maxViewDistance;
        wallAvoidance.ObstacleLayer = obstacleLayer;
    }

    /// <summary>
    /// Called every frame when this action needs to be performed.
    /// Allows the steerable to move to the closest waypoint
    /// </summary>
    public override void Execute(Steerable steerable)
    {
        // Override the steerable's min/max speed
        if (overrideSteerableSpeed)
        {
            steerable.MinSpeed = minSpeed;
            steerable.MaxSpeed = maxSpeed;
        }
        // Override the steerable's max force
        if (overrideMaxForce)
        {
            steerable.MaxForce = maxForce;
        }

        //checks if the boss fish is at the waypoint, if so then no need to move to waypoint anymore
        if ((Vector2)bigFish.GetComponent<Rigidbody>().velocity == Vector2.zero)
        {
            ActionCompleted();
        }
        else
        {
            steerable.AddMoveWaypointForce(waypointList, bigFish, slowingRadius, strengthMultiplier);
        }

        wallAvoidance.Execute(steerable);
    }
}