using UnityEngine;
using System.Collections;

/// <summary>
/// Allows fish to follow each other's swimming direction
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class Alignment : NPCActionable
{       
    public Alignment(int priority, string id) : base(priority, id)
    {
    }
    
    /// <summary>
    /// Called every frame when the action needs to be performed.
    /// Applies a steering force on the given steerable
    /// </summary>
	public override void Execute(Steerable steerable) 
    {
        base.Execute(steerable);
        
        steerable.AddAlignmentForce(strengthMultiplier);
    }
    
}