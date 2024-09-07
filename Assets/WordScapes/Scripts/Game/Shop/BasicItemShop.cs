using UnityEngine;
using UnityEngine.UI;

public class BasicItemShop : ItemShopUIBase
{
    [Header("Value")]
    public ValueShopUI valueUI;

    public override void SetData(ShopPackItem packItem)
    {
        base.SetData(packItem);

        valueUI.textValue.text = packItem.items[0].value.ToString();
        valueUI.iconType.sprite = packItem.items[0].iconType;
    }
}
