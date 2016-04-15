using UnityEngine;

/// <summary>
/// Responsible for mechanics related to flare ejection by player
///
/// @author - Karl C.
/// @author - Alex I.
/// @version - 1.0.0
///
/// </summary>
public class PlayerSpawnFlare
{
    private float cooldownTime;
    private float flareEnergyCost;
    private float flareCostPercentage;
    private float recoilForce;   
    private float timer;
    private Player player;
    private Transform flareSpawnObject;
    private Rigidbody rigidbody;
    private SmoothCamera smoothCamera;
    private FlareSound flareSound;
    private PlayerSound playerSound;
    private ControllerRumble controllerRumble;
    
    public PlayerSpawnFlare(FlareBean flareBean, Player player, ControllerRumble controllerRumble) 
    {               
        this.timer = 0;
        this.cooldownTime = flareBean.CoolDown;               
        this.flareSpawnObject = flareBean.FlareSpawnObject;
        this.recoilForce = flareBean.RecoilForce;
        this.flareCostPercentage = flareBean.FlareCostPercentage;
        this.flareEnergyCost = flareBean.FlareEnergyCost;               
        
        this.player = player;
        this.controllerRumble = controllerRumble;
        this.rigidbody = player.GetComponent<Rigidbody>();

        GameObject mainCamera = GameObject.Find("Main Camera");
        if (mainCamera != null) { this.smoothCamera = mainCamera.GetComponent<SmoothCamera>(); }
    }

    /// <summary>
    /// Spawns the flare if player has enough energy and flare spawner is ready
    /// </summary>
    /// <returns></returns>
    public bool SpawnFlare()
    {
        bool ready = true;
        if (timer < cooldownTime)
        {
            timer += Time.deltaTime;
            ready = false;
        }

        if (ready)
        {
            float cost = flareEnergyCost * flareCostPercentage;
            if ((player.LightEnergy.CurrentEnergy > (flareEnergyCost + cost)))
            {
                ShootFlare();
                return true;
            }
        }

        playerSound.InsufficientEnergySound();
        return false;

    }
    
    /// <summary>
    /// Shoots flare, deducts player's energy accordingly and
    /// zooms out camera to see more environment around player.
    /// </summary>
    private void ShootFlare()
    {
        player.LightEnergy.Deplete(flareEnergyCost);
        // Apply recoil in the opposite direction the flare was shot
        rigidbody.AddForce(-flareSpawnObject.right * recoilForce, ForceMode.Impulse);
        controllerRumble.ShotFlare();   // Rumble the controller
        timer = 0.0f;        
        flareSound.ShootFlareSound();

        //reset all values for the zoom whenever player fires a flare
        if (smoothCamera != null)
        {
            smoothCamera.MaxZoomOut();
            smoothCamera.ResetTimer();
        }
    }

    /// <summary>
    /// Invoked when BossFish eats the flare
    /// Trigger the sound effect.
    /// </summary>
    public void EatFlare()
    {
        flareSound.EatFlareSound(player.gameObject);
    }

}