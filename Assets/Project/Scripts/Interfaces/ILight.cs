using UnityEngine;

public interface ILight
{
    void SetColour(Color colour);
    void SetColorByType(LightColorType colorType);
    void TurnOn();
    void TurnOff();
}
