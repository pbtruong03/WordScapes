using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceButton : ButtonBase
{
    public Image iconImage;
    public TextMeshProUGUI textValue;
    public int value;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        textValue = GetComponentInChildren<TextMeshProUGUI>();
    }

    protected virtual void OnValueChange(int newValue)
    {
        StartCoroutine(IECounter(newValue));
    }

    protected override void OnClick()
    {
        base.OnClick();
        iconImage.transform.localScale = Vector3.one;
        iconImage.transform.DOPunchScale(Vector3.one * 0.2f, 0.35f, 1, 1);

        PopupManager.Instance.OpenShop();
    }

    IEnumerator IECounter(int newValue)
    {
        int differenceValue = Mathf.Abs(value - newValue);

        int increase = (differenceValue > 23) ? (differenceValue/23) : 1;

        if (value < newValue)
        {
            while (value < newValue)
            {
                value += increase;
                textValue.text = value.ToString();
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            increase *= -1;
            while (value > newValue)
            {
                value += increase;
                textValue.text = value.ToString();
                yield return new WaitForEndOfFrame();
            }

        }
        value = newValue;
        textValue.text = value.ToString();

    }


}
