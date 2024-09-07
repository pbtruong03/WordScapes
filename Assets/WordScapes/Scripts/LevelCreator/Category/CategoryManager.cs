#if UNITY_EDITOR

using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CategoryManager : SingletonBase<CategoryManager>
{
    [Header("Game Data")]
    public GameData gameData;
    public List<int> availabelIdLevel = new List<int>();

    public ParentCategory parentSelected;
    public ChildCategory childSelected;

    [Header("UI")]
    public ListLevelScroll availableLevel;
    public ListLevelScroll selectedLevel;
    [Space]
    public TMP_InputField inputIdFind;
    public Button FindButton;

    public CategorySelection categorySelection;

    public void Start()
    {
        FindButton.onClick.AddListener(() => FindAvailabelIdLevel());
    }

    public void UpdateParentSelected(ParentCategory parent)
    {
        parentSelected = parent;
    }
    public void UpdateChildSelected(ChildCategory child)
    {
        childSelected = child;
        selectedLevel.listIdLevel = childSelected.listLevelID;
        selectedLevel.UpdateList();
        FindAvailabelIdLevel();
        UpdateLevelInCategory();
    }

    public void FindAvailabelIdLevel()
    {
        availabelIdLevel = new List<int>(SaveLoadLevel.Instance.listIDLevels);
        foreach(var parent in gameData.listParent)
        {
            foreach(var child in parent.listChild)
            {
                foreach(var levelID in child.listLevelID)
                {
                    availabelIdLevel.Remove(levelID);
                }
            }
        }

        string idFind = inputIdFind.text;
        if(idFind != "")
        {
            List<int> tmp = new List<int>();
            for(int i = 0; i < availabelIdLevel.Count; i++)
            {
                if (availabelIdLevel[i].ToString().Contains(idFind))
                {
                    tmp.Add(availabelIdLevel[i]);
                }
            }
            availabelIdLevel = tmp;
        }

        availableLevel.listIdLevel = availabelIdLevel;
        availableLevel.UpdateList();
    }

    public void UpdateLevelInCategory()
    {
        childSelected.listLevelID = selectedLevel.listIdLevel;
        EditorUtility.SetDirty(childSelected);
    }

    /// 
    public void AddNewParent(ParentCategory newParent)
    {
        gameData.listParent.Add(newParent);
        UpdateOptionCategory();
    }
    public void RemoveParent()
    {
        if (gameData.listParent.Contains(parentSelected))
        {
            gameData.listParent.Remove(parentSelected); 
            string assetPath = AssetDatabase.GetAssetPath(parentSelected);
            AssetDatabase.DeleteAsset(assetPath);
            UpdateOptionCategory();

        }
    }

    public void AddNewChild(ChildCategory newChild)
    {
        parentSelected.listChild.Add(newChild);

        EditorUtility.SetDirty(parentSelected);

        UpdateOptionCategory();
    }

    public void RemoveChild()
    {
        if (parentSelected.listChild.Contains(childSelected))
        {
            parentSelected.listChild.Remove(childSelected);

            EditorUtility.SetDirty(parentSelected);

            string assetPath = AssetDatabase.GetAssetPath(childSelected);
            AssetDatabase.DeleteAsset(assetPath);
            UpdateOptionCategory();
        }
    }

    public void UpdateOptionCategory()
    {
        categorySelection.UpdateParentList();
    }

    private void SetDirtyData()
    {

        EditorUtility.SetDirty(gameData);

        EditorUtility.SetDirty(childSelected);

        EditorUtility.SetDirty(parentSelected);
    }

    private void OnDisable()
    {
        SetDirtyData();
    }
}


#endif