using UnityEngine;
using System.Collections;
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
    [Tooltip("The amount of force applied on the player when hit by an enemy")]
    private float knockbackForce = 10;
        
    [SerializeField]
    [Tooltip("Amount of time invulnerable after being hit by an enemy")]
    private float invulnerabilityTime = 3;

    [SerializeField]
    [Tooltip("Linear drag applied when player is hit by enemy")]
    private float invulnerabilityDrag = 2;
    
    [SerializeField]
    private Color probeColorOn;
    
    [SerializeField]
    private Color probeColorOff;
    
    [SerializeField]
    private Color localProbeColorOn;
    
    [SerializeField]
    private Color localProbeColorOff;

    public MovementBean movementBean;
    public FlareBean flareBean;
    public LightToggleBean lightToggleBean;


    private PlayerMovement movement;
    private PlayerLightToggle lightToggle;
    private PlayerSpawnFlare flareControl;


    private float lastTimeHit = -100;  // The last time player was hit by an enemy
    private float defaultDrag;  // Default rigidbody drag
    private float previousThrustAxis; // Previous value of Input.GetAxis("ThrustAxis")
    private bool isDead; // determines is current player is dead
    public bool isSafe; // used for boss AI
    private bool deathParticlesPlayed;   
    private ControllerRumble controllerRumble;  // Caches the controller rumble component   


    private IEnumerator changeIntensityCoroutine;

    private static Player playerInstance;
    private PlayerSound playerSound;


    [SerializeField]
    [Tooltip("Refers to flare game object")]
    private GameObject flareObject;


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
        this.defaultDrag = Rigidbody.drag;
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
    
    /// <summary>
    /// <see cref="Unity Documentation">
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        ConsumedLightSource += OnConsumedLightSource;
    }

    /// <summary>
    ///<see cref="Unity Documentation"> 
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
        Vector2 knockback = distance.normalized * knockbackForce;

        Rigidbody.velocity = Vector3.zero;
        Rigidbody.AddForce(knockback, ForceMode.Impulse);

        // If the player was hit by a fish
        if (enemyLightSource.CompareTag("Fish"))
        {
            // Instantiate hit particles
            GameObject.Instantiate(movementBean.FishHitParticles, transform.position, Quaternion.Euler(0, 0, 0));
            // Rumble the controller when the player hits a fish.
            controllerRumble.PlayerHitByFish();
        }

        // The player was just hit
        lastTimeHit = Time.time;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        // Player has collided upon death
        if (isDead && !deathParticlesPlayed && movementBean.PlayerDeathParticles != null)
        {
            // Calculate the angle of the player's velocity upon impact
            float crashAngle = Mathf.Rad2Deg * Mathf.Atan2(Rigidbody.velocity.y, Rigidbody.velocity.x);
            // Orient the explosion opposite to the player's velocity
            float explosionAngle = crashAngle + 180;
            // Spawn the explosion
            ParticleSystem explosion = GameObject.Instantiate(movementBean.PlayerDeathParticles,
                                        Transform.position, Quaternion.Euler(-90, explosionAngle, 0)) as ParticleSystem;
            // Explosion sound
            playerSound.ExplosionSound();
            // Rumble the controller
            controllerRumble.PlayerDied();

            Transform.localScale = Vector3.zero;
            Rigidbody.isKinematic = true;

            // Only play the death particles the first time the player crashes on an obstacle
            deathParticlesPlayed = true;

            this.transform.FindChild("ProbeModel").gameObject.SetActive(false); //remove bubbles on death
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


        // Clamp the player's velocity
        if (this.Rigidbody.velocity.sqrMagnitude > movementBean.MaxSpeed * movementBean.MaxSpeed)
        {
            this.Rigidbody.velocity = ((Vector2)this.Rigidbody.velocity).SetMagnitude(movementBean.MaxSpeed);
        }

        // Ensure that the rigidbody never spins
        this.Rigidbody.angularVelocity = Vector3.zero;

        float thrustAxis = Input.GetAxis("ThrustAxis");
        float brakeAxis = Input.GetAxis("BrakeAxis");

        if (Input.GetButtonDown("Thrust") || (previousThrustAxis == 0 && thrustAxis > 0))
        {
            movement.OnPropulsionStart();
            lightToggle.OnPropulsionStart();
            // this.ChangeColor(probeColorOn, true, 0);
        }

        if (Input.GetButton("Thrust"))
        {
            movement.Propulse(-movementBean.MassEjectionTransform.up);
        }

        if (thrustAxis != 0)
        {
            // Propulse in the direction of the left stick (opposite to the rear of the probe)
            movement.Propulse(-movementBean.MassEjectionTransform.up, thrustAxis);
        }

        if (Input.GetButtonUp("Thrust") || (previousThrustAxis > 0 && thrustAxis == 0))
        {
            movement.OnPropulsionEnd();
            lightToggle.OnPropulsionEnd();
        }

        // Brake
        if (Input.GetButton("Brake"))
        {
            movement.Brake(1);
        }
        if (brakeAxis != 0)
        {
            movement.Brake(brakeAxis);
        }

        // Makes the character follow the left stick's rotation
        movement.FollowLeftStickRotation();

        // Ensure that the rigidbody never spins
        this.Rigidbody.angularVelocity = Vector3.zero;

        previousThrustAxis = thrustAxis;
    }

    /// <summary>
    /// Listens for Lights button click
    /// When Lights button is clicked toggle the lights ON or OFF
    /// </summary>
    private void LightControlListener()
    {
        if (this.lightToggle != null)
        {
            if (Input.GetButtonDown("LightToggle"))
            {
                if (lightToggleBean.MinimalEnergy < this.LightEnergy.CurrentEnergy)
                {
                    this.lightToggle.ToggleLights();
                    playerSound.LightToggleSound();

                    if (changeIntensityCoroutine != null) { StopCoroutine(changeIntensityCoroutine); }

                    if (this.lightToggle.LightsEnabled)
                    {
                        this.ChangeColor(probeColorOn);
                        // changeIntensityCoroutine = materials.ChangeLightIntensity(this.lightToggle, 0.3f);
                        // StartCoroutine(changeIntensityCoroutine);
                    }
                    else
                    {
                        this.ChangeColor(probeColorOff);
                        // changeIntensityCoroutine = materials.ChangeLightIntensity(this.lightToggle, 0f);
                        // StartCoroutine(changeIntensityCoroutine);
                    }
                }
                else
                {
                    // If the player isn't thrusting, turn off his emissive lights
                    if (!movement.Thrusting)
                    {
                        this.ChangeColor(probeColorOff);
                    }
                    playerSound.InsufficientEnergySound();
                }
            }

            this.lightToggle.DepleteLight(lightToggleBean.TimeToDeplete, lightToggleBean.LightToggleEnergyCost);
        }
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
    /// Listens for restart button clicks
    /// </summary>
    private void RestartGameListener()
    {
        this.isSafe = true;
        SetCanAbsorbState(false); //remove canAbsorb

        if (Input.GetButtonDown("Restart"))
        {

            Debug.Log("Game Restarted");            
            Transform.localScale = new Vector3(1, 1, 1);
            Rigidbody.isKinematic = false;
            Rigidbody.useGravity = false;

            this.LightEnergy.Add(this.DefaultEnergy);
            this.isDead = false;
            this.deathParticlesPlayed = false;
            this.Rigidbody.drag = defaultDrag; // reset drag
            this.transform.FindChild("ProbeModel").gameObject.SetActive(true); //reactivate bubbles

            SetCanAbsorbState(true); //reset canAbsorb
            OnRespawn();


            // ReactivateObjects();

            // LoadGame();
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// Modify player's drag if he is invulnerable
    /// 0 = just became invulnerable
    /// 1 = not invulnerable anymore
    /// </summary>
    private void SetPlayerDrag()
    {
        this.Rigidbody.drag = defaultDrag;
        if (IsInvulnerable)
        {
            float invulnerabilityPercent = (Time.time - lastTimeHit) / invulnerabilityTime;
            this.Rigidbody.drag = (invulnerabilityDrag - defaultDrag) * (1 - invulnerabilityPercent) + defaultDrag;
        }
    }

    // CLASS PROPERTIES

    protected override bool IsAbsorbable
    {
        get { return IsInvulnerable ? false : true; }
    }

    public PlayerMovement Movement
    {
        get { return movement; }
        set { movement = value; }
    }

    /// <summary>
    /// If player lights are on, player is visible
    /// </summary>
    public bool IsDetectable
    {
        get
        {
            if (lightToggle != null)
            {
                return lightToggle.LightsEnabled;
            }
            return false;
        }
    }

    /// <summary>
    /// If true, the player has been hit and is temporarily
    /// invulnerable
    /// </summary>
    public bool IsInvulnerable
    {
        get { return (Time.time - lastTimeHit) < invulnerabilityTime; }
    }


    // NETWORK PART - CONSIDER MOVING TO NetworkPlayer.cs

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

            probeColorOn = localProbeColorOn;
            probeColorOff = localProbeColorOff;
            ChangeColor(probeColorOn);
            this.LightEnergy.Add(this.DefaultEnergy);

        }
    }

    private NetworkStartPosition[] spawnPoints;

    public void OnRespawn()
    {
        if (!isServer) { return; }
        RpcRespawn();
    }

    [Command]
    private void Cmd_ShootFlare()
    {
        GameObject flare = (GameObject)Instantiate(flareObject, flareBean.FlareSpawnObject.position, flareBean.FlareSpawnObject.rotation);
        NetworkServer.Spawn(flare);
    }

    [ClientRpc]
    void RpcRespawn()
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


}