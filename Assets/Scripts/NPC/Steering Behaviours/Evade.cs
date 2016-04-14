using UnityEngine;
using System.Collections;

/// <summary>
/// ????
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class Evade : NPCActionable
{   
    
    /// <summary>
	/// The steerable that this steering behavior is targetting
	/// </summary>
	public Steerable targetSteerable;
    
    public Evade(int priority, string id, Steerable targetSteerable) : base(priority, id)
    {
        this.targetSteerable = targetSteerable;
    }
    
	public override void Execute(Steerable steerable) 
    {
        base.Execute(steerable);
        
        if (targetSteerable)
        {
            steerable.AddEvadeForce(targetSteerable, strengthMultiplier);
        }
    }
    
}
