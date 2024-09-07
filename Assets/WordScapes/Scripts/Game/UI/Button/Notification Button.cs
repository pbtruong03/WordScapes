using UnityEngine;

public class NotificationButton : ToggleButtonBase
{
    protected override void ApplyChange()
    {
        Debug.Log($"Notification: " + isOn);
    }
}
