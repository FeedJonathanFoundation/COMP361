using UnityEngine;

/// <summary>
/// Denotes an action that an NPC can perform
///
/// Important note: Many member fields are public
/// because they can be edited from other classes 
/// without causing any undesirable side-effects 
/// on an instance of this class
/// 
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public abstract class NPCActionable
{
    [System.NonSerialized]
    protected int priority;
    [System.NonSerialized]
    protected string id;
    
    /** The strength of the force applied by this action. 
      * This is public because it can be edited publicly without
      * without undesirable side-effects */
    public float strengthMultiplier;
    
    [Tooltip("If true, the Steerable performing this action is given a new min/maxSpeed")]
    public bool overrideSteerableSpeed = false;
    [Tooltip("If overrideSteerableSpeed == true, and this action is performed, the steerable's minSpeed is set to this value")]
    public float minSpeed;
    [Tooltip("If overrideSteerableSpeed == true, and this action is performed, the steerable's maxSpeed is set to this value")]
    public float maxSpeed;
    
    [Tooltip("If true, calling Execute() overrides the steerable's max force with the specified 'maxForce' value")]
    public bool overrideMaxForce = false;
    [Tooltip("If overrideMaxForce == true, calling Execute() overrides the steerable's max force with this value")]
    public float maxForce;
    
    /** Stores the amount of time for which this action has been performed, in seconds */
    protected float timeActive;
    
    /** Called when the action is done being performed. The AbstractFish class then knows to stop performing the action. */
    public delegate void ActionCompleteHandler(NPCActionable completedAction);
    public event ActionCompleteHandler ActionComplete = delegate {};
    
    public NPCActionable(int priority, string id)
    {
        this.priority = priority;
        this.id = id;
    }
    
    /// <summary>
    /// Inform the AbstractFish performing this action that the action is complete 
    /// </summary>
    protected virtual void ActionCompleted()
    {
        //if (ActionComplete != null)
            // Notify subscribers that the action is complete
            ActionComplete(this);
    }
    
    /// <summary>
    /// Resets the action timer to zero. This timer determines how long the action is performed for
    /// </summary>
    protected void ResetTimer()
    {
         // Reset the timer for the next time this action is performed.
        timeActive = 0;
    }
    
    /// <summary>
    /// Returns true if the action can be stopped. Should return "false" if the action's timer is ongoing.
    /// </summary>
    public virtual bool CanBeCancelled()
    {
        return true;
    }
    
	// void Execute(Steerable steerable, SteeringBehavior steeringBehavior);
    
    /// <summary>
    /// Called every frame when this action needs to be performed.
    /// Applies the action on the given steerable
    /// </summary>
    public virtual void Execute(Steerable steerable)
    {
        timeActive += Time.deltaTime;
    }
    
    /// <summary>
    /// Returns a string representation of this action
    /// </summary>
    public new string ToString()
    {
        return base.ToString() + ", Priority = " + priority + ", ID = " + id;
    }
    
    public int Priority
    {
        get { return priority; }
        set { priority = value; }
    }
    
    public string Id 
    {
        get { return id; }
        set { id = value;}
    }
    
}