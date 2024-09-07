using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusWordCell : EnhancedScrollerCellView
{
    public string wordBonus;
    public TextMeshProUGUI textWordBonus;
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(() => { Debug.Log($"Word Bonus: {wordBonus}"); });   
    }

    public void SetData(string wordBonus)
    {
        this.wordBonus = wordBonus;
        textWordBonus.text = wordBonus;
    }
}