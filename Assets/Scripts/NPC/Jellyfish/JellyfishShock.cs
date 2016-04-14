using UnityEngine;
using System.Collections;

/// <summary>
/// The Jellyfish shock class causes the player harm
/// if the player enters the collider.
///
/// @author - Karl C.
/// @version - 1.0.0
///
/// </summary>
[RequireComponent(typeof(Collider))]
public class JellyfishShock : MonoBehaviour 
{
    [SerializeField]
    [Tooltip("Amount of energy sucked up but the jellyfish")]
    private float energyLost;
    erializeField]
    [Tooltip("Interval at which the jellyfish sucks up player energy")]
    private float timeBeforeEnergyLost;
    private float timer = 0;
    [SerializeField]
    [Tooltip("Particle effect played when the player is hit by a fish")]
    private ParticleSystem hitParticles;
    [SerializeField]
    [Tooltip("The amount of force applied on the player when hit by a jellyfish")]
    private float knockbackForce = 10;
    [SerializeField]
    [Tooltip("The amount of time the player flashes when hit")]
    private float hitFlashDuration = 2.0f;
    private ControllerRumble controllerRumble;
    private SoundManager soundManager;
    
    void Start()
    {
        GameObject soundObject = GameObject.FindWithTag("SoundManager");
        if (soundObject != null)
        {
            soundManager = soundObject.GetComponent<SoundManager>();
        }
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            Player player = collider.GetComponent<Player>();
            if(player)
            {
                Shock(player);
            }
        }
    }
    
    private void Shock(LightSource lightSource)
    {
        Knockback(lightSource);
        lightSource.LightEnergy.Deplete(energyLost);
        
        if (soundManager != null)
        {
            soundManager.PlaySound("JellyfishAttack", this.gameObject);
        }
    }
    
    private void Knockback(LightSource lightSource)
    {
        if (lightSource is Player)
        {
            Transform player = lightSource.transform;
            Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
            this.controllerRumble = player.GetComponent<ControllerRumble>();
            // Calculate a knockback force pushing the player away from the enemy fish
            Vector2 distance = (player.position - gameObject.transform.position);
            Vector2 knockback = distance.normalized * knockbackForce;

            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.AddForce(knockback, ForceMode.Impulse);

            // Instantiate hit particles
            GameObject.Instantiate(hitParticles, player.position, Quaternion.Euler(0, 0, 0));

            // Rumble the controller when the player hits a fish.
            controllerRumble.PlayerHitByFish();
        }

    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            if (soundManager != null)
            {
                soundManager.PlaySound("StopJellyfishAttack", this.gameObject);
            }
        }
    }

}
