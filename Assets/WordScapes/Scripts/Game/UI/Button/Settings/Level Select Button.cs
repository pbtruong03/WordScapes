using DG.Tweening;
using UnityEngine;

public class LevelSelectButton : ButtonBase
{
    public GameObject selectLevel;
    public UISettings uiSettings;

    protected override void OnClick()
    {
        base.OnClick();

        uiSettings.DisablePanel();
        UIManager.Instance.DisplayLevelSelect();

        selectLevel.transform.localScale = Vector3.one * 0.5f;
        selectLevel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InFlash);
    }
}
