using System;
using UnityEngine;

[Serializable]
public class ModemSettings
{
    public float minSignalStrength = 0f;
    public float maxSignalStrength = 1f;
    public float fluctuationFrequency = 0.5f;
    public bool enableFluctuation = true;
    public float connectionCheckInterval = 2f;
    public float reconnectAttemptInterval = 5f;
}