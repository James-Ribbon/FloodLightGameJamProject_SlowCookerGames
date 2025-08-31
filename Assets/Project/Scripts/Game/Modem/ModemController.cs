using System;
using UnityEngine;

public class ModemController : MonoBehaviour, ISwitch
{
    [Header("Settings")]
    //[SerializeField] private ModemSettings settings;

    [SerializeField] private ModemSignalController signalController;
    [SerializeField] private Light[] lights;

    public bool IsOn { get; private set; }

    private ConnectionStrength currentConnectionStrength;
    private float connectionCheckTimer;
    private float reconnectTimer;

    private void Awake()
    {
        //TurnOff();
        TurnOn();
    }

    private void OnEnable()
    {
       
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        if (!IsOn) return;

        signalController?.UpdateSignal(Time.deltaTime);

        connectionCheckTimer -= Time.deltaTime;

        if(connectionCheckTimer <= 0f)
        {
            CheckConnection();
            connectionCheckTimer = signalController.settings.connectionCheckInterval;
        }

        //Debug.Log($"Connection Check Timer: {connectionCheckTimer}");

        HandleReconnection();
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
        foreach (var light in lights)
        {
            light.TurnOn();
        }
    }

    public void TurnOff()
    {
        IsOn = false;
 
        foreach (var light in lights)
        {
            light.TurnOff();
        }
    }

    private void CheckConnection()
    {
        if (signalController != null)
        {
            UpdateConnectionStrength(signalController.SignalStrength);
        }
    }

    private void HandleSignalStrengthChanged(float strength)
    {
        if (IsOn)
        {
            UpdateConnectionStrength(strength);
        }
    }

    private void UpdateConnectionStrength(float strength)
    {
        ConnectionStrength newConnectionStrength;

        if (strength > 0.8f)
            newConnectionStrength = ConnectionStrength.Strong;
        else if (strength > 0.6f)
            newConnectionStrength = ConnectionStrength.Moderate;
        else if (strength > 0.1f)
            newConnectionStrength = ConnectionStrength.Weak;
        else
            newConnectionStrength = ConnectionStrength.NoConnection;
        /*else if (strength > 0.1f)
            newConnectionStrength = ConnectionStrength.AttemptingReconnect;
        else
            newConnectionStrength = ConnectionStrength.NoConnection;*/

        if (newConnectionStrength != currentConnectionStrength)
        {
            currentConnectionStrength = newConnectionStrength;
            UpdateLights();
            //ModemEvents.InvokeConnectionChanged(currentConnectionStrength);
        }

        //Debug.Log($"Connection Strength: {newConnectionStrength.ToString()}");
    }

    private void UpdateLights()
    {
        switch (currentConnectionStrength)
        {
            case ConnectionStrength.Strong:
                SetAllLights(LightColorType.Signal);
                break;
            case ConnectionStrength.Moderate:
                SetLights(LightColorType.Signal, LightColorType.Signal, LightColorType.NoConnection);
                break;
            case ConnectionStrength.Weak:
                SetLights(LightColorType.Signal, LightColorType.NoConnection, LightColorType.NoConnection);
                break;
            case ConnectionStrength.AttemptingReconnect:
                SetAllLights(LightColorType.Reconnect);
                break;
            case ConnectionStrength.NoConnection:
                SetAllLights(LightColorType.NoConnection);
                break;
            case ConnectionStrength.Off:
                break;
        }
    }

    private void SetAllLights(LightColorType colorType)
    {
        foreach (var light in lights)
        {
            if (light is Light modemLight)
            {
                modemLight.SetColorByType(colorType);
            }
            else
            {
                // Fallback to direct color setting
                Color color = colorType switch
                {
                    LightColorType.Off => Color.gray,
                    LightColorType.NoConnection => Color.red,
                    LightColorType.Reconnect => new Color(1f, 0.5f, 0f),
                    LightColorType.Signal => Color.green,
                    _ => Color.gray
                };
                light.SetColour(color);
            }
        }
    }

    private void SetLights(LightColorType color1, LightColorType color2, LightColorType color3)
    {
        if (lights.Length >= 1) SetLightColor(0, color1);
        if (lights.Length >= 2) SetLightColor(1, color2);
        if (lights.Length >= 3) SetLightColor(2, color3);
    }

    private void SetLightColor(int index, LightColorType colorType)
    {
        if (lights[index] is Light modemLight)
        {
            modemLight.SetColorByType(colorType);
        }
        else
        {
            // Fallback
            Color color = colorType switch
            {
                LightColorType.Off => Color.gray,
                LightColorType.NoConnection => Color.red,
                LightColorType.Reconnect => new Color(1f, 0.5f, 0f),
                LightColorType.Signal => Color.green,
                _ => Color.gray
            };
            lights[index].SetColour(color);
        }
    }

    private void HandleReconnection()
    {
        if (currentConnectionStrength == ConnectionStrength.AttemptingReconnect)
        {
            reconnectTimer -= Time.deltaTime;
            if (reconnectTimer <= 0f)
            {
                CheckConnection(); // Attempt to reconnect
                reconnectTimer = signalController.settings.reconnectAttemptInterval;

                // Optional: Add visual feedback for reconnection attempts
                BlinkLightsForReconnection();
            }
        }
        else
        {
            // Reset timer when not in reconnection state
            reconnectTimer = signalController.settings.reconnectAttemptInterval;
        }
    }

    // Optional helper method for visual feedback
    private void BlinkLightsForReconnection()
    {
        // Simple blink effect - turn lights off briefly
        foreach (var light in lights)
        {
            light.TurnOff();
        }

        // Turn them back on after a short delay
        Invoke(nameof(RestoreLightsAfterBlink), 0.2f);
    }

    private void RestoreLightsAfterBlink()
    {
        if (IsOn && currentConnectionStrength == ConnectionStrength.AttemptingReconnect)
        {
            foreach (var light in lights)
            {
                light.TurnOn();
                light.SetColorByType(LightColorType.Reconnect);
            }
        }
    }

    // Method to update all lights with a new profile
    /*    public void UpdateAllLightProfiles(LightProfileSO newProfile)
        {
            foreach (var light in lights)
            {
                if (light is Light modemLight)
                {
                    modemLight.UpdateLightProfile(newProfile);
                }
            }
            // Refresh current connection display
            UpdateLights();
        }*/
}



public enum ConnectionStrength
{
    Off,                
    NoConnection,       
    AttemptingReconnect,
    Weak,               
    Moderate,         
    Strong             
}