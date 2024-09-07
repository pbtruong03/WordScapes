using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParentCategoryEditor : MonoBehaviour
{
    public TextMeshProUGUI title;
    [Header("Create Parent Category")]
    public Button createButton;
    public Button editButton;
    [Space]
    public Button saveButton;

    [Header("Delete Option")]
    public GameObject deleteOption;
    public GameObject deletePanel;
    public TextMeshProUGUI notificationDelete;
    public Button deleteButton;
    public Button submitDelete;
    public Button cancelDelete;

    [Header("Component Parent Category")]
    public ParentCategory parent;
    public TMP_InputField nameCategory;

    private void Start()
    {
        createButton.onClick.AddListener(() => OnCreateCategory());
        editButton.onClick.AddListener(() => OnEditCategory());
        saveButton.onClick.AddListener(() => OnClickSaveCategory());

        // Delete Option
        deleteButton.onClick.AddListener(() => OnClickDelete());
        submitDelete.onClick.AddListener(() => DeleteCategory());
        cancelDelete.onClick.AddListener(() => deletePanel.SetActive(false));

        nameCategory.onValueChanged.AddListener((value) => { nameCategory.text = value.ToUpper(); });
    }

    private void OnEditCategory()
    {
        title.text = "Edit Parent Category";
        parent = CategoryManager.Instance.parentSelected;
        nameCategory.text = parent.name;
        deleteButton.gameObject.SetActive(true);
    }

    private void OnCreateCategory()
    {
        title.text = "Create Parent Category";
        parent = null;
        nameCategory.text = "";
        deleteButton.gameObject.SetActive(false);
    }
    private void OnClickDelete()
    {
        notificationDelete.text = $"Are you sure you want to delete ChildCategory: \n{parent.name} ?";
        deletePanel.SetActive(true);
    }

    private void DeleteCategory()
    {
        CategoryManager.Instance.RemoveParent();
        deletePanel.SetActive(false);
    }

    private void OnClickSaveCategory()
    {
        if(parent == null)
        {
            CreateNewCategory();
        } else
        {
            SaveChangeCategory();
        }
        this.gameObject.SetActive(false);
    }
    
    private void CreateNewCategory()
    {
        ParentCategory newParent = ScriptableObject.CreateInstance<ParentCategory>();
        newParent.name = nameCategory.text;
        newParent.listChild = new List<ChildCategory>();
        // Add to data
        CategoryManager.Instance.AddNewParent(newParent);
        // Create Asset
        string path = $"Assets/WordScapes/Resources/Data/Category/P_{nameCategory.text}.asset";
        UnityEditor.AssetDatabase.CreateAsset(newParent, path);
        UnityEditor.AssetDatabase.SaveAssets();
    }

    private void SaveChangeCategory()
    {
        parent.name = nameCategory.text;
        CategoryManager.Instance.UpdateOptionCategory();
    }

}
