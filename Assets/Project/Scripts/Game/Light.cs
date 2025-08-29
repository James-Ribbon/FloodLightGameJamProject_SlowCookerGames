using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;

public class Light : MonoBehaviour, ILight
{
    [Header("Light Profile")]
    [SerializeField] private LightProfileSO lightProfile;
    [Space]
    [SerializeField] private SpriteRenderer lightRenderer;

    public bool IsOn { get; private set; }

    private void Awake()
    {
        if (lightRenderer == null)
            lightRenderer = GetComponent<SpriteRenderer>();

        if (lightProfile == null)
        {
            Debug.LogWarning("No LightProfileSO assigned.");
        }
    }

    public void SetColour(Color colour)
    {
        if (!IsOn) return;

        lightRenderer.color = colour;
    }

    public void SetColorByType(LightColorType colorType)
    {
        if (!IsOn) return;

        Color targetColor = colorType switch
        {
            LightColorType.Off => lightProfile.OffColour,
            LightColorType.NoConnection => lightProfile.NoConnectionColour,
            LightColorType.Reconnect => lightProfile.ReconnectColour,
            LightColorType.Signal => lightProfile.SignalColour,
            _ => lightProfile.OffColour
        };

        lightRenderer.color = targetColor;
    }

    public void Toggle()
    {
        if (IsOn)
            TurnOff();
        else
            TurnOn();
    }

    public void TurnOn()
    {
        IsOn = true;
        SetColorByType(LightColorType.Reconnect);
    }

    public void TurnOff()
    {
        IsOn = false;
        SetColorByType(LightColorType.Off);
    }
}

public enum LightColorType
{
    Off,
    NoConnection,
    Reconnect,
    Signal
}