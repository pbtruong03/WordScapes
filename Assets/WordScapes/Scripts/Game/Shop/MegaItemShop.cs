using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MegaItemShop : ItemShopUIBase
{
    public TextMeshProUGUI textName;
    [Header("Value")]
    public List<ValueShopUI> listValueUI;

    public override void SetData(ShopPackItem packItem)
    {
        base.SetData(packItem);

        textName.text = packItem.namePack;

        int i = 0;
        while (i < listValueUI.Count && i < packItem.items.Count)
        {
            listValueUI[i].textValue.text = packItem.items[i].value.ToString();
            listValueUI[i].iconType.sprite = packItem.items[i].iconType;

            i++;
        }
    }
}
