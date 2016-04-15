using UnityEngine;
using System.Collections;

/// <summary>
/// The Eat Flare class destroys the flare object when
/// it enters a trigger and plays the corresponding sound.
///
/// @author - Karl C.
/// @version - 1.0.0
///
/// </summary>
public class BossEatFlare : MonoBehaviour
{
    private GameObject player;
    private SoundManager soundManager;
    
    void Start()
    {
        player = GameObject.Find("Player");
        GameObject soundObject = GameObject.FindWithTag("SoundManager");
        if (soundObject != null)
        {
            soundManager = soundObject.GetComponent<SoundManager>();
        }
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Flare"))
        {
            DestroyFlare(collider.transform.parent.gameObject);
        }
    }
    
    private void DestroyFlare(GameObject flare)
    {
        Destroy(flare);
        if (soundManager != null)
        {
            soundManager.PlaySound("BossEat", this.gameObject);
        }
        player.GetComponent<PlayerSpawnFlare>().EatFlare();
    }
}
