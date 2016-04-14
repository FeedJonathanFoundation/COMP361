using UnityEngine;
using System.Collections;

/// <summary>
/// The Jellyfish Movement class causes the jellyfish to move
/// up and down.
///
/// @author - Karl C.
/// @version - 1.0.0
///
/// </summary>
public class JellyfishMovement : MonoBehaviour 
{
    [SerializeField]
    [Tooltip("Distance jellyfish travels from its initial point")]
    private float jellyfishTravelDistance;
    [SerializeField]
    [Tooltip("Speed of the jellyfish movement")]
    private float jellyfishSpeed;
    private Vector3 jellyfishStart;
    private bool goingUp; //decide if he goes up or down
    private Vector3 movement;
    private Rigidbody parentRigidbody;
    
    void Start() 
    {
        jellyfishStart = this.transform.position;
        goingUp = (Random.value >= 0.5? true : false); //randomly chooses true or false
        parentRigidbody = this.transform.root.GetComponent<Rigidbody>();
        SetVelocity();
    }

    void Update() 
    {
        float distance = Vector3.Distance(jellyfishStart,this.transform.position);
        if(distance >= jellyfishTravelDistance)
        {
            goingUp = !goingUp;
            SetVelocity();
        }
    }
    
    /// <summary>
    /// Sets the velocity based on upwards or downwards movement
    /// </summary>
    private void SetVelocity()
    {
        if(goingUp)
        {
            movement = Vector3.up * jellyfishSpeed;
        }
        else
        {
            movement = Vector3.down * jellyfishSpeed;
        }
        parentRigidbody.velocity = Vector3.zero;
        parentRigidbody.AddForce(movement * jellyfishSpeed,ForceMode.Force);
    }
}
