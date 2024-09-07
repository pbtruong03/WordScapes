using UnityEngine;

public class DictionaryButton : ButtonBase
{
    public GameObject dictionaryContainer;


    protected override void OnClick()
    {
        base.OnClick();
        dictionaryContainer.SetActive(!dictionaryContainer.activeSelf);
        GameEvent.displayDictionary?.Invoke();
    }
}
