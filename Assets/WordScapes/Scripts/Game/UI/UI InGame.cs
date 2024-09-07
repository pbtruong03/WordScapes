using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIInGame : MonoBehaviour
{
    [Header("Text UI")]
    public TextMeshProUGUI textLevelID;
    public TextMeshProUGUI textCategoryOrder;

    [Header("")]
    public GameObject gridBoard;


    private void OnEnableUI(bool isEnable)
    {
        if (isEnable)
        {
            textLevelID.text = $"LEVEL {GameManager.Instance.currentLevel}";
            textCategoryOrder.text = "PINE 1";

            textLevelID.DOFade(1f, 0.1f);
            textCategoryOrder.DOFade(1f, 0.1f);
        }
        else
        {
            textLevelID.DOFade(0f, 0.1f);
            textCategoryOrder.DOFade(0f, 0.1f);
        }
    }

    private void OnEnable()
    {
        GameEvent.inGameplay += OnEnableUI;
    }

    private void OnDisable()
    {
        GameEvent.inGameplay -= OnEnableUI;
    }
}
