using UnityEngine;
using System.Collections;

/// <summary>
/// The player safe zone class ???
///
/// @author - Karl C.
/// @version - 1.0.0
///
/// </summary>
public class PlayerSafeZone : MonoBehaviour
{
    [SerializeField]
    [Tooltip("If given a current, it will activate a current and block that path.")]
    private GameObject blockingCurrent;

    /// <summary>
    /// ???
    /// </summary>
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player") // have to put this because LightAbsorber has a player tag
        {
            Player player = col.GetComponent<Player>();
            if (player)
            {
                player.isSafe = true;
                if (blockingCurrent)
                {
                    player.MaxSpeed(5);
                }
            }
        }
    }

    /// <summary>
    /// ???
    /// </summary>
    IEnumerator WaitBeforeCurrent(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
    }
    
    /// <summary>
    /// ???
    /// </summary>
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
