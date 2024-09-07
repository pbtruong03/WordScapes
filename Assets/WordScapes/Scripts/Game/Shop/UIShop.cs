using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    public CanvasGroup canvasShop;
    public Transform shopContainer;

    public Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(CloseShop);
    }

    private void CloseShop()
    {
        canvasShop.DOFade(0, 0.1f);
        shopContainer.DOScale(Vector3.zero, 0.2f);

        DOVirtual.DelayedCall(0.2f, () => { gameObject.SetActive(false); });
    }

    private void OnEnable()
    {
        canvasShop.DOFade(0, 0);
        shopContainer.localScale = Vector3.zero;

        canvasShop.DOFade(1, 0.4f);
        shopContainer.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
    }
}
