
using DG.Tweening;

public abstract class BottomButtonBase : ButtonBase
{
    protected override void OnEnableButton(bool enable)
    {
        if (enable)
        {
            rectTransform.DOPivotY(0.5f, 0.5f).SetEase(Ease.OutSine);
        }
        else
        {
            rectTransform.DOPivotY(10f, 0.5f).SetEase(Ease.OutSine);
        }
    }
}
