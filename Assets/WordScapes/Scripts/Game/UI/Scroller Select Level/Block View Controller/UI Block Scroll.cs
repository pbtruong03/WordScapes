using EnhancedUI.EnhancedScroller;
using System;

public class UIBlockScroll : EnhancedScrollerCellView
{
    protected ScrollViewData _data;

    public virtual void SetData(ScrollViewData data)
    {
        _data = data;
    }

    public virtual void SetData(ScrollViewData data, int indexData, Action<int, int, float, float> updateTween, Action<int, int> endTween)
    {
        _data = data;
    }
}
