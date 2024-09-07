using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class ToggleButtonBase : MonoBehaviour
{
    public Toggle toggle;
    public Image backgroundOn;
    public RectTransform handleRect;

    public bool isOn = true;

    public void Start()
    {
        toggle.onValueChanged.AddListener((value) => OnValueChange(value));
    }

    private void OnValueChange(bool value)
    {
        isOn = value;

        ApplyChange();
        if (isOn)
        {
            handleRect.DOAnchorPosX(30f, 0.2f, true).SetEase(Ease.InOutQuad);
            backgroundOn.DOFade(1f, 0.2f).SetEase(Ease.OutQuad);
        }
        else
        {
            handleRect.DOAnchorPosX(-30f, 0.2f, true).SetEase(Ease.InOutQuad);
            backgroundOn.DOFade(0f, 0.2f).SetEase(Ease.OutQuad);
        }

        AudioManager.Instance.PlaySFX(AudioType.ClickSoundFx);
    }

    protected abstract void ApplyChange();
}
