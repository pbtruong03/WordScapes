using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIMainMenu : MonoBehaviour
{
    public Image background;
    public TextMeshProUGUI nameGame;

    private void OnEnableUI(bool enable)
    {
        if (enable)
        {
            background.DOFade(1, 0.5f);
            nameGame.DOFade(1, 0.8f).SetEase(Ease.InQuart);
        } else
        {
            background.DOFade(0, 0.25f);
            nameGame.DOFade(0, 0.2f);
        }
    }

    private void OnEnable()
    {
        GameEvent.inMainMenu += OnEnableUI;
    }

    private void OnDisable()
    {
        GameEvent.inMainMenu -= OnEnableUI;
    }
}
