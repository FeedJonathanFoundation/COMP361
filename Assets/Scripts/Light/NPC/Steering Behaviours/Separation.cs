using UnityEngine;
using System.Collections;

/// <summary>
/// If active, allows an NPC to separate from its neighbouring
/// NPCs 
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class Separation : NPCActionable
{       
    public Separation(int priority, string id) : base(priority, id)
    {
    }
    
    /// <summary>
    /// Adds a force which steers the steerable away from its neighbours
    /// </summary>
	public override void Execute(Steerable steerable) 
    {
        base.Execute(steerable);   
        
        steerable.AddSeparationForce(strengthMultiplier);   
    }
    
}
