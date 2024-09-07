using Sonat;
using UnityEngine;

public class TrackingEvent : MonoBehaviour
{
    public static void LevelStart()
    {
        new SonatLogLevelStart()
        {
            is_first_play = (DataManager.unlockedLevel == GameManager.Instance.currentLevel),

        }.Post();
    }
    public static void LevelEnd(int _level, bool _issucces, bool _isFirstPlay)
    {
        new SonatLogLevelEnd()
        {
            level = _level,
            success = _issucces,
            is_first_play = _isFirstPlay
        }.Post();
    }

    public static void ShowVideoAds(string _placement)
    {
        new SonatLogVideoRewarded()
        {
            placement = _placement,

        }.Post();
    }

    public static void OpenShop()
    {
        new SonatLogOpenShop()
        {

        }.Post();
    }

    public static void UseBooster(string _name)
    {
        new SonatLogUseBooster()
        {
            name = _name,

        }.Post();
    }
}
