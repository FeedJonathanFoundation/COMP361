using UnityEngine;

/// <summary>
/// Defines behaviour of boss fish when seeking the player
/// 
/// @author - Karl C.
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class BossSeekPlayer : NPCActionable
{

    [Tooltip("Set maxSpeed to this value when the player is too far")]
    [SerializeField]
    private float speedMaxIncreased;
    
    [Tooltip("Set minSpeed to this value when the player is too far")]
    [SerializeField]
    private float speedMinIncreased;
    
    [Tooltip("Distance at which the boss will increase its speed to presue the player")]
    [SerializeField]
    private float distanceSpeedIncrease;
    
    [Tooltip("The seek action performed when the NPC has more light than the target light source.")]
    [SerializeField]
    private Seek seekPlayer;
        
       
    private WallAvoidance wallAvoidance;
    private LightSource targetLightSource;
    private bool bossAtSafeZone;
     
     
    public BossSeekPlayer(int priority, string id, LightSource targetLightSource, bool bossSafe) : base(priority, id)
    {
        this.targetLightSource = targetLightSource;
        this.bossAtSafeZone = bossSafe;
        this.SetPriority(priority);
        this.SetID(id);
    }

    /// <summary>
    /// Calls ChildActionComplete() when either the seek or flee actions are completed.
    /// Invoked in the Start() function of the fish performing this action.
    /// </summary>
    public void Init()
    {
        seekPlayer.ActionComplete += ChildActionComplete;
        wallAvoidance.ActionComplete += ChildActionComplete;
    }

    public void SetPriority(int priority)
    {
        this.priority = priority;
        seekPlayer.Priority = priority;
        wallAvoidance.Priority = priority;
    }

    public void SetID(string id)
    {
        this.id = id;
        seekPlayer.Id = id;
        wallAvoidance.Id = id;
    }

    /// <summary>
    /// Called every frame when the action needs to be performed.
    /// Applies a seeking force on the given steerable
    /// </summary>
    public override void Execute(Steerable steerable)
    {
        Player player = targetLightSource.gameObject.GetComponent<Player>();
        if (!player.IsSafe)
        {
            base.Execute(steerable);

            if (overrideSteerableSpeed)
            {
                steerable.MinSpeed = minSpeed;
                steerable.MaxSpeed = maxSpeed;
            }
            // Override the steerable's max force
            if (overrideMaxForce)
            {
                steerable.MaxForce = maxForce;
            }

            Vector3 position = targetLightSource.transform.position;
            if (overrideBossSpeed(steerable, position))
            {
                steerable.MinSpeed = speedMinIncreased;
                steerable.MaxSpeed = speedMaxIncreased;
            }
            steerable.AddSeekForce(position, strengthMultiplier);
        }
        else
        {
            ActionCompleted();
        }

        wallAvoidance.Execute(steerable);
    }

    private bool overrideBossSpeed(Steerable steerable, Vector3 target)
    {
        float distance = Vector2.Distance(steerable.transform.position, target);
        if (distance >= distanceSpeedIncrease)
        {
            return true;
        }
        return false;
    }

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
                seekPlayer.TargetTransform = value.transform;
            }
            else
            {
                seekPlayer.TargetTransform = null;
            }
        }
    }
}
