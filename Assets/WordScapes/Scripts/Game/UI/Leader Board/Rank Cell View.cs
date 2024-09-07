using EnhancedUI.EnhancedScroller;
using System.Data;
using TMPro;

public class RankCellView : EnhancedScrollerCellView
{
    public TextMeshProUGUI textRank;
    public TextMeshProUGUI textPlayerName;
    public TextMeshProUGUI textBrilliance;

    public TextMeshProUGUI textRankTitle;


    public void SetData(RankData rankData, int dataIndex)
    {
        this.dataIndex = dataIndex;

        textRank.text = (dataIndex + 1).ToString();
        textPlayerName.text = rankData.playerName;
        textBrilliance.text = rankData.totalBrilliance.ToString();

        if(dataIndex == 0)
        {
            textRankTitle.text = "Legendary";
            textRankTitle.gameObject.SetActive(true);
        }
        else
        {
            textRankTitle.gameObject.SetActive(false);
        }
    }
}
