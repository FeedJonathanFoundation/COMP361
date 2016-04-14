using UnityEngine;
using System.Collections;

/// <summary>
/// ????
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class Seek : NPCActionable
{   
    private Transform targetTransform;
    private SoundManager soundManager;
    
    
    public Seek(int priority, string id, Transform transform) : base(priority, id)
    {
        targetTransform = transform;
    }
    
    void Start()
    {
        GameObject soundObject = GameObject.FindWithTag("SoundManager");
        if (soundObject != null)
        {
            soundManager = soundObject.GetComponent<SoundManager>();
        }
    }
    
	public override void Execute(Steerable steerable) 
    {
        base.Execute(steerable);
        
        if (targetTransform)
        {
            // Override the steerable's min/max speed
            if (overrideSteerableSpeed)
            {
                steerable.MinSpeed = minSpeed;
                steerable.MaxSpeed = maxSpeed;
            }
            // Override the steerable's max force
            if (overrideMaxForce)
            {
                steerable.MaxForce = maxForce;
            }
            
            // If player's lights are on, seek player
            if (targetTransform.gameObject.CompareTag("Player")) 
            {
                Player player = targetTransform.gameObject.GetComponent<Player>();
                if (player.IsDetectable())
                {
                    steerable.AddSeekForce(targetTransform.position, strengthMultiplier);

                    PlaySeekSound();


                }
            }
            else
            {
                steerable.AddSeekForce(targetTransform.position, strengthMultiplier);
            }
            
        }
    }
    
    private void PlaySeekSound()
    {
        if (soundManager != null)
        {
            soundManager.PlaySound("Detection", targetTransform.gameObject);
        }
    }
    
    
    /// <summary>
    /// The transform to seek
    /// </summary>
    public Transform TargetTransform
    {
        get { return targetTransform; }
        set { targetTransform = value; }
    }
}
