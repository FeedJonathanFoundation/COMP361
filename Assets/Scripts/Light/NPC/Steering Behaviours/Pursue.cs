using UnityEngine;
using System.Collections;

/// <summary>
/// Allows a fish to pursue other GameObjects
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class Pursue : NPCActionable
{   
    /** The Transform to pursue. */
    private Transform targetTransform;
    
    /// <summary>
	/// The steerable that this steering behavior is targetting
	/// </summary>
	public Steerable targetSteerable;
    
    public Pursue(int priority, string id, Steerable targetSteerable) : base(priority, id)
    {
        this.targetSteerable = targetSteerable;
    }
    
    /// <summary>
    /// Makes the steerable pursue the 'targetTransform' instance
    /// </summary>
	public override void Execute(Steerable steerable) 
    {
        base.Execute(steerable);
        
        if (targetSteerable)
        {
            steerable.AddPursueForce(targetSteerable, strengthMultiplier);
        }
    }
    
}
