using UnityEngine;

/// <summary>
/// Flare object extends LightSource
///
/// @author - Jonathan L.A
/// @author - Alex I.
/// @version - 1.0.0
///
/// </summary>
public class Flare : LightSource
{
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

    private new Rigidbody rigidbody;            //rigibody information, init in Start()
    private Light lightObject;                  //light of the flare, init in Start()
    private LensFlare flareLens;                //controls brightness of flare, init in Start()
    private float intensity;                    //intensity of light, used for decay of light
    private float timer = 0;                    //timer for decaylight

    // Use this for initialization
    protected void Start()
    {
        this.rigidbody = GetComponent<Rigidbody>();
        this.lightObject = gameObject.GetComponentInChildren<Light>();
        this.flareLens = lightObject.GetComponent<LensFlare>();
        this.rigidbody.velocity = transform.right * speed;
        Destroy(gameObject, destroyTime);
    }

    protected override void Update()
    {
        base.Update();
        if ((timer += Time.deltaTime) > timeBeforeDecay)
        {
            lightObject.intensity -= Time.deltaTime * lightReduction;
            flareLens.brightness -= Time.deltaTime * fadeSpeed;
            timer = 0.0f;
            timeBeforeDecay = decayRateTime;
        }

    }
}
