using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CategorySelection : MonoBehaviour
{
    [Header("DropDown Menu Select")]
    public TMP_Dropdown parentCategory;
    public TMP_Dropdown childCategory;

    [Header("Category")]
    public Dictionary<string, ParentCategory> dicParent;
    public Dictionary<string, ChildCategory> dicChild;
    public List<string> listNameParent;
    List<string> listNameChildCategory;

    private void Start()
    {
        parentCategory.onValueChanged.AddListener(delegate { UpdateChildList(); });
        childCategory.onValueChanged.AddListener(delegate { childChangeValue(); });

        listNameParent = new List<string>();
        listNameChildCategory = new List<string>();


        dicParent = new Dictionary<string, ParentCategory>();
        dicChild = new Dictionary<string, ChildCategory>();
        UpdateParentList();
    }

    public void UpdateParentList()
    {
        dicParent.Clear();
        dicChild.Clear();
        listNameParent.Clear();

        foreach (var category in CategoryManager.Instance.gameData.listParent)
        {
            dicParent.Add(category.name, category);
            listNameParent.Add(category.name);
        }
        childCategory.ClearOptions();
        parentCategory.ClearOptions();
        parentCategory.AddOptions(listNameParent);
        UpdateChildList();
    }

    public void UpdateChildList()
    {
        dicChild.Clear();
        listNameChildCategory.Clear();
        if (parentCategory.options.Count <= 0) return;

        string nameParent = parentCategory.options[parentCategory.value].text;

        if (listNameParent.Contains(nameParent))
        {
            CategoryManager.Instance.UpdateParentSelected(dicParent[nameParent]);

            foreach (var category in dicParent[nameParent].listChild)
            {
                listNameChildCategory.Add(category.name);
            }
            childCategory.ClearOptions();
            childCategory.AddOptions(listNameChildCategory);
        }
        childChangeValue();
    }

    public void childChangeValue()
    {
        var parent = CategoryManager.Instance.gameData.listParent[parentCategory.value];
        if (childCategory.value < parent.listChild.Count)
        {
            CategoryManager.Instance.UpdateChildSelected(parent.listChild[childCategory.value]);
        }
    }
}
