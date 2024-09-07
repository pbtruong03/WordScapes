using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private InAppDataBase shopData;

    [Header("Prefabs")]
    public BasicItemShop basicPack;
    public SingleItemShop singlePack;
    public MegaItemShop megaPack;

    [Header("Transform")]
    public Transform dailyTransform;
    public Transform contentShopTransform;
    public Transform coinsPackTransform;

    [Header("Time")]
    public TextMeshProUGUI textTime;

    private void Start()
    {
        InitShop();
    }

    private void InitShop()
    {
        foreach(ShopPackItem shopPack in shopData.packs)
        {
            ShopItemKey shopItemKey = shopPack.key;

            switch (shopItemKey)
            {
                case ShopItemKey.daily_item_1:
                case ShopItemKey.daily_item_2:
                case ShopItemKey.daily_item_3:
                    InitDailyItem(shopPack);
                    break;

                case ShopItemKey.mega_bundle:
                case ShopItemKey.brilliant_bundle:
                case ShopItemKey.supper_bundle:
                case ShopItemKey.ultimate_bundle:
                    InitMegaItem(shopPack);
                    break;

                case ShopItemKey.package_piggy_bank:
                case ShopItemKey.package_double_coin:
                case ShopItemKey.remove_ads:
                    InitPackageItem(shopPack);
                    break;

                case ShopItemKey.package_coin_1:
                case ShopItemKey.package_coin_2:
                case ShopItemKey.package_coin_3:
                case ShopItemKey.package_coin_4:
                case ShopItemKey.package_coin_5:
                    InitPackCoinItem(shopPack);
                    break;

                default:
                    Debug.Log("Missing Key");
                    break;
            }
        }

        coinsPackTransform.SetParent(contentShopTransform);
    }

    private void InitDailyItem(ShopPackItem shopPack)
    {
        var shopItem = Instantiate(basicPack, dailyTransform);
        shopItem.SetData(shopPack);
    }

    private void InitMegaItem(ShopPackItem shopPack)
    {
        var shopItem = Instantiate(megaPack, contentShopTransform);
        shopItem.SetData(shopPack);
    }

    private void InitPackageItem(ShopPackItem shopPack)
    {
        var shopItem = Instantiate(singlePack, contentShopTransform);
        shopItem.SetData(shopPack);
    }

    private void InitPackCoinItem(ShopPackItem shopPack)
    {
        var shopItem = Instantiate(basicPack, coinsPackTransform);
        shopItem.SetData(shopPack);
    }
}
