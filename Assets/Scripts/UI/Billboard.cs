using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Hight of the health bar.")]
    private float hightOffset;
    private GameObject player;
    private Transform probeModel;
    
    void Start()
    {
        player = transform.parent.gameObject;
        probeModel = player.transform.FindChild("ProbeModel");
        transform.parent = null;
    }
    
    void Update()
    {
        transform.LookAt(Camera.main.transform);
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
