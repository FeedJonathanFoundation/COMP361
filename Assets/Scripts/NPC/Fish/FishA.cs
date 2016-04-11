using UnityEngine;
using System.Collections;

/// <summary>
/// FishA class is responsible for behaviour related to fish A
/// Small, easy to kill NPCs
///
/// @author - Stella L.
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
public class FishA : AbstractFish
{
    [SerializeField]
    private Flocking flockingBehaviour;
    
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
                                      
        playerBehaviour.SetPriority(1);     // Medium priority
        playerBehaviour.SetID("-1");
        playerBehaviour.Init();
        
        flareBehaviour.SetPriority(2);      // Highest priority
        flareBehaviour.SetID("-2");
        flareBehaviour.Init();
    }
    
    /// <summary>
    /// Set's the fish's lowest priority to the default flocking behaviour
    /// </summary>
    public override void Move() 
    {
        flockingBehaviour.SetPriority(0);   // Lowest priority
        flockingBehaviour.SetID(this.LightSourceID);
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
    
    public override void ReactToNPC(Transform other) { }
    
    public override void NPCOutOfSight(Transform other) { }
    
    /// <summary>
    /// Seeks the flare when it is nearby
    /// </summary>
    public override void ReactToFlare(Transform flare)
    {
        flareBehaviour.TargetFlare = flare;
        AddAction(flareBehaviour);
    }
    
}