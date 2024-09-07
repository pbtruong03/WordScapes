using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryController : MonoBehaviour, IEnhancedScrollerDelegate
{
    private int _levelID;
    private List<string> _listWord;
    private List<MeaningWordData> _datas;

    public EnhancedScroller scroller;

    public MeaningWordCell meaningWordCellPrefab;
    public WordnikAPI wordnikAPI;

    public FlickSnapScroller flickSnap;
    public float cellViewSize = 873; 
    public float calculateStartCellBias = 0f;

    private bool delegatedScroller;

    void Start()
    {
        _datas = new List<MeaningWordData>();
        _listWord = new List<string>();
        delegatedScroller = false;  
    }

    public void LoadNewLevel(int levelID)
    {
        if (this._levelID != levelID)
        {
            this._levelID = levelID;
            _datas.Clear();
            flickSnap.MaxDataElements = 0;

            scroller.ReloadData();
        }
    }

    public void AddNewData(string wordStr)
    {
        wordStr = wordStr.ToLower();
        
        if (_listWord.Contains(wordStr)) return;

        _listWord.Add(wordStr);
        wordnikAPI.findMeanWord(wordStr);
    }

    public void AddElement(MeaningWordData data)
    {
        _datas.Add(data);

        flickSnap.MaxDataElements = _datas.Count;
    }

    private void ReloadDataScroller()
    {
        if (!delegatedScroller)
        {
            scroller.Delegate = this;
            scroller.CalculateStartCellBias = calculateStartCellBias;
        }

        scroller.ReloadData();
    }

    #region EnhancedScroll

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        MeaningWordCell meaningCell = scroller.GetCellView(meaningWordCellPrefab) as MeaningWordCell;

        meaningCell.SetData(_datas[dataIndex]);
        meaningCell.name = $"{_datas[dataIndex].word}";

        return meaningCell;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return cellViewSize;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _datas.Count;
    }

    #endregion

    private void OnEnable()
    {
        GameEvent.displayDictionary += ReloadDataScroller;
    }

    private void OnDisable()
    {
        GameEvent.displayDictionary -= ReloadDataScroller;
    }
}
