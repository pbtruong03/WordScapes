using DG.Tweening;

public class CloseInGameButton : ButtonBase
{
    protected override void OnClick()
    {
        base.OnClick();
        UIManager.Instance.DisplayMainMenu();
        
        LevelManager.Instance.EndLevel(false);

        rectTransform.DOPivotX(5f, 0.3f);
    }

    private void OnEnable()
    {
        rectTransform.DOPivotX(0.5f, 0.3f);
    }
}
