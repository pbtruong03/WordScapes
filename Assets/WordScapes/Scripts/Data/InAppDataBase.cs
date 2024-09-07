using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class InAppDataBase : ScriptableObject
{
    public List<ShopPackItem> packs;
}


[System.Serializable]
public class ShopPackItem
{
    public string namePack;
    public ShopItemKey key;

    public string textBonus;
    public List<Item> items;
}

[System.Serializable]
public class Item
{
    public ItemType type;
    public int value;
    public Sprite iconType;
}

public enum ItemType
{
    coin,
    idea_booster,
    point_booster,
    rocket_booster,
    diamond,
    remove_ads
}