using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

/// <summary>
/// Object Pooler class creates a pool of game objects for reuse.
///
/// @author - Stella L.
/// @version - 1.0.0
///
/// </summary>
public class ObjectPooler : NetworkBehaviour
{

    public static ObjectPooler current;
    
    [Tooltip("Game objects to pool and spawn.")]
    [SerializeField]
    private GameObject[] pooledObjects;
    
    [Tooltip("Number of game objects to pool (in order).")]
    [SerializeField]
    private int[] pooledAmount;
    
    [Tooltip("If true, more game objects will be instantiated if they 'run out' of pooled objects.")]
    [SerializeField]
    private bool extensible;

    [Tooltip("If true and if they 'run out' of the desired pooled object, another available object will be substituted.")]
    [SerializeField]
    private bool replace;

    private List<GameObject>[] pool;
    
    /// <summary>
    /// Initializes the global pool
    /// </summary>
    void Awake()
    {
        current = this;
    }
    
    /// <summary>
    /// Creates the pool
    /// </summary>
    public void Start()
    {
        pool = new List<GameObject>[pooledObjects.Length];
        if (!isServer) { return; }
        for (int i = 0; i < pooledObjects.Length; i++)
        {
            pool[i] = new List<GameObject>();
            for (int j = 0; j < pooledAmount[i]; j++)
            {
                GameObject gameobject = (GameObject)Instantiate(pooledObjects[i]);

                AbstractFish fish = gameobject.GetComponent<AbstractFish>();
                if (fish != null)
                {
                    fish.OnActiveChange(false);
                }
                else
                {
                    gameobject.SetActive(false);
                }
                pool[i].Add(gameobject);
                
                LightSource lightSource = gameobject.GetComponent<LightSource>();
                NpcID npcID = gameobject.GetComponent<NpcID>();
                if (npcID != null)
                {
                    string identity =  lightSource.LightSourceID;
                    npcID.ID = identity;
                }
                
            }
        }
    }
    
	
    /// <summary>
    /// Returns an object from the pool that is not in use
    /// </summary>
	public GameObject GetPooledObject(int objectID)
    {
        if (objectID > pooledObjects.Length)
        {
            Debug.Log("Trying to rerieve a pooled object that does not exist. Add new object to pooledObjects array.");
            return null;
        }
        for (int i = 0; i < pool[objectID].Count; i++)
        {
            GameObject current = pool[objectID][i];
            if (!current.activeInHierarchy)
            {
                ReactivateObjectLight(current);
                return current;
            }
        }
        if (replace && FindReplacement() != null)
        {
            return FindReplacement();
        }
        if (extensible)
        {
            return ExtendPool(objectID);
        }
        return null;
    }
    
    /// <summary>
    /// Returns an object from the pool that is not in use
    /// that is not the desired object (it is a replacement)
    /// </summary>
    
    private GameObject FindReplacement()
    {
        for (int i = 0; i < pooledObjects.Length; i++)
        {
            for (int j = 0; j < pool[i].Count; j++)
            {
                GameObject current = pool[i][j];
                if (!current.activeInHierarchy)
                {
                    ReactivateObjectLight(current);
                    return current;
                }
            }
        }
        return null;
    }
    
    /// <summary>
    /// Returns a new object if the pool is already full
    /// </summary>
    private GameObject ExtendPool(int objectID)
    {
        GameObject gameobject = (GameObject)Instantiate(pooledObjects[objectID]);
        pool[objectID].Add(gameobject);
        return gameobject;
    }
    
    /// <summary>
    /// Resets the pool, as if it had never been used
    /// </summary>
    public void ResetPool()
    {
        for (int i = 0; i < pooledObjects.Length; i++)
        {
            for (int j = 0; j < pool[i].Count; j++)
            {
                GameObject current = pool[i][j];
                if (current.activeSelf == true)
                {
                    AbstractFish fish = current.GetComponent<AbstractFish>();
                    if (fish != null)
                    {
                        fish.OnActiveChange(false);
                    }
                    else
                    {
                        current.SetActive(false);
                    }
                }
                ReactivateObjectLight(current);
            }
        }
    }
    
    /// <summary>
    /// Restores the default energy of the pooled object
    /// </summary>
    private void ReactivateObjectLight(GameObject current)
    {
        LightSource light = current.GetComponent<LightSource>();
        if (light != null)
        {
            light.LightEnergy.Add(light.DefaultEnergy);
            AbstractFish fish = current.GetComponent<AbstractFish>();
            if (fish)
            {
                fish.Dead = false;
            }
        }
    }
    
    /// <summary>
    /// Returns number of objects in the pool
    /// </summary>
    public int PooledObjectCount
    {
        get
        {
            return pooledObjects.Length;
        }
    }
}
