using UnityEngine;
using System.Collections;

/// <summary>
/// Steering behaviour which allows fish to swim close together
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class Cohesion : NPCActionable
{       
    public Cohesion(int priority, string id) : base(priority, id)
    {
    }
    
    /// <summary>
    /// Called every frame when the action needs to be performed.
    /// Applies a cohesion steering force on the given steerable
    /// </summary>
	public override void Execute(Steerable steerable) 
    {
        base.Execute(steerable);
        
        steerable.AddCohesionForce(strengthMultiplier);
    }
    
}
