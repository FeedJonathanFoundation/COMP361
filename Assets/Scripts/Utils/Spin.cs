using UnityEngine;

/// <summary>
/// Spins a GameObject around a given axis
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
public class Spin : MonoBehaviour
{    
    [Tooltip("The speed at which the object spins (degrees/sec)")]
    [SerializeField]
    private float spinSpeed = 50f;
            
    [Tooltip("The axis around which the object spins")]
    [SerializeField]
    private RotationAxis rotationAxis;    
    private new Transform transform;
    private enum RotationAxis { X,Y,Z }
    
    /// <summary>
    /// <see cref="Unity Documentation">
    /// </summary>
    void Start()
    {
        transform = GetComponent<Transform>();
    }
    
    /// <summary>
    /// <see cref="Unity Documentation">
    /// </summary>
    void FixedUpdate()
    {
        Vector3 rotationDirection = Vector3.zero;
        
        // Choose the direction around which the object will spin
        switch (rotationAxis)
        {
            case RotationAxis.X:
                rotationDirection = transform.right;
                break;
            case RotationAxis.Y:
                rotationDirection = transform.up;
                break;
            case RotationAxis.Z:
                rotationDirection = transform.forward;
                break;
            default:
                Debug.LogError("Rotation axis not specified (Spin.cs)");
                break;
        }
        
        // Perform the rotation
        transform.RotateAround(transform.position, rotationDirection, spinSpeed*Time.fixedDeltaTime);
    }
}

