using UnityEngine;
using System.Collections;

/// <summary>
/// The player safe zone class marks the player as 'safe'
/// when inside a safe zone trigger collider.
///
/// @author - Karl C.
/// @version - 1.0.0
///
/// </summary>
[RequireComponent(typeof(Collider))]
public class PlayerSafeZone : MonoBehaviour
{
    
    [SerializeField]
    private int maxSpeed = 5;
    [SerializeField]
    [Tooltip("If given a current, it will activate a current and block that path.")]
    private GameObject blockingCurrent;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Player player = col.GetComponent<Player>();
            if (player)
            {
                player.isSafe = true;
                if (blockingCurrent)
                {
                    player.movementBean.MaxSpeed = 5;
                }
            }
        }
    }

    IEnumerator WaitBeforeCurrent(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
    }
    
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            Player player = col.GetComponent<Player>();
            if (player)
            {
                player.isSafe = false;
                if (blockingCurrent)
                {
                    blockingCurrent.SetActive(true);
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
