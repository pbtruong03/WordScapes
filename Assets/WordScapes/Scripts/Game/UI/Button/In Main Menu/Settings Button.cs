using UnityEngine;

public class SettingsButton : BottomButtonBase
{
    public GameObject settingsPanel;

    protected override void OnClick()
    {
        base.OnClick();
        settingsPanel.SetActive(true);
    }

    private void OnEnable()
    {
        GameEvent.inMainMenu += OnEnableButton;
    }
    private void OnDisable()
    {
        GameEvent.inMainMenu -= OnEnableButton;
    }
}
