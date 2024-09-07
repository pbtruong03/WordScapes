using TMPro;
using UnityEngine;

public class IdeaButton : RightButtonBase, IBoosterButton
{
    [Header("Amount & Cost")]
    public GameObject amountObj;
    public TextMeshProUGUI textAmount;

    public TextMeshProUGUI textCost;

    public void OnAmountChanged(int amount)
    {
        if(amount == 0)
        {
            amountObj.SetActive(false);
        }
        else
        {
            amountObj.SetActive(true);
            textAmount.text = amount.ToString();
        }
    }

    protected override void OnClick()
    {
        base.OnClick();
        GameEvent.onClickIdea?.Invoke();
    }
    private void OnEnable()
    {
        GameEvent.inGameplay += base.OnEnableButton;
        GameEvent.amountIdeaChanged += OnAmountChanged;

        OnAmountChanged(DataManager.numIdea);
        textCost.text = DataManager.Instance.costIdea.ToString();
        Debug.Log($"--------- Button Idea: {DataManager.Instance.costIdea}");
    }
    private void OnDisable()
    {
        GameEvent.amountIdeaChanged -= OnAmountChanged;
        GameEvent.inGameplay -= base.OnEnableButton;
    }
}
