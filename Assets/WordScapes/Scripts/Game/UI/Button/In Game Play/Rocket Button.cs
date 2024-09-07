using TMPro;
using UnityEngine;

public class RocketButton : RightButtonBase, IBoosterButton
{
    [Header("Amount & Cost")]
    public GameObject amountObj;
    public TextMeshProUGUI textAmount;

    public TextMeshProUGUI textCost;

    protected override void OnClick()
    {
        base.OnClick();

        GameEvent.onClickRocket?.Invoke();
    }

    public void OnAmountChanged(int amount)
    {
        if (amount == 0)
        {
            amountObj.SetActive(false);
        }
        else
        {
            amountObj.SetActive(true);
            textAmount.text = amount.ToString();
        }
    }

    private void OnEnable()
    {
        GameEvent.inGameplay += base.OnEnableButton;
        GameEvent.amountRocketChanged += OnAmountChanged;

        OnAmountChanged(DataManager.numRocket);
        textCost.text = DataManager.Instance.costRocket.ToString();
    }

    private void OnDisable()
    {
        GameEvent.inGameplay -= base.OnEnableButton;
        GameEvent.amountRocketChanged -= OnAmountChanged;
    }
}
