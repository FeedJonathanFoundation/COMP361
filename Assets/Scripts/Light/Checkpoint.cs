using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Checkpoint class is responsible for behaviour related to Checkpoint object. 
/// It recharges player object with energy on collision and saves current progress.
///
/// Optionally, it switches scenes, to bring the player to the next level.
///
/// @author - Alex I.
/// @version - 1.0.0
///
/// </summary>
public class Checkpoint : LightSource
{
    [SerializeField]
    [Tooltip("If set to true, this checkpoint will teleport player to the next scene")]
    private bool changeScene = false;
    
    protected override void Awake()
    {       
        base.Awake();
        this.InfiniteEnergy = true; // override default LightSource value
    }

    /// <summary>
    /// Invoked when players collider intersects with checkpoint collider
    /// </summary>
    /// <param name="other"></param>
    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.name == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();                                    
            PlayerData data = new PlayerData();
            
            if (changeScene)
            {
                // if checkpoint changes scene, save values for the new scene
                data.playerPosition = VectorExtensions.Vector3ToString(new Vector3(0, 0, 0));                 
            } 
            else
            {
                data.playerPosition = VectorExtensions.Vector3ToString(other.gameObject.transform.position);
            }            
            
            data.playerRotation = VectorExtensions.Vector3ToString(other.gameObject.transform.localEulerAngles);
            data.playerScale = VectorExtensions.Vector3ToString(other.gameObject.transform.localScale);
            data.playerEnergy = other.gameObject.GetComponent<Player>().LightEnergy.CurrentEnergy;            
            DataManager.SaveFile(data);    
            
            if (changeScene)
            {
                StartCoroutine(ChangeLevel());                                            
            }        
        }
    }
    
    /// <summary>
    /// If changeScene option is set to true, coroutine is invoked to smoothly
    /// fade out to black and fade in on the new level.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeLevel()
    {
        GameObject camera = GameObject.Find("Main Camera");
        float fadeTime = 0;
        
        // Apply camera fade while loading the next scene
        if (camera != null && camera.GetComponent<Fade>())
        {                
            fadeTime = camera.GetComponent<Fade>().BeginFade(1);                                        
        }
        
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);            
        
        // Destroy checkpoint to prevent it from appearing in the next scene
        Destroy(this.gameObject); 
    }

}


