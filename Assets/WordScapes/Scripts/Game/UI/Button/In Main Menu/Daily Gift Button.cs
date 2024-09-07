public class DailyGiftButton : RightButtonBase
{
    protected override void OnClick()
    {
        base.OnClick();
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
