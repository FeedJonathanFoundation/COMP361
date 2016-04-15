using UnityEngine;
using UnityEngine.Networking;

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
    private NetworkStartPosition[] spawnPoints;
    
    void Start()
    {
        if (!isLocalPlayer) { return; }
        InitializePlayer(); 
        InitializeHealthBar();
        player.LightEnergy.LightChanged += OnLightChanged;

        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
    }
    
    void OnDisable()
    {
        if (!isLocalPlayer) { return; }
        player.LightEnergy.LightChanged -= OnLightChanged;
    }
    
    void InitializePlayer()
    {
        player = GetComponent<Player>();
    }
    
    void InitializeHealthBar()
    {
        if (!isLocalPlayer) { return; }
        maxHealth = player.DefaultEnergy;
        currentHealth = maxHealth;
        
        multiplier = healthBarWidth / maxHealth;
        
        healthBar.sizeDelta = new Vector2(currentHealth * multiplier, healthBar.sizeDelta.y);
    }
    
    void OnLightChanged(float currentEnergy)
    {
        if (!isLocalPlayer) { return; }
        if (maxHealth == 0 || player == null) { InitializePlayer(); }        
        currentHealth = currentEnergy;
        healthBar.sizeDelta = new Vector2(currentHealth * multiplier, healthBar.sizeDelta.y);
    }

}