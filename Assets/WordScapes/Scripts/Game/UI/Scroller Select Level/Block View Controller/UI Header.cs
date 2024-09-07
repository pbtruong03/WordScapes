using System.Collections.Generic;
using UnityEngine;

public class UIHeader : UIBlockScroll
{
    private HeaderViewData _headerData;

    public override void SetData(ScrollViewData data)
    {
        base.SetData(data);

        HeaderViewData header = data as HeaderViewData;
        // Set Size 
        header.cellSize = gameObject.GetComponent<RectTransform>().rect.height;
    }
}
