using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChildCategoryButton : ButtonBase
{
    public int indexParent;
    public int indexCellParent;
    public int indexChild;

    [Header("Infomation")]
    public ChildCategory child;
    public string nameChild;
    public bool onSelect;

    [Header("Component")]
    public Image childBackground;
    public Image borderSelected;
    public TextMeshProUGUI textNameChild;

    public GameObject unlockedChild;
    public GameObject lockedChild;

    public void SetChild(ChildCategory child, int indexParent, int indexCellParent, int indexChild)
    {
        this.indexParent = indexParent;
        this.indexChild = indexChild;

        this.child = child;
        unlockedChild.SetActive(true);
        lockedChild.SetActive(false);
        button.interactable = true;

        nameChild = this.child.name;
        textNameChild.text = nameChild;
        childBackground.sprite = this.child.backgroundImage;
    }


    // Set Child locked
    public void SetChild()
    {
        unlockedChild.SetActive(false);
        lockedChild.SetActive(true);
        button.interactable = false;
    }

    public void OnSelectChild(bool selected)
    {
        onSelect = selected;
        borderSelected.gameObject.SetActive(onSelect);
    }

    protected override void OnClick()
    {
        base.OnClick();

        bool selectTmp = !onSelect;
        GameEvent.changeChildSelect?.Invoke(false);
        OnSelectChild(selectTmp);
        GameEvent.setListLevel?.Invoke(indexParent, indexCellParent, indexChild, onSelect);
    }

    private void ResetData()
    {
        onSelect = false;
        borderSelected.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvent.changeChildSelect += OnSelectChild;
    }

    private void OnDisable()
    {
        ResetData();
        GameEvent.changeChildSelect -= OnSelectChild;
    }
}
