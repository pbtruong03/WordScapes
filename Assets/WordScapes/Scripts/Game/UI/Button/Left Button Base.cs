using DG.Tweening;

public abstract class LeftButtonBase : ButtonBase
{
    protected override void OnEnableButton(bool enable)
    {
        if (enable)
        {   
            rectTransform.DOPivotX(0.5f, 0.5f).SetEase(Ease.OutSine);
        }
        else
        {
            rectTransform.DOPivotX(10f, 0.5f).SetEase(Ease.OutSine);
        }
    }
}
