using UnityEngine;
using System.Collections;

/// <summary>
/// The Billboard class keeps the health bar upright and above
/// the player.
///
/// @author - Stella L.
/// @author - Karl C.
/// @version - 1.0.0
///
/// </summary>
public class Billboard : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Hight of the health bar.")]
    private float hightOffset;
    private GameObject player;
    private Transform probeModel;
    
    /// <summary>
    /// Initializes the billboard.
    /// </summary>
    void Start()
    {
        player = transform.parent.gameObject;
        probeModel = player.transform.FindChild("ProbeModel");
        transform.parent = null;
    }
    
    /// <summary>
    /// Sets the health bar upright and above the player.
    /// </summary>
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        PlaceAbovePlayer();
    }
    
    /// <summary>
    /// Sets the health bar above the player.
    /// </summary>
    private void PlaceAbovePlayer()
    {
        if (probeModel != null && probeModel.gameObject.activeSelf == true)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);    
            }
            Vector3 newPosition = player.transform.position;
            newPosition.y += hightOffset;
            transform.position = newPosition;           
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);    
            }
                                 
        }
    }

}
