using EnhancedUI.EnhancedScroller;
using TMPro;
using UnityEngine;
public class MeaningWordCell : EnhancedScrollerCellView
{
    private MeaningWordData _data;

    [Header("UI Component")]
    public TextMeshProUGUI textWord;
    public TextMeshProUGUI textMeanContent;
    public TextMeshProUGUI textSourceDic;


    public void SetData(MeaningWordData data)
    {
        _data = data;

        textWord.text = _data.word;
        textMeanContent.text = _data.meanContent;
        textSourceDic.text = _data.sourceDic;
    }
}
