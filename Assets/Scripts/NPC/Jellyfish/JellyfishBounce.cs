using UnityEngine;
using System.Collections;

/// <summary>
/// The Jellyfish Bounce class causes jellyfish bouncing movement
/// on player collision.
/// To make the jellyfish are not affected by the collision,
/// make sure 'is kinematic' is checked in rigidbody
///
/// @author - Karl C.
/// @version - 1.0.0
///
/// </summary>
public class JellyfishBounce : MonoBehaviour 
{

    [SerializeField]
    [Tooltip("Intensity that the player bounces on the jellyfish.")]
    private float bounceIntensity;
    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Bounce(collision);
        }
    }
    
    private void Bounce(Collision collision)
    {
        Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.Reflect(collision.relativeVelocity*-bounceIntensity, collision.contacts[0].normal);
    }
}
