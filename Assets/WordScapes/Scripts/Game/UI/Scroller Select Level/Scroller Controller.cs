using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    public List<ScrollViewData> datas;

    public EnhancedScroller scroller;
    public RectTransform rectScroller;

    public EnhancedScrollerCellView headerUIPrefab;
    public EnhancedScrollerCellView parentUIPrefab;
    public EnhancedScrollerCellView footerUIPrefab;

    [Header("Height of header and footer")]
    public float headerHeight;
    public float footerHeight;

    [Header("Jump Tween")]
    public EnhancedScroller.TweenType tweenType;
    public float tweenTime;

    //
    private bool _lastPadderActive;
    private float _lastPadderSize;


    private void Start()
    {
        Application.targetFrameRate = 60;

        rectScroller = scroller.GetComponent<RectTransform>();
        scroller.Delegate = this;

        scroller.lookAheadAfter = 1000f;
        scroller.lookAheadBefore = 1000f;

        LoadData();
    }

    private void LoadData()
    {
        datas = new List<ScrollViewData>();
        int levelIdStart = 1;

        datas.Add(new HeaderViewData(headerHeight));

        foreach (var parent in DataManager.Instance.gameData.listParent)
        {
            ParentViewData parentData = new ParentViewData();

            parentData.parent = parent;
            parentData.parentName = parent.name;

            parentData.indexLevelStart = levelIdStart;

            foreach (var child in parent.listChild)
            {
                levelIdStart += child.listLevelID.Count;
            }

            parentData.indexLevelEnd = levelIdStart - 1;

            parentData.indexCateActive = -1;
            parentData.expandedSize = 472;
            parentData.collapsedSize = 472;
            parentData.curentSize = 472;

            parentData.tweenType = Tween.TweenType.easeInOutSine;
            parentData.tweenTimeCollapse = 0.15f;
            parentData.tweenTimeExpand = 0.3f;

            datas.Add(parentData);
        }

        datas.Add(new FooterViewData(footerHeight));

        OpenCurrentLevel();
        scroller.ReloadData();

    }
        int indexCurParent = -1;
        int indexCurChild = -1;


    private void OpenCurrentLevel()
    {
        int curLevel = DataManager.unlockedLevel;
        int curLevelId = DataManager.listLevelID[curLevel - 1];

        int numDatas = datas.Count;
        ParentViewData parentData;

        for (int i = 0; i < numDatas; i++)
        {
            if (datas[i] is ParentViewData)
            {
                parentData = datas[i] as ParentViewData;

                parentData.curentSize = parentData.collapsedSize;

                if (parentData.indexLevelStart <= curLevel && parentData.indexLevelEnd >= curLevel)
                {
                    int numChild = parentData.parent.listChild.Count;
                    for (int indexCate = 0; indexCate < numChild; indexCate++)
                    {
                        if (parentData.parent.listChild[indexCate].listLevelID.Contains(curLevelId))
                        {
                            parentData.indexCateActive = indexCate;
                            //GameEvent.loadLevelinChild?.Invoke(parentData.parent.listChild[indexCate]);
                            indexCurParent = i;
                            indexCurChild = indexCate;

                            break;
                        }
                    }
                    JumpToDataIndex(i);
                }
                else
                {
                    parentData.indexCateActive = -1;
                }
            }
        }

        StartCoroutine(IESetListLevel());
    }

    IEnumerator IESetListLevel()
    {
        yield return new WaitForSeconds(0.03f);
        GameEvent.setListLevel?.Invoke(indexCurParent, scroller.GetCellViewAtDataIndex(indexCurParent).cellIndex, indexCurChild, true);
    }

    public void JumpToDataIndex(int dataIndex)
    {
        scroller.JumpToDataIndex(Mathf.Clamp(dataIndex, 0, datas.Count - 1), 0.5f, 0.5f, true, tweenType, tweenTime);
    }


    #region Tween ParentCategory
    public void InitializeTween(int dataIndex, int cellViewIndex)
    {
        ParentViewData parentData = datas[dataIndex] as ParentViewData;

        if (parentData == null) return;



        var cellPosition = scroller.GetScrollPositionForCellViewIndex(cellViewIndex, EnhancedScroller.CellViewPositionEnum.Before);

        var tweenCellOffset = cellPosition - scroller.ScrollPosition;

        scroller.IgnoreLoopJump(true);

        scroller.ReloadData();

        cellPosition = scroller.GetScrollPositionForCellViewIndex(cellViewIndex, EnhancedScroller.CellViewPositionEnum.Before);

        scroller.SetScrollPositionImmediately(cellPosition - tweenCellOffset);

        scroller.IgnoreLoopJump(false);

        // Set Last Padder
        _lastPadderActive = scroller.LastPadder.IsActive();
        _lastPadderSize = scroller.LastPadder.minHeight;

        if(parentData.indexCateActive == -1)
        {
            scroller.LastPadder.minHeight -= parentData.SizeDifference;
        }
        else
        {
            scroller.LastPadder.minHeight += parentData.SizeDifference;
        }

        scroller.LastPadder.gameObject.SetActive(true);

        var uiParent = scroller.GetCellViewAtDataIndex(dataIndex) as UIParentCategory;

        uiParent.BeginTween();
    }


    private void TweenUpdated(int dataIndex, int cellViewIndex, float newValue, float delta)
    {
        scroller.LastPadder.minHeight -= delta;
    }


    private void TweenEnd(int dataIndex, int cellViewIndex)
    {
        ParentViewData parentData = datas[dataIndex] as ParentViewData;

        if (parentData != null && parentData.indexCateActive != -1)
        {
            JumpToDataIndex(dataIndex);
        }

        scroller.LastPadder.gameObject.SetActive(_lastPadderActive);
        scroller.LastPadder.minHeight = _lastPadderSize;
    }
    #endregion

    #region EnhancedSroller
    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        ParentViewData parentData = datas[dataIndex] as ParentViewData;

        if (parentData != null)
        {
            return parentData.Size;
        }
        else if (datas[dataIndex] is HeaderViewData) 
        {
            return headerHeight;
        }
        else
        {
            return footerHeight;
        }
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return datas.Count;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        UIBlockScroll blockView;

        if (datas[dataIndex] is ParentViewData)
        {
            blockView = this.scroller.GetCellView(parentUIPrefab) as UIParentCategory;
            blockView.SetData(datas[dataIndex], dataIndex, TweenUpdated, TweenEnd);
        }
        else if (datas[dataIndex] is HeaderViewData)
        {
            blockView = this.scroller.GetCellView(headerUIPrefab) as UIHeader;
            blockView.SetData(datas[dataIndex]);
        }
        else
        {
            blockView = this.scroller.GetCellView(footerUIPrefab) as UIFooter;
            blockView.SetData(datas[dataIndex]);
        }

        return blockView;
    }
    #endregion



    private void OnEnable()
    {
        if (datas != null && datas.Count > 0)
        {
            OpenCurrentLevel();
            scroller.ReloadData();
        }
    }
}
