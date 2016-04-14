using UnityEngine;

[System.Serializable]
public class FlareBean
{
    public float coolDown;
    public float flareEnergyCost;
    public float flareCostPercentage;
    public float recoilForce;
    [Tooltip("Refers to the flare spawn zone")]
    public Transform flareSpawnObject;

}