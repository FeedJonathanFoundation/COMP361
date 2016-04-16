using UnityEngine;
using System.Collections;

/// <summary>
/// If active, a chosen NPC will wander in a random direction smoothly
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class Wander : NPCActionable 
{
    
    [Tooltip("Wander: The greater this value, the stronger the wander force, and the more likely the entity will change directions whilst moving.")]
	[SerializeField]
    private float circleDistance = 1f;
    
    [Tooltip("Wander: The greater the radius, the stronger the wander force, and the more likely the entity will change directions")]
	[SerializeField]
    private float circleRadius = .5f;
    
    [Tooltip("Wander: The maximum angle in degrees that the wander force can change between two frames")]
	[SerializeField]
    private float angleChange = 30f;
    
    public Wander(int priority, string id) : base(priority, id) 
    {
    }
    
    /// <summary>
    /// Executes the wander action on the given steerable, making it 
    /// wander in a random direction
    /// </summary>
	public override void Execute(Steerable steerable) 
    {      
        base.Execute(steerable);
          
        steerable.AddWanderForce(circleDistance, circleRadius, angleChange, strengthMultiplier);
    }
    
    /// <summary>
	/// The distance from the entity to the wander circle. The greater this value, the stronger the wander force, 
	/// and the more likely the entity will change directions. 
	/// </summary>
    public float CircleDistance
    {
        get { return circleDistance; }
        set { circleDistance = value; }
    }
    
	/// <summary>
	/// The greater the radius, the stronger the wander force, and the more likely the entity will change directions
	/// </summary>
    public float CircleRadius
    {
        get { return circleRadius; }
        set { circleRadius = value; }
    }
    
	/// <summary>
	/// The maximum angle in degrees that the wander force can change between two frames
	/// </summary>
    public float AngleChange
    {
        get { return angleChange; }
        set { angleChange = value; }
    }
}
