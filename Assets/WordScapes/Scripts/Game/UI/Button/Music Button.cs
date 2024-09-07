using UnityEngine;

public class MusicButton : ToggleButtonBase
{
    protected override void ApplyChange()
    {
        DataManager.musicOn = isOn;
        AudioManager.Instance.musicSource.mute = !isOn;
    }
}
