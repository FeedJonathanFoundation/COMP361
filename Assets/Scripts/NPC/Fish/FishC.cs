using UnityEngine;
using System.Collections;

// Large, impossible to kill NPCs
// Seeks smaller fish by default
public class FishC : AbstractFish
{
    [SerializeField]
    private Flocking flockingBehaviour;
    
    [Tooltip("When another fish is in sight, this fish will either seek or flee it")]
    [SerializeField]
    private SeekOrFleeLight otherFishBehaviour;
    
    [Tooltip("Then action performed when the fish detects the player")]
    [SerializeField]
    private SeekOrFleeLight playerBehaviour;

    [Tooltip("The action performed when flare is within the fish's line of sight")]    
    [SerializeField]
    private SeekFlare flareBehaviour;
    
    /// <summary>
    /// Initializes the fish object
    /// </summary>
    protected override void Awake()
    {
        base.Awake(); 
        
        otherFishBehaviour.SetPriority(1);  // Medium priority
        otherFishBehaviour.Init();
        
        playerBehaviour.SetPriority(2);     // High priority
        playerBehaviour.SetID("-1");
        playerBehaviour.Init();
        
        flareBehaviour.SetPriority(3);      // Very high priority
        flareBehaviour.SetID("-2");
        flareBehaviour.Init();
    }
    
    /// <summary>
    /// Set's the fish's lowest priority to the default flocking behaviour
    /// </summary>
    public override void Move() 
    {
        flockingBehaviour.SetPriority(0);   // Lowest priority
        flockingBehaviour.SetID(GetID());
        AddAction(flockingBehaviour);
    }
    
    /// <summary>
    /// Called every frame when the fish can see the player
    /// </summary>
    public override void ReactToPlayer(Transform player)
    {        
        playerBehaviour.TargetLightSource = player.GetComponent<LightSource>();
        AddAction(playerBehaviour);
    }
    
    public override void ReactToNPC(Transform other)
    {                
        LightSource currentFishTarget = otherFishBehaviour.TargetLightSource;
        
        if (currentFishTarget == null)
        {
            AbstractFish fish = other.gameObject.GetComponent<AbstractFish>();
            string id = fish.GetID();
            
            otherFishBehaviour.TargetLightSource = fish;
            otherFishBehaviour.SetID(id);
            AddAction(otherFishBehaviour);
        }
    }
    
    public override void NPCOutOfSight(Transform other)
    {
        if (otherFishBehaviour.TargetLightSource != null 
            && otherFishBehaviour.TargetLightSource.transform == other)
        {
            otherFishBehaviour.TargetLightSource = null;
        }
    }
    
    /// <summary>
    /// Seeks the flare when it is nearby
    /// </summary>
    public override void ReactToFlare(Transform flare)
    {
        flareBehaviour.TargetFlare = flare;
        AddAction(flareBehaviour);
    }
    
}