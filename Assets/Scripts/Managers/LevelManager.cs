using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Set of static classes to manage player level (unity scenes).
/// New game/load game functionality and initial settings. 
/// 
/// @author - Alex I.
/// @version - 1.0.0
///
/// </summary>
public class LevelManager : MonoBehaviour
{
    private int currentLevel;  
    private bool disableCheckpoints;
    private Player player;
    
    protected void Awake()
    {
        this.currentLevel = SceneManager.GetActiveScene().buildIndex;
        this.disableCheckpoints = true;
        this.player = GetComponent<Player>();
        ResetPlayerState();
        LoadGame();
    }
            
    /// <summary>
    /// Invoked when a new scene is loaded
    /// </summary>
    protected void OnLevelWasLoaded(int level)
    {
        Debug.Log("Scene " + level + " is loaded!");
        ResetPlayerState();
    }
    
    /// <summary>
    /// Loads the last saved game state on the scene or places player at the origin
    /// </summary>
    private void LoadGame()
    {
        PlayerData data = DataManager.LoadFile();
        if (data != null && !disableCheckpoints)
        {
            if (data.levelID != this.currentLevel)
            {
                if (SceneManager.sceneCountInBuildSettings > data.levelID)
                {
                    SceneManager.LoadScene(data.levelID, LoadSceneMode.Single);
                }
            }
            this.player.Transform.position = VectorExtensions.Vector3FromString(data.playerPosition);
            this.player.Transform.localEulerAngles = VectorExtensions.Vector3FromString(data.playerRotation);
        }        
    }
    
    /// <summary>
    /// Resets player position, rotation and velocity when the scene
    /// is reloaded
    /// </summary>
    public void ResetPlayerState()
    {
        player = GetComponent<Player>();
        if (player != null)
        {
            this.player.Rigidbody.velocity = Vector3.zero;
            this.player.Transform.position = Vector3.zero;
            this.player.Transform.localEulerAngles = new Vector3(0, 0, -90);   
        }        
    }
    
    public int CurrentLevel
    {
        get { return this.currentLevel; }
        set { this.currentLevel = value; }
    }


   
}