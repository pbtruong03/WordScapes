

public class GiftButton : RightButtonBase
{
    protected override void OnClick()
    {
        base.OnClick();
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
