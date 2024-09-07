
using UnityEngine;

public class URLButton : ButtonBase
{
    public string url;

    protected override void OnClick()
    {
        base.OnClick();

        Application.OpenURL(url);
    }
}
