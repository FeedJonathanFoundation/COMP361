using UnityEngine;

/// <summary>
/// The player safe zone class marks the player as 'safe' when inside 
/// a safe zone trigger collider. The boss fish can't reach player in this zone.
///
/// @author - Karl C.
/// @version - 1.0.0
///
/// </summary>
[RequireComponent(typeof(Collider))]
public class PlayerSafeZone : MonoBehaviour
{
    
    [Tooltip("Max player speed in the safe zone")]
    [SerializeField]
    private int maxSpeed = 5;
    
    [SerializeField]
    [Tooltip("If given a current, it will activate a current and block that path.")]
    private GameObject blockingCurrent;

    /// <summary>
    /// Executed when player enters the safe zone
    /// </summary>
    /// <param name="playerCollider"></param>
    void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            Player player = playerCollider.GetComponent<Player>();
            if (player)
            {
                player.IsSafe = true;
                if (blockingCurrent) { player.MovementBean.MaxSpeed = 5; }
            }
        }
    }
    
    /// <summary>
    /// Executed when player exits the safe zone
    /// </summary>
    /// <param name="playerCollider"></param>
    void OnTriggerExit(Collider playerCollider)
    {
        if (playerCollider.tag == "Player")
        {
            Player player = playerCollider.GetComponent<Player>();
            if (player)
            {
                player.IsSafe = false;
                if (blockingCurrent)
                {
                    blockingCurrent.SetActive(true);
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
