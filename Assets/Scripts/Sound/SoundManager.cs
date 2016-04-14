using UnityEngine;
using System.Collections;

/// <summary>
/// Manages miscellaneous sound playing.
///
/// @author - Stella L.
/// @version - 1.0.0
///
/// </summary>
public class SoundManager : MonoBehaviour
{

    public void PlaySound(string name, GameObject target)
    {
        switch (name)
        {
            case "Current":
                AkSoundEngine.PostEvent("Current", target);
                break;
            case "Pulse":
                AkSoundEngine.PostEvent("Pulse", target);
                break;
            case "CriticalHealth":
                AkSoundEngine.PostEvent("CriticalHealth", target);
                break;
            case "Detection":
                AkSoundEngine.PostEvent("Fish_Detection", target);
                break;
            case "BossEat":
                AkSoundEngine.PostEvent("BossEat", target);
                break;
            case "JellyfishAttack":
                 AkSoundEngine.PostEvent("JellyfishAttack", target);
                 break;
            case "StopJellyfishAttack":
                AkSoundEngine.PostEvent("StopJellyfishAttack", target);
                break;
            default:
                break;
        }
    }

}
