using DG.Tweening;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevelController : MonoBehaviour
{
    [SerializeField] private float padding = 48;
    [SerializeField] private float spacingLevel = 64;
    [SerializeField] private float heightLevel = 180;

    private int indexOldParent = -1;

    [Header("Prefabs")]
    public GameObject parentPrefabs;
    public GameObject levelButtonPrefabs;

    [Header("Other Transform")]
    public RectTransform levelContainer;


    public Dictionary<int, string> dicLettersOfLevel = new Dictionary<int, string>();

    [Header("Other Script")]
    public ScrollerController scrollerController;

    public void Start()
    {
        ProcessData();
    }

    private void ProcessData()
    {
    }

    private void SetLevelContainer(int indexParent, int indexCellParent, int indexChild, bool active)
    {
        levelContainer.gameObject.SetActive(false);

        ParentViewData parentViewData = scrollerController.datas[indexParent] as ParentViewData;
        if (parentViewData == null) return;

        ChildCategory child = parentViewData.parent.listChild[indexChild];

        if (indexOldParent != -1 && indexOldParent != indexParent)
        {
            var parentOldData = scrollerController.datas[indexOldParent] as ParentViewData;
            var uiOldParent = scrollerController.scroller.GetCellViewAtDataIndex(indexOldParent) as UIParentCategory;

            if (parentOldData != null && uiOldParent != null)
            {
                parentOldData.indexCateActive = -1;
                scrollerController.InitializeTween(uiOldParent.dataIndex, uiOldParent.cellIndex);
            }
        }

        if (!active)
        {
            parentViewData.indexCateActive = -1;
            levelContainer.gameObject.SetActive(false);
        }
        else
        {
            parentViewData.indexCateActive = indexChild;

            InitListLevel(child);

            float numRowLevel = Mathf.Ceil(child.listLevelID.Count / 4f);
            float expandedValue = numRowLevel * heightLevel + padding + (numRowLevel - 1) * spacingLevel + 8;

            parentViewData.expandedSize = parentViewData.collapsedSize + expandedValue;
        }

        if (indexParent != indexOldParent)
        {
            DOVirtual.DelayedCall(0.2f, () => { scrollerController.InitializeTween(indexParent, indexCellParent); });
            indexOldParent = indexParent;
        }
        else
        {
            scrollerController.InitializeTween(indexParent, indexCellParent);
        }
    }


    public void InitListLevel(ChildCategory child)
    {
        int numLevels = child.listLevelID.Count;
        int startIdLevel = DataManager.Instance.dicLevelIdStart[child];

        ReloadLevelPool(numLevels);

        for (int i = 0; i < numLevels; i++)
        {
            var levelId = startIdLevel + i;

            // Add Letters for Levels
            if (!dicLettersOfLevel.ContainsKey(levelId) && levelId <= DataManager.unlockedLevel)
            {
                string path = $"Data/Level/{child.listLevelID[i]}";
                TextAsset fileLevel = Resources.Load<TextAsset>(path);
                if (fileLevel == null) continue;

                LevelData levelData = JsonConvert.DeserializeObject<LevelData>(fileLevel.text);

                dicLettersOfLevel.Add(levelId, levelData.letters);
            }

            var levelBtn = levelContainer.GetChild(i);
            var levelBtnScript = levelBtn.GetComponent<LevelButton>();

            if (levelId <= DataManager.unlockedLevel)
            {
                levelBtnScript.SetLevel(levelId, dicLettersOfLevel[levelId]);
                if (levelId == DataManager.unlockedLevel)
                {
                    levelBtnScript.SetCurrentLevel();
                }
            }
            else
            {
                levelBtnScript.SetLevel(levelId);
            }

            levelBtn.gameObject.SetActive(true);
        }
    }

    private void ReloadLevelPool(int numLevels)
    {
        while (levelContainer.childCount < numLevels)
        {
            var levelBtn = Instantiate(levelButtonPrefabs, levelContainer);
        }

        for (int i = 0; i < levelContainer.childCount; i++)
        {
            levelContainer.GetChild(i).gameObject.SetActive(false);
        }
    }


    private void SetTransformLevel(Transform transformParent)
    {
        levelContainer.SetParent(transformParent);
        levelContainer.transform.localScale = Vector3.one;
        levelContainer.gameObject.SetActive(true);
    }

    private void DisplayLevelContainer(bool isActive)
    {
        levelContainer.gameObject.SetActive(isActive);
    }

    private void OnEnable()
    {
        GameEvent.setListLevel += SetLevelContainer;
        GameEvent.setTransformLevel += SetTransformLevel;
        GameEvent.setDisplayLevel += DisplayLevelContainer;
        GameEvent.loadLevelinChild += InitListLevel;
    }

    private void OnDisable()
    {
        GameEvent.setListLevel -= SetLevelContainer;
        GameEvent.setTransformLevel -= SetTransformLevel;
        GameEvent.setDisplayLevel -= DisplayLevelContainer;
        GameEvent.loadLevelinChild -= InitListLevel;
    }
}
