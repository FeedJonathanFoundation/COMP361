using UnityEngine;
using System.Collections;

/// <summary>
/// Destroys a particle system n seconds after it is spawned
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
public class DestroyParticles : MonoBehaviour
{
    [Tooltip("The amount of time after spawning before the particles are destroyed")]
    [SerializeField]
    private float destroyTime = 7;

	/// <summary>
    /// Called when a DestroyParticles instance is created
    /// </summary>
	void Start () 
    {
        StartCoroutine(DestroyAfter(destroyTime));
	}

    /// <summary>
    ///  Deactivates this GameObject after 'time' seconds
    /// </summary>
    private IEnumerator DestroyAfter(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            yield return null;
        }

        gameObject.SetActive(false);
    }

}
