using UnityEngine;

/// <summary>
/// Contains set of static helper classes used when processing user input
/// from keyboard or controller.
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
public static class InputManager
{
    /// <summary>
    /// Returns the direction in which the left stick is pointing.
    /// </summary>
    public static Vector2 GetLeftStick()
    {
        // Get the direction the left-stick is pointing
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 leftStickDirection = new Vector2(horizontalInput,verticalInput);
        
        return leftStickDirection;
    }
}