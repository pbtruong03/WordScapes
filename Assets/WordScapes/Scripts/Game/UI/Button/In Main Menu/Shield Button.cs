using UnityEngine;

public class ShieldButton : BottomButtonBase
{
    [SerializeField] private GameObject leaderBoard;

    protected override void OnClick()
    {
        base.OnClick();

        UIManager.Instance.CloseAllUI();
        leaderBoard.SetActive(true);
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
