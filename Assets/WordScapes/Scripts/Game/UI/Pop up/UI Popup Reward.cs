using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupReward : UIPopupBase
{
    [Header("Component")]
    public TextMeshProUGUI textLevelID;
    public TextMeshProUGUI textComplete;
    public TextMeshProUGUI textValueReward;
    public int valueReward;

    public Button collectButton;

    [Header("Coin Move")]
    public RectTransform rectForm;

    private void Start()
    {
        collectButton.onClick.AddListener(() => CollectCoinReward());
    }

    private void CollectCoinReward()
    {
        collectButton.gameObject.SetActive(false);
        StartCoroutine(IEDisplayCoinMove());
    }

    IEnumerator IEDisplayCoinMove()
    {
        UIManager.Instance.objMoveCtrl.CreateObjectMove(TypeMoveObject.Coin, rectForm.position, true);

        DataManager.EarnCoin(valueReward);

        yield return new WaitForSeconds(1.3f);

        UIManager.Instance.DisplayMainMenu();
        OnDisablePopup();
    }

    public override void OnEnablePopup()
    {
        transform.localScale = Vector3.zero;

        base.OnEnablePopup();
        collectButton.gameObject.SetActive(true);
        textLevelID.text = $"LEVEL {GameManager.Instance.currentLevel}";

        int coinReward = DataManager.cateOfLevelID[GameManager.Instance.currentLevel].Item1.coinReward;
        valueReward = coinReward;

        textValueReward.text = valueReward.ToString();

        transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.InQuad);
    }

    public override void OnDisablePopup()
    {
        base.OnDisablePopup();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OnEnablePopup();
    }
}
