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
        Debug.Log("start");
        player = transform.parent.gameObject;
        probeModel = player.transform.FindChild("ProbeModel");
        transform.parent = null;
    }
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        if(probeModel.name == "ProbeModel" && probeModel.gameObject.activeSelf == true)
        {
            Vector3 newPosition = player.transform.position;
            newPosition.y += hightOffset;
            transform.position = newPosition;
        }
        else
        {
            transform.gameObject.SetActive(false);
        }
        
    }
}
