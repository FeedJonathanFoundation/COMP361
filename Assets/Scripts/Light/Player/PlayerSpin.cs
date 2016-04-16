using UnityEngine;


/// <summary>
/// Responsible for all the mechanics related to player's 
/// rotations in the game universe
///
/// @author - Jonathan L.A
///
/// @version - 1.0.0
///
/// </summary>
public class PlayerSpin : MonoBehaviour
{    
    [Tooltip("The speed at which the player spins whilst idle (degrees/sec)")]
    [SerializeField]
    private float idleSpinSpeed = 50f;
    private new Transform transform;
    private new Rigidbody rigidbody;    
    private enum SpinState { Idle } //Determines the way in which the player is spinning
      
    /// <summary>
    /// <see cref="Unity Documentation">
    /// </summary>  
    void Start()
    {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody>();
    }
    
    /// <summary>
    /// <see cref="Unity Documentation">
    /// </summary>
    void FixedUpdate()
    {
        transform.RotateAround(transform.position, transform.up, idleSpinSpeed*Time.fixedDeltaTime);
    }
}