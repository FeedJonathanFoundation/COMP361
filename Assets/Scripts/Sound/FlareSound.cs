using UnityEngine;

/// <summary>
/// Flare Sound plays the appropriate flare-related sounds based
/// on distance and context.
///
/// @author - Stella L.
/// @version - 1.0.0
///
/// </summary>
public class FlareSound : MonoBehaviour
{

    GameObject target;

    public FlareSound(GameObject target)
    {
        this.target = target;
    }

    public void ShootFlareSound()
    {
        AkSoundEngine.PostEvent("Flare", target);
    }
    
    /// <summary>
    /// Changes the flare sound based on distance 
    /// </summary>
    public void SetFlareDistance(float flareDistance)
    {
        AkSoundEngine.SetRTPCValue("flareDistance", flareDistance);
    }

    public void EatFlareSound(GameObject target)
    {
        AkSoundEngine.PostEvent("FlareEat", target);
    }
    
}
