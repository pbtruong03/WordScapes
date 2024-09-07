using System.Collections.Generic;

public class ParentViewData : ScrollViewData
{
    // Info Parent
    public int index;
    public ParentCategory parent;
    public string parentName;


    // Other Info
    public int indexLevelStart;
    public int indexLevelEnd;

    // Is Active Container Level
    public int indexCateActive;
    public float collapsedSize;
    public float expandedSize;
    public float curentSize;

    // Tween
    public EnhancedUI.EnhancedScroller.Tween.TweenType tweenType;
    public float tweenTimeExpand;
    public float tweenTimeCollapse;

    public float Size
    {
        get
        {
            return (indexCateActive == -1) ? collapsedSize : expandedSize;
        }
    }

    public float SizeDifference
    {
        get
        {
            if (indexCateActive == -1)
            {
                return curentSize - collapsedSize;
            } 
            else
            {
                return expandedSize - curentSize;
            }
        }
    }

}
