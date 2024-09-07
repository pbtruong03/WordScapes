using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
using UnityEngine;

public class BonusWordController : MonoBehaviour, IEnhancedScrollerDelegate
{
    public List<string> datas;

    public EnhancedScroller scroller;

    public BonusWordCell bonusWordCellPrefabs;
    public float bonusWordCellSize = 64;

    private bool delegatedScroller;

    private void Start()
    {
        delegatedScroller = false;
        datas = new List<string>();
    }

    public void AddNewData(string word)
    {
        datas.Add(word);
    }

    private void ReloadDataScroller()
    {
        if (!delegatedScroller)
        {
            scroller.Delegate = this;
        }

        scroller.ReloadData();
    }

    public void EarnBonusReward(Vector3 bonusPos)
    {
        int numCoin = datas.Count;
        if (numCoin == 0) return;

        UIManager.Instance.objMoveCtrl.CreateObjectMove(TypeMoveObject.Coin, bonusPos, numCoin > 1); 

        DataManager.EarnCoin(numCoin);
    }

    public void ResetData()
    {
        datas.Clear();
    }

    #region Enhanced Scroller
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        BonusWordCell cell = scroller.GetCellView(bonusWordCellPrefabs) as BonusWordCell;

        cell.SetData(datas[dataIndex]);
        cell.name = datas[dataIndex];

        return cell;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return bonusWordCellSize;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return datas.Count;
    }
    #endregion

    private void OnEnable()
    {
        GameEvent.displayBonusWord += ReloadDataScroller;
    }

    private void OnDisable()
    {
        GameEvent.displayBonusWord -= ReloadDataScroller;
    }
}
