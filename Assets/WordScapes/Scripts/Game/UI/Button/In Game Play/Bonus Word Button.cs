using UnityEngine;

public class BonusWordButton : LeftButtonBase
{
    public GameObject bonusWordContainer;

    protected override void OnClick()
    {
        base.OnClick();
        bonusWordContainer.SetActive(!bonusWordContainer.activeSelf);
        GameEvent.displayBonusWord?.Invoke();
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
