using DG.Tweening;
using UnityEngine;

public class DiamondButton : ResourceButton
{
    protected override void Start()
    {
        base.Start();
        value = DataManager.diamond;
        textValue.text = value.ToString();
    }

    protected override void OnClick()
    {
        base.OnClick();
    }

    public void OnHidenButton(bool hide)
    {
        if (!hide)
        {
            rectTransform.DOPivotY(0.5f, 0.5f).SetEase(Ease.OutSine);
        }
        else
        {
            rectTransform.DOPivotY(-10f, 0.5f).SetEase(Ease.OutSine);
        }
    }

    private void OnEnable()
    {
        GameEvent.diamondChanged += OnValueChange;
        GameEvent.inGameplay += OnHidenButton;
    }
    private void OnDisable()
    {
        GameEvent.diamondChanged -= OnValueChange;
        GameEvent.inGameplay -= OnHidenButton;
    }
}
