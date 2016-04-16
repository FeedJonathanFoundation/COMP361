using UnityEngine;
using System.Collections;

/// <summary>
/// If active, makes an NPC seek or flee a given
/// light source
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class SeekOrFleeLight : NPCActionable
{
    [Tooltip("If true, this NPC will always flee the light source.")]   
    [SerializeField]
    private bool alwaysFlee = false;
    [Tooltip("If true, this NPC will always seek the light source.")]   
    [SerializeField]
    private bool alwaysSeek = false;
    
    /// <summary>
	/// The light that this NPC can see
	/// </summary>
    private LightSource targetLightSource;

    /// <summary>
    /// The seek action performed when the NPC has more light than the target light source.
    /// </summary>
    [SerializeField]
    private Seek seekWhenStronger;
    /// <summary>
    /// The flee action performed when the NPC has less light than the target light source.
    /// </summary>
    [SerializeField]
    private Flee fleeWhenWeaker;
    /// <summary>
    /// The wall avoidance action performed when the NPC has less light than the target light source.
    /// </summary>
    [SerializeField]
    private WallAvoidance wallAvoidance;
    
    public SeekOrFleeLight(int priority, string id, LightSource targetLightSource) : base(priority, id)
    {
        this.targetLightSource = targetLightSource;
        
        SetPriority(priority);
        SetID(id);
    }
    
    /// <summary>
    /// Call this method in the Start() function of the fish performing this action.
    /// </summary>
    public void Init()
    {
        // Call ChildActionComplete() when either the seek or flee actions are completed.
        seekWhenStronger.ActionComplete += ChildActionComplete;
        fleeWhenWeaker.ActionComplete += ChildActionComplete;
        wallAvoidance.ActionComplete += ChildActionComplete;
    }
    
    /// <summary>
    /// Sets the priorities of each steering behaviour.
    /// This priority is used to index the behaviours in a priority dictionary
    /// </summary>
    public void SetPriority(int priority)
    {
        this.priority = priority;
        
        seekWhenStronger.priority = priority;
        fleeWhenWeaker.priority = priority;
        wallAvoidance.priority = priority;
    }
    
    /// <summary>
    /// Updates the ID for each internal steering behaviour.
    /// Allows the action to be referenced by a unique index
    /// </summary>
    public void SetID(string id)
    {
        this.id = id;
        
        seekWhenStronger.id = id;
        fleeWhenWeaker.id = id;
        wallAvoidance.id = id;
    }
    
    /// <summary>
    /// Makes the steerable seek or flee the light source target
    /// </summary>
	public override void Execute(Steerable steerable) 
    {
        base.Execute(steerable);
        
        // Retrieve the amount of light energy possessed by the NPC performing this action
        float myLightEnergy = steerable.GetComponent<LightSource>().LightEnergy.CurrentEnergy;
        
        if (targetLightSource)
        {
            // If this fish has less light than its target
            if(!alwaysSeek && (myLightEnergy < targetLightSource.LightEnergy.CurrentEnergy || alwaysFlee))
            {
                // Flee the light source since it is stronger than this fish
                fleeWhenWeaker.Execute(steerable);                              
            }
            // Else, if this fish has more light than its target
            else
            {
                // Seek the light source
                seekWhenStronger.Execute(steerable);
            }
        }
        else 
        {
            // If the light source has been destroyed, stop performing this action.
            ActionCompleted();
        }
        
        wallAvoidance.Execute(steerable);
    }
    
    /// <summary>
    /// If true, this action can be cancelled before it's completed
    /// </summary>
    public override bool CanBeCancelled()
    {
        return seekWhenStronger.CanBeCancelled() && fleeWhenWeaker.CanBeCancelled();
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
    public LightSource TargetLightSource
    {
        get { return targetLightSource; }
        set 
        { 
            targetLightSource = value; 
            
            if (value)
            {
                seekWhenStronger.TargetTransform = value.transform;
                fleeWhenWeaker.TargetTransform = value.transform;
            }
            else
            {
                seekWhenStronger.TargetTransform = null;
                fleeWhenWeaker.TargetTransform = null;
            }
        }
    }
    
}
