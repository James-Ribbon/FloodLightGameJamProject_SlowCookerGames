using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    //Power Events
    public static UnityAction<Component> OnPowerLost;
    public static UnityAction<Component> OnPowerRestored;

    //Uploading Events
    public static UnityAction<Component> OnUploadStarted;
    public static UnityAction<Component> OnUploadCompleted;
    public static UnityAction<Component> OnUploadInterrupted;

    //Generic Events
    public static UnityAction OnGamePaused;
    public static UnityAction OnGameResumed;
    public static UnityAction OnGameOver;
    public static UnityAction OnLevelComplete;
}
