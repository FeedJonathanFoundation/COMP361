using UnityEngine;

/// <summary>
/// Large, invulnerable NPC. 
/// Seeks smaller fish, flares and player by default
///
/// @author - Karl C.
/// @version - 1.0.0
/// 
/// </summary>
public class FishBoss : AbstractFish
{
    [Tooltip("Then action performed when the fish detects the player")]
    [SerializeField]
    private BossSeekPlayer followPlayer;

    [Tooltip("The action performed when flare is within the fish's line of sight")]
    [SerializeField]
    private SeekFlare flareBehaviour;

    [Tooltip("Then action performed when the player is in a safe zone")]
    [SerializeField]
    private MoveClosestWaypoint moveToWaypoint;

    private GameObject player;
    private float animationSpeed;
    private Animator animator;
    private float speedFactor;

    protected override void Awake()
    {
        // call parent LightSource Awake() first
        base.Awake();

        this.player = GameObject.Find("Player");
        this.animationSpeed = 1f;
        this.animator = GetComponentInChildren<Animator>();
        this.speedFactor = 5f;

        SetAnimationSpeed();
        Animate(false, true);

        // Low priority
        moveToWaypoint.SetBigFish(this.transform);
        moveToWaypoint.SetPriority(0);
        moveToWaypoint.SetID("-3");

        // Medium priority
        followPlayer.SetPriority(2);
        followPlayer.SetID("-1");
        followPlayer.Init();

        // Very High priority
        flareBehaviour.SetPriority(3);
        flareBehaviour.SetID("-2");
        flareBehaviour.Init();
    }

    protected override void Update()
    {
        base.Update();
        BossReactToPlayer();
    }

    public override void Move()
    {
        Animate(false, true);
        moveToWaypoint.SetPriority(0);   // Lowest priority
        moveToWaypoint.SetID(GetID());
        AddAction(moveToWaypoint);
        SetAnimationSpeed();
    }

    public override void ReactToPlayer(Transform player)
    {
    }

    public override void ReactToNPC(Transform other)
    {
    }

    public override void NPCOutOfSight(Transform other)
    {
    }

    /// <summary>
    /// FishBoss seeks the flare
    /// </summary>
    /// <param name="flare"></param>
    public override void ReactToFlare(Transform flare)
    {
        Animate(true, true);
        flareBehaviour.TargetFlare = flare;
        AddAction(flareBehaviour);
        SetAnimationSpeed();
    }

    /// <summary>
    /// Invoke animation of swiming and bite based on the passed parameters
    /// </summary>
    /// <param name="bite"></param>
    /// <param name="swim"></param>
    private void Animate(bool bite, bool swim)
    {
        if (animator)
        {
            animator.SetBool("Bite", bite);
            animator.SetBool("Swim", swim);
        }
    }

    /// <summary>
    /// AI responsible for always following the player 
    /// when he is not in a safe zone 
    /// </summary>
    private void BossReactToPlayer()
    {
        if (!player)
        {
            player = GameObject.Find("Player");
        }

        if (!player.GetComponent<Player>().IsSafe)
        {
            Animate(true, true);
            followPlayer.TargetLightSource = player.GetComponent<LightSource>();
            AddAction(followPlayer);
            SetAnimationSpeed();
        }

    }

    /// <summary>
    /// Sets animation speed of swiming and attack actions
    /// </summary>
    private void SetAnimationSpeed()
    {
        if (animator)
        {
            float currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;
            animationSpeed = currentSpeed / speedFactor;
            if (animationSpeed > 0)
            {
                animator.SetFloat("Speed", animationSpeed);
            }
            else
            {
                animator.SetFloat("Speed", 0.2f);
            }
        }
    }
}