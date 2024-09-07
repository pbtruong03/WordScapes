using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardController : MonoBehaviour, IEnhancedScrollerDelegate
{
    public EnhancedScroller scroller;
    public List<RankData> rankDatas;

    public RankCellView rankCellViewPrefab;

    public RankData playerRank;
    public RankCellView playerRankCell;


    // Start is called before the first frame update
    void Start()
    {
        rankDatas = new List<RankData>();
        scroller.Delegate = this;

        LoadData();
    }

    
    private void LoadData()
    {
        rankDatas = LeaderBoardManager.saveLeaderBoard.rankDatas;

        //Player Data
        int playerIndex = LeaderBoardManager.FindPlayerIndex();

        playerRank = rankDatas[playerIndex];

        playerRankCell.SetData(playerRank, playerIndex);

        scroller.ReloadData();
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        RankCellView cellView = scroller.GetCellView(rankCellViewPrefab) as RankCellView;

        cellView.name = $"Rank {dataIndex}";

        cellView.SetData(rankDatas[dataIndex], dataIndex);

        return cellView;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 164;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return rankDatas.Count;
    }

    private void OnEnable()
    {
        if (rankDatas != null)
        {
            LoadData();
        }
    }
}
