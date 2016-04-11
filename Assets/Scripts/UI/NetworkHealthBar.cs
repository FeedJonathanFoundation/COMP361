using UnityEngine;
using System.Collections;
using UnityEngine.UI;
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
        
        // Debug.Log("currentEnergy: " + currentEnergy + "\n healthBar: " + healthBar.sizeDelta.x);
        currentHealth = currentEnergy;
        healthBar.sizeDelta = new Vector2(currentHealth * multiplier, healthBar.sizeDelta.y);
    }

    // public void OnRespawn()
    // {
    //     if (!isServer) { return; }
    //     RpcRespawn();
    // }

    // [ClientRpc]
    // void RpcRespawn()
    // {
    //     if (isLocalPlayer)
    //     {
    //         Vector3 spawnPoint = Vector3.zero;
    //         if (spawnPoints != null && spawnPoints.Length > 0)
    //         {
    //             spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
    //             // maybe have it iterate instead of being random
    //         }
    //         transform.position = spawnPoint;
    //     }
    // }

}