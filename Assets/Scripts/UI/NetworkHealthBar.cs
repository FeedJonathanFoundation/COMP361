using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// The Network Health Bar class maintains each individual player's health bar status,
/// which updates based on the amount of light energy.
///
/// @author - Stella L.
/// @version - 1.0.0
///
/// </summary>
public class NetworkHealthBar : NetworkBehaviour
{

    [SerializeField]
    float healthBarWidth = 200f;
    [SerializeField]
    RectTransform healthBar;
    private float multiplier;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    [SyncVar(hook = "OnLightChanged")]
    private float currentHealth;
    [SerializeField]
    private Player player;
    
    /// <summary>
    /// Initializes the health bar
    /// sets the default values and assigns
    /// it to a player.
    /// </summary>
    void Start()
    {
        if (!isLocalPlayer) { return; }
        InitializePlayer(); 
        InitializeHealthBar();
        player.LightEnergy.LightChanged += OnLightChanged;
    }
    
    /// <summary>
    /// Unsubscribe from the OnLightChanged event.
    /// </summary>
    void OnDisable()
    {
        if (!isLocalPlayer) { return; }
        player.LightEnergy.LightChanged -= OnLightChanged;
    }
    
    /// <summary>
    /// Finds the player component of the current game object.
    /// </summary>
    void InitializePlayer()
    {
        player = GetComponent<Player>();
    }
    
    /// <summary>
    /// Initializes the health bar by setting
    /// the max health value and current value based on the 
    /// player's max energy level.
    /// </summary>
    void InitializeHealthBar()
    {
        if (!isLocalPlayer) { return; }
        maxHealth = player.DefaultEnergy;
        currentHealth = maxHealth;
        
        multiplier = healthBarWidth / maxHealth;
        
        healthBar.sizeDelta = new Vector2(currentHealth * multiplier, healthBar.sizeDelta.y);
    }
    
    /// <summary>
    /// Called whenever the player's light energy value changes
    /// and updates the health bar accordingly.
    /// </summary>
    void OnLightChanged(float currentEnergy)
    {
        if (!isLocalPlayer) { return; }
        if (maxHealth == 0 || player == null) { InitializePlayer(); }        
        currentHealth = currentEnergy;
        healthBar.sizeDelta = new Vector2(currentHealth * multiplier, healthBar.sizeDelta.y);
    }

}