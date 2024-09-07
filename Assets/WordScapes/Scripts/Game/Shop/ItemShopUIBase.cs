using Sonat;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopUIBase : MonoBehaviour
{
    private ShopPackItem packItem;
    private ShopItemKey shopItemKey;
    public Image backGround;

    [Header("Price")]
    public float price;
    public Image iconPrice;
    public TextMeshProUGUI textPrice;

    [Header("Bonus")]
    public GameObject bonusObj;
    public TextMeshProUGUI textBonus;

    private Button button;

    protected virtual void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    protected virtual void OnClick()
    {
        Kernel.Resolve<Purchaser>().Buy((int)shopItemKey, BuyPurchaser, new SonatLogBuyShopItemIapInput("", "", 1));
    }

    public virtual void SetData(ShopPackItem packItem)
    {
        textPrice.text = $"{Kernel.Resolve<Purchaser>().GetPriceText((int)packItem.key)}";
        this.packItem = packItem;
        shopItemKey = packItem.key;
        
        if (packItem.textBonus == "")
        {
            bonusObj.SetActive(false);
        }
        else
        {
            bonusObj.SetActive(true);
            textBonus.text = packItem.textBonus;
        }
    }

    public void BuyPurchaser(bool success)
    {
        Debug.Log(success);
        if (!success) return;

        foreach (var item in packItem.items)
        {
            switch (item.type)
            {
                case ItemType.coin:
                    DataManager.EarnCoin(item.value);
                    break;

                case ItemType.diamond:
                    DataManager.EarnDiamond(item.value);
                    break;

                case ItemType.idea_booster:
                    DataManager.Instance.EarnIdeaBooster(item.value);
                    break;

                case ItemType.point_booster:
                    DataManager.Instance.EarnPointBooster(item.value);
                    break;

                case ItemType.rocket_booster:
                    DataManager.Instance.EarnRocketBooster(item.value);
                    break;

                case ItemType.remove_ads:
                    Kernel.Resolve<AdsManager>().EnableNoAds();
                    break;
            }
        }
    }
}

[Serializable]
public struct ValueShopUI
{
    public Image iconType;
    public TextMeshProUGUI textValue;
}
