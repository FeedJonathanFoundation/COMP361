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
public class Alignment : NPCActionable
{       
    public Alignment(int priority, string id) : base(priority, id)
    {
    }
    
	public override void Execute(Steerable steerable) 
    {
        base.Execute(steerable);
        
        steerable.AddAlignmentForce(strengthMultiplier);
    }
    
}