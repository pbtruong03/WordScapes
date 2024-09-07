using TMPro;
using UnityEngine;

public class SingleItemShop : ItemShopUIBase
{
    public TextMeshProUGUI textName;
    [Header("Value")]
    public ValueShopUI valueUI;

    public override void SetData(ShopPackItem packItem)
    {
        base.SetData(packItem);
        textName.text = packItem.namePack;

        if (packItem.key == ShopItemKey.remove_ads)
        {
            valueUI.textValue.text = "NO MORE ADS!";

            valueUI.iconType.gameObject.SetActive(false);
        }
        else
        {
            valueUI.textValue.text = packItem.items[0].value.ToString();

            valueUI.iconType.gameObject.SetActive(true);
            valueUI.iconType.sprite = packItem.items[0].iconType;
        }
    }
}
