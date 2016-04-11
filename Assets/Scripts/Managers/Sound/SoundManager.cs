using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{

    public void PlaySound(string name, GameObject target)
    {
        switch (name)
        {
            case "Current":
                // AkSoundEngine.PostEvent("Current", target);
                break;
            case "Pulse":
                // AkSoundEngine.PostEvent("Pulse", target);
                break;
            case "CriticalHealth":
                // AkSoundEngine.PostEvent("CriticalHealth", this.gameObject);
            case "Detection":
                // AkSoundEngine.PostEvent("Fish_Detection", targetTransform.gameObject);
            case "BossEat":
                // AkSoundEngine.PostEvent("BossEat", target);
            case "JellyfishAttack":
                 // AkSoundEngine.PostEvent("JellyfishAttack", this.gameObject);
                 break;
            case "StopJellyfishAttack":
                // AkSoundEngine.PostEvent("StopJellyfishAttack", this.gameObject);
                break;
            default:
                break;
        }
    }
    
	/* GOES IN ATTRIBUTES */
        // private SoundManager soundManager;
    /* GOES IN VOID START() */
        // GameObject soundObject = GameObject.FindWithTag("SoundManager");
    //     if (soundObject != null)
    //     {
    //         soundManager = soundObject.GetComponent<SoundManager>();
    //     }
    /* GOES IN VOID PLAYSOUND() */
        // if (soundManager != null)
    //     {
    //         soundManager.PlaySound("", this.gameObject);
    //     }
    
}
