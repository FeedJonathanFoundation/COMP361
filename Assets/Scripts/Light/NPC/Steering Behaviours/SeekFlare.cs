using UnityEngine;
using System.Collections;

/// <summary>
/// If active, this action allows a fish to seek the flare
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class SeekFlare : NPCActionable
{    
    /// <summary>
	/// The light that this NPC will seek
	/// </summary>
    private Transform targetFlare;

    /// <summary>
    /// The arrival (seek) action performed when the NPC is seeking the light.
    /// </summary>
    [SerializeField]
    private Arrival arrivalForce;
    
    /// <summary>
    /// The wall avoidance action performed when the NPC is seeking the light.
    /// </summary>
    [SerializeField]
    private WallAvoidance wallAvoidance;
    
    public SeekFlare(int priority, string id, Transform targetFlare) : base(priority, id)
    {
        this.targetFlare = targetFlare;
        
        SetPriority(priority);
        SetID(id);
    }
    
    /// <summary>
    /// Call this method in the Start() function of the fish performing this action. 
    /// </summary>
    public void Init()
    {
        // Call ChildActionComplete() when either the seek or flee actions are completed.
        arrivalForce.ActionComplete += ChildActionComplete;
        wallAvoidance.ActionComplete += ChildActionComplete;
    }
    
    /// <summary>
    /// Sets the priorities of each steering behaviour.
    /// This priority is used to index the behaviours in a priority dictionary
    /// </summary>
    public void SetPriority(int priority)
    {
        this.priority = priority;
        
        arrivalForce.Priority = priority;
        wallAvoidance.Priority = priority;
    }
    
    /// <summary>
    /// Updates the ID for each internal steering behaviour.
    /// Allows the action to be referenced by a unique index
    /// </summary>
    public void SetID(string id)
    {
        this.id = id;
        
        arrivalForce.Id = id;
        wallAvoidance.Id = id;
    }
    
     /// <summary>
    /// Makes the steerable seek the currently-set flare instance
    /// </summary>
	public override void Execute(Steerable steerable) 
    {
        base.Execute(steerable);
        
        if (targetFlare)
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
            
            // Seek the light source
            arrivalForce.Execute(steerable);
        }
        else
        {
            // If the flare has been destroyed, stop seeking it
            ActionCompleted();
        }
        
        wallAvoidance.Execute(steerable);
    }
    
    /// <summary>
    /// If true, this action can be cancelled before it's completed
    /// </summary>
    public override bool CanBeCancelled()
    {
        return arrivalForce.CanBeCancelled() && wallAvoidance.CanBeCancelled();
    }
    
    /// <summary>
    /// Called when a child action is complete to notify subscribers that the parent action is also complete 
    /// </summary>
    private void ChildActionComplete(NPCActionable childAction)
    {
        // Call ActionCompleted() to notify subscribers that the parent action is complete
        ActionCompleted();
    }
    
    /// <summary>
	/// The light that this NPC will seek or flee
	/// </summary>
    public Transform TargetFlare
    {
        get { return targetFlare; }
        set 
        { 
            targetFlare = value; 
            
            arrivalForce.TargetTransform = value.transform;
        }
    }
    
}
