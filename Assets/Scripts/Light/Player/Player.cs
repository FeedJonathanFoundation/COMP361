using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Player class is responsible for behaviour related to player's object
///
/// @author - Jonathan L.A
/// @author - Alex I.
/// @version - 1.0.0
///
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(Transform))]
public class Player : LightSource
{
    [SerializeField]
    [Tooltip("Flare game object")]
    private GameObject flareObject;
    private ProbeBean probeBean;
    private FlareBean flareBean;
    private MovementBean movementBean;
    private LightToggleBean lightToggleBean;

    private PlayerMovement movement;
    private PlayerLightToggle lightToggle;
    private PlayerSpawnFlare flareControl;
    private ControllerRumble controllerRumble;
    private PlayerSound playerSound;
    
    private bool isDead;
    public bool isSafe; // used for boss AI
    private bool showDeathParticles;


    private static Player playerInstance;
    private NetworkStartPosition[] spawnPoints;


    /// <summary>
    /// Initialize Player components
    /// <see cref="Unity Documentation">
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        this.controllerRumble = GetComponent<ControllerRumble>();
        this.playerSound = GetComponent<PlayerSound>();
        this.movement = new PlayerMovement(movementBean, this.Transform, this.Rigidbody, this.LightEnergy);
        this.lightToggle = new PlayerLightToggle(this.Transform.Find("LightsToToggle").gameObject, this, lightToggleBean);
        this.flareControl = new PlayerSpawnFlare(flareBean, this, controllerRumble);
        this.movementBean.DefaultDrag = Rigidbody.drag;
        this.isDead = false;
        this.isSafe = true;

    }

    /// <summary>
    /// Listen for player actions
    /// <see cref="Unity Documentation">
    /// </summary>
    protected override void Update()
    {
        if (!isLocalPlayer) { return; }
        base.Update();
        if (isDead)
        {
            RestartGameListener();
        }
        else
        {
            MoveControlListener();
            LightControlListener();
            FlareControlListener();
        }
    }

    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer)
        {
            if (playerInstance != null && playerInstance != this)
            {
                GameObject.Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
                playerInstance = this;
            }

            probeBean.ProbeColorOn = probeBean.LocalProbeColorOn;
            probeBean.ProbeColorOff = probeBean.LocalProbeColorOff;
            ChangeColor(probeBean.ProbeColorOn);
            this.LightEnergy.Add(this.DefaultEnergy);

        }
    }
            
    /// <summary>
    /// Changes the color of the player avatar to the given one
    /// </summary>
    /// <param name="color">target color</param>
    /// <param name="isSmooth">if true, the color change will follow a smooth gradient</param>
    protected override void ChangeColor(Color color)
    {
        if (!isLocalPlayer) { return; }

        foreach (GameObject probe in GameObject.FindGameObjectsWithTag("Probe"))
        {
            string probeName = probe.transform.root.gameObject.name;
            if (probeName != this.name) { return; }

            Renderer renderer = probe.GetComponent<Renderer>();
            foreach (Material material in renderer.materials)
            {
                material.SetColor("_EmissionColor", color);
            }
        }
    }

    /// <summary>
    /// <see cref="Unity Documentation">
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        ConsumedLightSource += OnConsumedLightSource;
    }

    /// <summary>
    /// <see cref="Unity Documentation"> 
    /// </summary>
    protected override void OnDisable()
    {
        base.OnEnable();
        ConsumedLightSource -= OnConsumedLightSource;
    }

    /// <summary>
    /// Sets player state to 'dead' when LightDepleted event is triggered
    /// </summary>
    protected override void OnLightDepleted()
    {
        if (!isLocalPlayer) { return; }
        base.OnLightDepleted();

        // If the player just died
        if (!isDead)
        {
            movement.OnPropulsionEnd();
            Rigidbody.useGravity = true;
        }

        isDead = true;
        playerSound.PlayerDeathSound();
        Debug.Log("Game OVER! Press 'R' to restart!");
    }

    /// <summary>
    /// Invoked when the player is hit by a light source that is stronger than him
    /// </summary>
    protected override void OnKnockback(LightSource enemyLightSource)
    {
        // Calculate a knockback force pushing the player away from the enemy fish
        Vector2 distance = (Transform.position - enemyLightSource.Transform.position);
        Vector2 knockback = distance.normalized * movementBean.KnockbackForce;

        Rigidbody.velocity = Vector3.zero;
        Rigidbody.AddForce(knockback, ForceMode.Impulse);

        // If the player was hit by a fish
        if (enemyLightSource.CompareTag("Fish"))
        {
            GameObject.Instantiate(movementBean.FishHitParticles, transform.position, Quaternion.Euler(0, 0, 0));
            controllerRumble.PlayerHitByFish();
        }

        movementBean.LastTimeHit = Time.time;
    }
    
    /// <summary>
    /// Call to network method to respawn network player  
    /// </summary>
    protected void OnRespawn()
    {
        if (!isServer) { return; }
        RpcRespawn();
    }
           
    /// <summary>
    /// Network call, executed when player respawns after death
    /// </summary>
    [ClientRpc]
    protected void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            Vector3 spawnPoint = Vector3.zero;
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                // maybe have it iterate instead of being random
            }
            transform.position = spawnPoint;
        }
    }

    /// <summary>
    /// Create a flare instance and send in over network to peers
    /// </summary>
    [Command]
    private void Cmd_ShootFlare()
    {
        GameObject flare = (GameObject) Instantiate(flareObject, flareBean.FlareSpawnObject.position, flareBean.FlareSpawnObject.rotation);
        NetworkServer.Spawn(flare);
    }

    /// <summary>
    /// Listens for input responsible for spawning a player
    /// </summary>
    private void FlareControlListener()
    {
        if (Input.GetButtonDown("UseFlare"))
        {
            bool spawnSuccess = flareControl.SpawnFlare();
            if (spawnSuccess) { Cmd_ShootFlare(); }
        }
    }
    
     /// <summary>
    /// Listens for Lights button click
    /// When Lights button is clicked toggle the lights ON or OFF
    /// </summary>
    private void LightControlListener()
    {
        if (Input.GetButtonDown("LightToggle") && this.lightToggle != null)
        {
            if (lightToggleBean.MinimalEnergy < this.LightEnergy.CurrentEnergy)
            {
                this.lightToggle.ToggleLights();
                playerSound.LightToggleSound();
                if (this.lightToggle.LightsEnabled)
                {
                    this.ChangeColor(probeBean.ProbeColorOn);
                }
                else
                {
                    this.ChangeColor(probeBean.ProbeColorOff);
                }
            }
            else
            {
                // If the player isn't thrusting, turn off his emissive lights
                if (!movement.Thrusting)
                {
                    this.ChangeColor(probeBean.ProbeColorOff);
                }
                playerSound.InsufficientEnergySound();
            }
        }

        // Deplete player's light while light is on
        this.lightToggle.DepleteLight(lightToggleBean.TimeToDeplete, lightToggleBean.LightToggleEnergyCost);        
    }

    
    /// <summary>
    /// Listens for input related to movement of the player
    /// </summary>
    private void MoveControlListener()
    {
        if (isDead)
        {
            // Slow down gravity;
            Rigidbody.AddForce(Vector3.up * 20, ForceMode.Force);
            return;
        }

        SetPlayerDrag();
        SetPlayerVelocity();

        float thrustAxis = Input.GetAxis("ThrustAxis");
        float brakeAxis = Input.GetAxis("BrakeAxis");

        if (Input.GetButtonDown("Thrust") || (movementBean.PreviousThrustAxis == 0 && thrustAxis > 0))
        {
            movement.OnPropulsionStart();
            lightToggle.OnPropulsionStart();
        }
        
        if (Input.GetButtonUp("Thrust") || (movementBean.PreviousThrustAxis > 0 && thrustAxis == 0))
        {
            movement.OnPropulsionEnd();
            lightToggle.OnPropulsionEnd();
        }

        if (Input.GetButton("Thrust"))
        {
            movement.Propulse(-movementBean.MassEjectionTransform.up);
        }
        
        // Brake
        if (Input.GetButton("Brake"))
        {
            movement.Brake(1);
        }

        // Propulse in the direction of the left stick (opposite to the rear of the probe)
        if (thrustAxis != 0)
        {            
            movement.Propulse(-movementBean.MassEjectionTransform.up, thrustAxis);
        }
        
        if (brakeAxis != 0)
        {
            movement.Brake(brakeAxis);
        }

        // Makes the character follow the left stick's rotation
        movement.FollowLeftStickRotation();
        movementBean.PreviousThrustAxis = thrustAxis;
    }

    /// <summary>
    /// Listens for restart button clicks
    /// </summary>
    private void RestartGameListener()
    {
        this.isSafe = true;
        CanAbsorbState = false; //remove canAbsorb

        if (Input.GetButtonDown("Restart"))
        {
            Debug.Log("Game Restarted");
            Transform.localScale = new Vector3(1, 1, 1);
            Rigidbody.isKinematic = false;
            Rigidbody.useGravity = false;
            this.LightEnergy.Add(this.DefaultEnergy);
            this.isDead = false;
            this.showDeathParticles = false;
            this.Rigidbody.drag = movementBean.DefaultDrag; // reset drag
            this.transform.FindChild("ProbeModel").gameObject.SetActive(true); //reactivate bubbles
            CanAbsorbState = true; //reset canAbsorb
            OnRespawn();
        }
    }

    /// <summary>
    /// Modify player's drag if he is invulnerable
    /// 0 = just became invulnerable
    /// 1 = not invulnerable anymore
    /// </summary>
    private void SetPlayerDrag()
    {
        this.Rigidbody.drag = movementBean.DefaultDrag;
        if (IsInvulnerable)
        {
            float invulnerabilityPercent = (Time.time - movementBean.LastTimeHit) / movementBean.InvulnerabilityTime;
            this.Rigidbody.drag = (movementBean.InvulnerabilityDrag - movementBean.DefaultDrag) * (1 - invulnerabilityPercent) + movementBean.DefaultDrag;
        }
    }
    
    /// <summary>
    /// Clamp player's velocity and ensure that the rigidbody never spins
    /// </summary>
    private void SetPlayerVelocity()
    {        
        // Clamp the player's velocity
        if (this.Rigidbody.velocity.sqrMagnitude > movementBean.MaxSpeed * movementBean.MaxSpeed)
        {
            this.Rigidbody.velocity = ((Vector2)this.Rigidbody.velocity).SetMagnitude(movementBean.MaxSpeed);
        }
        // Ensure that the rigidbody never spins
        this.Rigidbody.angularVelocity = Vector3.zero;
    }
       
    protected override bool IsAbsorbable
    {
        //Determines if player can be absorbed by other Light Sources
        get { return IsInvulnerable ? false : true; }
    }
        
    public bool IsDetectable
    {
        get
        {
            //If player's lights are on, player is visible            
            if (lightToggle != null)
            {
                return lightToggle.LightsEnabled;
            }
            return false;
        }
    }
   
    public bool IsInvulnerable
    {
        // If true, the player has been hit and is temporarily invulnerable 
        get { return (Time.time - movementBean.LastTimeHit) < movementBean.InvulnerabilityTime; }
    }

    public MovementBean MovementBean
    {
        get { return movementBean; }
    }

    public PlayerMovement Movement
    {
        get { return movement; }
        set { movement = value; }
    }
    
}