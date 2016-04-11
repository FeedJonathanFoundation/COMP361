using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Hight of the health bar.")]
    private float hightOffset;
    private GameObject player;
    void Start()
    {
        Debug.Log("start");
        player = GameObject.FindWithTag("Player");
        transform.parent = null;
    }
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        Vector3 newPosition = player.transform.position;
        newPosition.y += hightOffset;
        transform.position = newPosition;
    }
}
