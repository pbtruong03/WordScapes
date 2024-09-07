#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;

public class ListLevelScroll : MonoBehaviour
{
    public GameObject prefabLevel;

    [Header("This")]
    public Transform thisContentTransform;
    public List<int> listIdLevel = new List<int>();

    [Header("Other")]
    public Transform otherContentTransform;
    public ListLevelScroll other;

    public void UpdateList()
    {
        while(thisContentTransform.childCount < listIdLevel.Count)
        {
            var levelButton = Instantiate(prefabLevel, thisContentTransform);
        }
        for (int i = 0; i < thisContentTransform.childCount; i++)
        {
            thisContentTransform.GetChild(i).gameObject.SetActive(false);
        }
        for(int i = 0; i < listIdLevel.Count; i++)
        {
            var levelButton = thisContentTransform.GetChild(i).gameObject;
            levelButton.SetActive(true);
            levelButton.GetComponent<LevelIdButton>()?.SetIDLevel(listIdLevel[i]);
        }
    }

    public void AddLevel(int idLevel)
    {
        listIdLevel.Add(idLevel);
    }

    public void RemoveLevel(GameObject level)
    {
        int idLevel = level.GetComponent<LevelIdButton>().id;

        level.transform.SetParent(otherContentTransform);
        listIdLevel.Remove(idLevel);
        other.AddLevel(idLevel);
    }
}

#endif