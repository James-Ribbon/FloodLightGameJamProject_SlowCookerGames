using UnityEngine;

public interface ISwitch
{
    bool IsOn { get; }
    void Toggle();
    void TurnOn();
    void TurnOff();
}
