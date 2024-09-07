using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupRankup : UIPopupBase
{
    public Image congraImage;
    public GameObject brillianceObj;
    public TextMeshProUGUI textBrillianceValue;
    public int value;

    public UIPopupWin uiPopupWin;

    public Button skipButton;

    private void Start()
    {
        skipButton.onClick.AddListener(SkipClick);
    }

    private void SkipClick()
    {
        OnDisablePopup();
        DOVirtual.DelayedCall(0.2f, () => {
            gameObject.SetActive(false);
            uiPopupWin.gameObject.SetActive(true);
        });
    }

    IEnumerator IECounter(int numBrillianceReward)
    {
        yield return new WaitForSeconds(0.66f);
        for(int i = 0; i < numBrillianceReward; i++)
        {
            value++;
            textBrillianceValue.text = value.ToString();
            yield return new WaitForSeconds(0.02f);
        }

        //yield return new WaitForSeconds(0.5f);
        //OnDisablePopup();
    }

    public override void OnEnablePopup()
    {
        base.OnEnablePopup();
        congraImage.transform.localScale = Vector3.zero;
        value = DataManager.brilliance;
        textBrillianceValue.text = value.ToString();

        congraImage.DOFade(1, 0.15f);
        congraImage.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack);
        brillianceObj.transform.DOScale(1, 0.1f);

        skipButton.transform.localScale = Vector3.zero;
        skipButton.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack);

        int numBrillianceReward = LevelManager.Instance.levelData.brilliancePoint;

        DataManager.RankUp(numBrillianceReward);

        StartCoroutine(IECounter(numBrillianceReward));
    }

    public override void OnDisablePopup()
    {
        base.OnDisablePopup();
        congraImage.DOFade(0, 0.1f);
        brillianceObj.transform.DOScale(0, 0.1f);
        skipButton.transform.DOScale(0, 0.1f);
    }
}
