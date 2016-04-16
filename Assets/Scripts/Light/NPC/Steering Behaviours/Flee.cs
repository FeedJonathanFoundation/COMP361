using UnityEngine;
using System.Collections;

/// <summary>
/// Applies a fleeing steering force on an NPC.
/// Allows a fish to flee from the player
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class Flee : NPCActionable
{   
    [SerializeField]
    [Tooltip("If true, the fleeing behaviour stays active for 'timer' seconds")]
    public bool useTimer;
    [SerializeField]
    [Tooltip("The action stays active 'timer' seconds before being recycled")]
    private float timer;
    
    /** The transform to flee from */
    private Transform targetTransform;
    
    public Flee(int priority, string id, Transform transform) : base(priority, id)
    {
        targetTransform = transform;
    }
    
    /// <summary>
    /// Called every frame when this action needs to be performed.
    /// Applies a fleeing steering force on the given steerable
    /// </summary>
	public override void Execute(Steerable steerable) 
    {
        base.Execute(steerable);
        
        // If the action has elapsed its timer
        if (useTimer && timeActive > timer)
        {            
            // Inform subscribers that the action is completed. This stops the action's execution.
            ActionCompleted();
            
            // Reset the timer for the next time the action is performed.
            ResetTimer();
        }
        
        if (targetTransform)
        {            
            // If player's lights are on, seek player
            if (targetTransform.gameObject.CompareTag("Player")) 
            {
                Player player = targetTransform.gameObject.GetComponent<Player>();
                if (player.IsDetectable)
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
                }
                else
                {
                    // The player is hidden. Thus, the fish should stop fleeing
                    //ActionCompleted();
                }
            }
            else
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
            }
            
            steerable.AddFleeForce(targetTransform.position, strengthMultiplier);
        }
    }
    
    /// <summary>
    /// Returns true if this fleeing action can be cancelled
    /// before it is completed
    /// </summary>
    public override bool CanBeCancelled()
    {
        // Return true only if the timer is elapsed or if a timer isn't used.
        return (useTimer && timeActive > timer) || !useTimer;
    }
    
    /// <summary>
    /// The transform to flee
    /// </summary>
    public Transform TargetTransform
    {
        get { return targetTransform; }
        set { targetTransform = value; }
    }
}
