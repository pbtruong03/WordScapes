using UnityEngine;

public class ConvertButton : LeftButtonBase
{
    protected override void OnClick()
    {
        base.OnClick();
        GameEvent.onClickConvertLetters?.Invoke();
    }
    private void OnEnable()
    {
        GameEvent.inGameplay += base.OnEnableButton;
    }
    private void OnDisable()
    {
        GameEvent.inGameplay -= base.OnEnableButton;
    }
}
