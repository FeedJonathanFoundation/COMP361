using UnityEngine;

/// <summary>
/// Stores the probe (player avatar) related variables imported from 
/// Unity Editor in a single self-contained class. 
///
/// @author - Alex I.
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class ProbeBean
{
    [Tooltip("Color of the probe with lights ON")]
    [SerializeField]
    private Color probeColorOn;    
    
    [Tooltip("Color of the probe with lights OFF")]
    [SerializeField]
    private Color probeColorOff;    
    
    [Tooltip("Color of the local probe with lights ON. Used for networking.")]
    [SerializeField]
    private Color localProbeColorOn;    
    
    [Tooltip("Color of the local probe with lights OFF. Used for networking.")]
    [SerializeField]
    private Color localProbeColorOff; 
    
    public Color ProbeColorOn
    {
        get { return probeColorOn; }
        set { probeColorOn = value; }
    }
    
    public Color ProbeColorOff
    {
        get { return probeColorOff; }
        set { probeColorOff = value; }
    }
    
    public Color LocalProbeColorOn
    {
        get { return localProbeColorOn; }
        set { localProbeColorOn = value; }
    }
    
    public Color LocalProbeColorOff
    {
        get { return localProbeColorOff; }
        set { localProbeColorOff = value; }
    }
    
}