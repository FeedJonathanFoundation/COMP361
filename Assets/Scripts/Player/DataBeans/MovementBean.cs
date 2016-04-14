 using UnityEngine;
 
 [System.Serializable]
 public class MovementBean
 {
    [Tooltip("The position at which the character ejects mass")]
    public Transform massEjectionTransform;

    [Tooltip("The light ball ejected by the player when thrusting")]
    public GameObject lightBallPrefab;

    [Tooltip("The player's max speed in m/s")]
    public float maxSpeed = 100;

    [Tooltip("The amount of force applied on the player when ejecting one piece of mass")]
    public float thrustForce = 0;

    [Tooltip("The higher the value, the faster the propulsion when changing directions")]
    public float changeDirectionBoost = 0;

    [Tooltip("The amount of light energy spent when ejecting one piece of mass")]
    public float thrustEnergyCost = 1;

    [Tooltip("The damping to apply when the brakes are on at full strength")]
    public float brakeDrag = 1;

    [Tooltip("The speed of rotation of the player in response to user input")]
    public float rotationSpeed = 5;

    [Tooltip("The parent of the propulsion particle effects activated when the player is propulsing")]
    public GameObject jetFuelEffect;

    [Tooltip("Particle effect played when the player is hit by a fish")]
    public ParticleSystem fishHitParticles;

    [Tooltip("Particle effect played when the player dies")]
    public ParticleSystem playerDeathParticles;
 }
 