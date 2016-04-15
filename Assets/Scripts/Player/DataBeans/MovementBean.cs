 using UnityEngine;
 
/// <summary>
/// Stores the movement related variables imported from Unity Editor in a 
/// single self-contained class. Primitives are initialized with default values, 
/// that are overwriten from Unity Editor.
///
/// @author - Alex I.
/// @version - 1.0.0
///
/// </summary>
 [System.Serializable]
 public class MovementBean
 {
    [Tooltip("The position at which the character ejects mass")]
    [SerializeField]
    private Transform massEjectionTransform;

    [Tooltip("The light ball ejected by the player when thrusting")]
    [SerializeField]
    private GameObject lightBallPrefab;

    [Tooltip("The player's max speed in m/s")]
    [SerializeField]
    private float maxSpeed = 100;

    [Tooltip("The amount of force applied on the player when ejecting one piece of mass")]
    [SerializeField]
    private float thrustForce = 0;

    [Tooltip("The higher the value, the faster the propulsion when changing directions")]
    [SerializeField]
    private float changeDirectionBoost = 0;

    [Tooltip("The amount of light energy spent when ejecting one piece of mass")]
    [SerializeField]
    private float thrustEnergyCost = 1;

    [Tooltip("The damping to apply when the brakes are on at full strength")]
    [SerializeField]
    private float brakeDrag = 1;

    [Tooltip("The speed of rotation of the player in response to user input")]
    [SerializeField]
    private float rotationSpeed = 5;

    [Tooltip("The parent of the propulsion particle effects activated when the player is propulsing")]
    [SerializeField]
    private GameObject jetFuelEffect;

    [Tooltip("Particle effect played when the player is hit by a fish")]
    [SerializeField]
    private ParticleSystem fishHitParticles;

    [Tooltip("Particle effect played when the player dies")]
    [SerializeField]
    private ParticleSystem playerDeathParticles;
        
    public Transform MassEjectionTransform
    {
        get { return massEjectionTransform; }
    }
    
    public GameObject LightBallPrefab
    {
        get { return lightBallPrefab; }
    }
    
    public float MaxSpeed
    {
        get { return maxSpeed;  }
        set { maxSpeed = value; }
    }
    
    public float ThrustForce
    {
        get { return thrustForce; }
    }
    
    public float ChangeDirectionBoost
    {
        get { return changeDirectionBoost; }
    }
    
    public float ThrustEnergyCost
    {
        get { return thrustEnergyCost; }
    }
    
    public float BrakeDrag
    {
        get { return brakeDrag; }
    }
    
    public float RotationSpeed
    {
        get { return rotationSpeed; }
    }
    
    public GameObject JetFuelEffect
    {
        get { return jetFuelEffect; }
    }
    
    public ParticleSystem FishHitParticles
    {
        get { return fishHitParticles; }
    }
    
    public ParticleSystem PlayerDeathParticles
    {
        get { return playerDeathParticles; }
    }
    
 }
 