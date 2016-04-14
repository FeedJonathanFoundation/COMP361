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
public class Cohesion : NPCActionable
{       
    public Cohesion(int priority, string id) : base(priority, id)
    {
    }
    
	public override void Execute(Steerable steerable) 
    {
        base.Execute(steerable);
        
        steerable.AddCohesionForce(strengthMultiplier);
    }
    
}
