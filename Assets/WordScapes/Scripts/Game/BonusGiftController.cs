using DG.Tweening;
using Sonat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusGiftController : MonoBehaviour
{
    public Button bonusGiftBtn;

    [Header("Bonus Gift")]
    public Image bgBonusGift;
    public CanvasGroup bonusGiftGroup;
    public Button closeBonusGiftBtn;
    public Button detailBtn;
    public Button watchBtn;

    [Header("Collect Bonus Gift")]
    public Image bgBonusCollect;
    public CanvasGroup bonusCollectGroup;
    [Space]
    public Image iconItem;
    public TextMeshProUGUI textValue;
    public Button collectBtn;

    private Item itemReward;

    [Header("Detail Bonus Gift")]
    public Image bgDetail;
    public CanvasGroup detailGroup;
    public Button closeDetailBtn;

    [Header("Sprite Item")]
    public Sprite coinSprite;
    public Sprite ideaSprite;
    public Sprite pointSprite;
    public Sprite rocketSprite;

    private void Start()
    {
        bonusGiftBtn.onClick.AddListener(OpenBonusGift);
        closeBonusGiftBtn.onClick.AddListener(CloseBonusGift);

        watchBtn.onClick.AddListener(WatchVideo);

        detailBtn.onClick.AddListener(OpenDetailBonus);
        closeDetailBtn.onClick.AddListener(CloseDetailBonus);

        collectBtn.onClick.AddListener(CollectReward);
    }

    #region BonusGift
    private void OpenBonusGift()
    {
        bgBonusGift.DOFade(0, 0);
        bonusGiftGroup.alpha = 0;
        bonusGiftGroup.transform.localScale = Vector3.zero;

        bgBonusGift.gameObject.SetActive(true);

        bgBonusGift.DOFade(0.5f, 0.35f);
        bonusGiftGroup.DOFade(1, 0.35f);
        bonusGiftGroup.transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.OutBack);
    }

    private void CloseBonusGift()
    {
        bonusGiftGroup.DOFade(0, 0.1f);
        bonusGiftGroup.transform.DOScale(Vector3.zero, 0.1f);
        bgBonusGift.DOFade(0, 0.1f);

        DOVirtual.DelayedCall(0.1f,() => { bgBonusGift.gameObject.SetActive(false); });
    }

    private void WatchVideo()
    {
        var log = new SonatLogVideoRewarded()
        {
            placement = "bonus_gift",
        };

        Kernel.Resolve<AdsManager>().ShowVideoAds(EarnGift, log);
    }

    private void EarnGift()
    {
        int ranInt = Random.Range(0, 101);
        itemReward = new Item();

        if (ranInt > 92)
        {
            itemReward.type = ItemType.rocket_booster;
            itemReward.value = 1;
        }
        else if (ranInt > 80)
        {
            itemReward.type = ItemType.point_booster;
            itemReward.value = 1;
        }
        else if (ranInt > 65)
        {
            itemReward.type = ItemType.idea_booster;
            itemReward.value = 1;
        }
        else if (ranInt > 55)
        {
            itemReward.type = ItemType.coin;
            itemReward.value = 150;
        }
        else if (ranInt > 35)
        {
            itemReward.type = ItemType.coin;
            itemReward.value = 80;
        }
        else if (ranInt > 15)
        {
            itemReward.type = ItemType.coin;
            itemReward.value = 50;
        }
        else
        {
            itemReward.type = ItemType.coin;
            itemReward.value = 40;
        }

        CloseBonusGift();

        SetDataBonus();
        OpenBonusCollect();
    }

    private void SetDataBonus()
    {
        switch (itemReward.type)
        {
            case ItemType.coin:
                textValue.text = $"+{itemReward.value} Coin";
                iconItem.sprite = coinSprite;
                break;
            case ItemType.idea_booster:
                textValue.text = $"+{itemReward.value} Idea";
                iconItem.sprite = ideaSprite;
                break;
            case ItemType.point_booster:
                textValue.text = $"+{itemReward.value} Point";
                iconItem.sprite = pointSprite;
                break;
            case ItemType.rocket_booster:
                textValue.text = $"+{itemReward.value} Rocket";
                iconItem.sprite = rocketSprite;
                break;
            default:
                break;
        }
    }
    #endregion

    #region Collect Bonus Gift
    private void OpenBonusCollect()
    {
        bonusCollectGroup.alpha = 0;
        bonusCollectGroup.transform.localScale = Vector3.zero;
        bgBonusCollect.DOFade(0, 0);

        bgBonusCollect.gameObject.SetActive(true);

        bgBonusCollect.DOFade(0.5f, 0.35f);
        bonusCollectGroup.DOFade(1, 0.35f);
        bonusCollectGroup.transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.OutBack);
    }

    private void CollectReward()
    {
        switch (itemReward.type)
        {
            case ItemType.coin:
                DataManager.EarnCoin(itemReward.value);
                break;
            case ItemType.idea_booster:
                DataManager.Instance.EarnIdeaBooster(itemReward.value);
                break;
            case ItemType.point_booster:
                DataManager.Instance.EarnPointBooster(itemReward.value);
                break;
            case ItemType.rocket_booster:
                DataManager.Instance.EarnRocketBooster(itemReward.value);
                break;
            default:
                break;
        }

        CloseBonusCollect();
    }

    private void CloseBonusCollect()
    {
        bgBonusCollect.DOFade(0, 0.1f);
        bonusCollectGroup.DOFade(0, 0.1f);
        bonusCollectGroup.transform.DOScale(Vector3.zero, 0.1f);

        DOVirtual.DelayedCall(0.1f, () => { bgBonusCollect.gameObject.SetActive(false); });
    }
    #endregion

    #region DetailBonusGift
    private void OpenDetailBonus()
    {
        detailGroup.alpha = 0;
        detailGroup.transform.localScale = Vector3.zero;
        bgDetail.DOFade(0, 0);

        bgDetail.gameObject.SetActive(true);

        bgDetail.DOFade(0.5f, 0.35f);
        detailGroup.DOFade(1, 0.35f);
        detailGroup.transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.OutBack);
    }

    private void CloseDetailBonus()
    {
        bgDetail.DOFade(0, 0.1f);
        detailGroup.DOFade(0, 0.1f);
        detailGroup.transform.DOScale(Vector3.zero, 0.1f);

        DOVirtual.DelayedCall(0.1f, () => { bgDetail.gameObject.SetActive(false); });
    }
    #endregion
}
