using TMPro;
using UnityEngine;

public class PointButton : LeftButtonBase, IBoosterButton
{
    public bool onPointButton = true;
    public GameObject pointEntry;
    public GameObject pointCancel;
    [Space]
    public GameObject uiPointSupport;

    [Header("Amount & Cost")]
    public GameObject amountObj;
    public TextMeshProUGUI textAmount;

    public TextMeshProUGUI textCost;

    protected override void OnClick()
    {
        base.OnClick();

        SetStateButton(onPointButton);
        if (!onPointButton)
        {
            GameEvent.onPointerHint?.Invoke(onPointButton);
            GameEvent.onClickPoint?.Invoke();
        }
        else
        {
            GameEvent.onPointerHint?.Invoke(false);
        }
    }

    public void SetStateButton(bool state)
    {
        onPointButton = !state;
        pointEntry.SetActive(!state);

        pointCancel.SetActive(state);
    }

    public void OnPointSupport(bool value)
    {
        uiPointSupport.SetActive(value);
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
        OnAmountChanged(DataManager.numPoint);
        textCost.text = DataManager.Instance.costPoint.ToString();

        GameEvent.onPointerHint += SetStateButton;
        GameEvent.onPointerHint += OnPointSupport;
        GameEvent.amountPointChanged += OnAmountChanged;
    }

    private void OnDisable()
    {
        GameEvent.inGameplay -= base.OnEnableButton;
        GameEvent.onPointerHint -= SetStateButton;
        GameEvent.onPointerHint -= OnPointSupport;
        GameEvent.amountPointChanged -= OnAmountChanged;
    }
}
