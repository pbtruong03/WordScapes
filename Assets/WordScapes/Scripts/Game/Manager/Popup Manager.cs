using System.Collections;
using UnityEngine;

public class PopupManager : SingletonBase<PopupManager>
{
    public UIPopupBase popupRankUp;
    public UIPopupBase popupWin;
    public UIPopupBase popupWinReward;

    public GameObject shop;

    public void StartDisplayPopup()
    {
        StartCoroutine(IEDisplayPopup());
    }

    IEnumerator IEDisplayPopup()
    {
        popupRankUp.OnEnablePopup();
        yield return null;
    }

    public void DisplayWinReward()
    {
        popupWinReward.gameObject.SetActive(true);
    }

    public void OpenShop()
    {
        shop.SetActive(true);
        Kernel.Resolve<FireBaseController>().LogEvent("open_shop");
    }
}
