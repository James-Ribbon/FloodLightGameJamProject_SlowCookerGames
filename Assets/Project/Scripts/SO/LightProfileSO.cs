using UnityEngine;

[CreateAssetMenu(fileName = "LightProfile", menuName = "ScriptableObjects/LightProfile")]
public class LightProfileSO : ScriptableObject
{
    [SerializeField] private Color offColour;
    [SerializeField] private Color noConnectionColour;
    [SerializeField] private Color reconnectColour;
    [SerializeField] private Color signalColour;

    public Color OffColour => offColour;
    public Color NoConnectionColour => noConnectionColour;
    public Color ReconnectColour => reconnectColour;
    public Color SignalColour => signalColour;

    public Color GetColor(LightColorType colorType)
    {
        return colorType switch
        {
            LightColorType.Off => offColour,
            LightColorType.NoConnection => noConnectionColour,
            LightColorType.Reconnect => reconnectColour,
            LightColorType.Signal => signalColour,
            _ => offColour  // Default fallback
        };
    }

    private void OnValidate()
    {
        offColour.a = 1f;
        noConnectionColour.a = 1f;
        reconnectColour.a = 1f;
        signalColour.a = 1f;
    }
}
