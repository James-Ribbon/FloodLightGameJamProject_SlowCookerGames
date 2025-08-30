using System.Runtime.CompilerServices;
using UnityEngine;

public class ModemSignalController : MonoBehaviour
{
    [Header("Signal Settings")]
    public ModemSettings settings;// { get; private set; }

    public float SignalStrength;// { get; private set; }
    public float FluctuationFrequency => settings.fluctuationFrequency;

    private float fluctuationTimer;
    private bool isFluctuating;

    private void Start()
    {
        EnableFluctuation(settings.enableFluctuation);
        SignalStrength = (settings.minSignalStrength + settings.maxSignalStrength) / 2f;
    }

    public void UpdateSignal(float deltaTime)
    {
        if (!isFluctuating) return;

        fluctuationTimer += deltaTime;
        if (fluctuationTimer >= 1f / settings.fluctuationFrequency)
        {
            GenerateNewSignal();
            fluctuationTimer = 0f;
        }
    }

    private void GenerateNewSignal()
    {
        //float noise = Mathf.PerlinNoise(Time.time * settings.fluctuationFrequency, 0f);
        float rand = Random.value;

        SignalStrength = Mathf.Lerp(settings.minSignalStrength, settings.maxSignalStrength, rand);
        Debug.Log($"Updated Signal Strength: {SignalStrength}");
        //ModemEvents.InvokeSignalStrengthChanged(SignalStrength);
    }

    public void SetStaticSignal(float strength)
    {
        SignalStrength = Mathf.Clamp(strength, settings.minSignalStrength, settings.maxSignalStrength);
        //ModemEvents.InvokeSignalStrengthChanged(SignalStrength);
    }

    public void EnableFluctuation(bool enable)
    {
        isFluctuating = enable;
    }

    public void UpdateSettings(ModemSettings newSettings)
    {
        settings = newSettings;
        EnableFluctuation(settings.enableFluctuation);
    }
}
