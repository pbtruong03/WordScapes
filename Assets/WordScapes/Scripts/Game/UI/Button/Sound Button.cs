using UnityEngine;

public class SoundButton : ToggleButtonBase
{
    protected override void ApplyChange()
    {
        DataManager.soundOn = isOn;
        AudioManager.Instance.sfxSource.mute = !isOn;
    }
}
