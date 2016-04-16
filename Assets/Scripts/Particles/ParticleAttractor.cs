using UnityEngine;
using System.Collections;

public class ParticleAttractor : MonoBehaviour
{
    [Tooltip("The higher the value, the faster the particles will move towards their target.")]
    [SerializeField]
    private float attractionSpeed;
    
    [Tooltip("If true, the particles will be attracted to the mouth of the probe")]
    [SerializeField]
    private bool attractToPlayer;
    
    /** The system whose particles are attracted to a point. */
    private new ParticleSystem particleSystem;
    /** The particles which are attracted to a certain position */
    private ParticleSystem.Particle[] particles;
    
    /** The point to which the particles are attracted if 'attractToPlayer == true' 
      * DO NOT access directly. Use "PlayerMouth" property instead.
      */
    private static Transform playerMouth;
    
    /// <summary>
    /// Called every frame
    /// </summary>
    void Update()
    {        
        Transform attractionPoint = AttractionPoint;
        
        // Attract the particles to the mouth of the player
        if (attractToPlayer)
        {
            attractionPoint = PlayerMouth;
        }
        
        InitializeIfNeeded();
        
        int numParticles = particleSystem.GetParticles(particles);
        
        for (int i = 0; i < numParticles; i++)
        {
            // Attract the particles to the attraction point
            particles[i].velocity = Vector3.Lerp(particles[i].velocity, (attractionPoint.position - particles[i].position), 
                                                 attractionSpeed * Time.deltaTime);    
        }
        
        // Apply changes to the particle system
        particleSystem.SetParticles(particles, numParticles);
        
    }
    
    /// <summary>
    /// Create the 'particles' array for all the particles in the GameObject's ParticleSystem,
    /// if it hasn't been instantiated yet.
    /// </summary> 
    private void InitializeIfNeeded()
    {
        if (particleSystem == null) 
        { 
            particleSystem = GetComponent<ParticleSystem>(); 
        }
            
        if (particles == null || particles.Length < particleSystem.maxParticles)
        {
            particles = new ParticleSystem.Particle[particleSystem.maxParticles];
        }
    }
    
    /// <summary>
    /// The point to which the particles move towards 
    /// </summary>
    private Transform AttractionPoint
    {
        get; set;
    }
    
    /// <summary>
    /// The point to which the particles are attracted if 'attractToPlayer == true'
    /// </summary>
    private static Transform PlayerMouth
    {
        get 
        {
            if (playerMouth == null)
            {
                // Find the player's mouth
                playerMouth = GameObject.Find("Player").transform.FindChild("MassShooter");
            }   
            
            return playerMouth;
        }
    }
    
}