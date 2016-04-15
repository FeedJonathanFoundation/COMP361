using UnityEngine;

/// <summary>
/// Flare object extends LightSource
/// </summary>
public class Flare : LightSource
{
    //variables not for user
    private new Rigidbody rigidbody;            //rigibody information, init in Start()
    private Light lightObject;                  //light of the flare, init in Start()
    private LensFlare flareLens;                //controls brightness of flare, init in Start()
    private float intensity;                    //intensity of light, used for decay of light
    private float timer = 0;                    //timer for decaylight

    //variables changable in the interface
    
    [Tooltip("Speed at which the flare travels")]
    [SerializeField]
    private float speed;
    
    [Tooltip("Time before the light starts diminishing")]
    [SerializeField]
    private float timeBeforeDecay;
    
    [Tooltip("How long the objects last before being destroyed")]
    [SerializeField]
    private float destroyTime;
    
    [Tooltip("Time interval in which the light diminishes")]
    [SerializeField]
    private float decayRateTime;
    
    [Tooltip("The higher the value, the faster light(flare) diminishes")]
    [SerializeField]
    private float fadeSpeed;
    
    [Tooltip("The higher the value, the faster light(spot) diminishes")]
    [SerializeField]
    private float lightReduction;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        lightObject = gameObject.GetComponentInChildren<Light>();
        flareLens = lightObject.GetComponent<LensFlare>();

        rigidbody.velocity = transform.right * speed;
        Destroy(gameObject, destroyTime);
    }
	
	// Update is called once per frame
	void Update()
    {
        //might need to put if rotating!    
        //timers could help with frames
        
        if ((timer += Time.deltaTime) > timeBeforeDecay)
        {
            lightObject.intensity -= Time.deltaTime * lightReduction;
            flareLens.brightness -= Time.deltaTime * fadeSpeed;
            timer = 0.0f;
            timeBeforeDecay = decayRateTime;
        }
        
	}
}
