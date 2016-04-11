using UnityEngine;
using System.Collections;

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
    
    void OnTriggerEnter(Collider col)
    {

        if (col.CompareTag("Flare"))
        {
            Destroy(col.transform.parent.gameObject);
            if (soundManager != null)
            {
                soundManager.PlaySound("BossEat", this.gameObject);
            }
            
            player.GetComponent<FlareSpawner>().EatFlare();
        }
    }
}
