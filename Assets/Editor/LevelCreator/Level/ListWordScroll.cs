using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ListWordScroll : MonoBehaviour
{
    public GameObject prefabWordButton;

    [Header("this List Word Scroll")]
    public Transform thisContentTransform;
    public List<string> wordList;

    [Header("other List Word Scroll")]
    public ListWordScroll other;
    public Transform otherContentTransform;

    //update ListWord
    public void UpdateList()
    {
        while (thisContentTransform.childCount < 50)
        {
            Instantiate(prefabWordButton, thisContentTransform);
        }
        // Set word to wordButton
        for (int i = 0; i < thisContentTransform.childCount; i++)
        {
            var child = thisContentTransform.GetChild(i);
            if(i < CreatorManager.Instance.availableWords.Count)
            {
                child.GetComponent<WordButton>().SetWord(CreatorManager.Instance.availableWords[i]);
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    //Delete List
    public void RemoveList()
    {
        wordList.Clear();
        while(thisContentTransform.childCount > 0)
        {
            thisContentTransform.GetChild(0).transform.SetParent(otherContentTransform, false);
        }
        CreatorManager.Instance.UpdateListLetter();
    }

    // Add Word
    public void AddWord(GameObject wordbtn)
    {
        var wordText = wordbtn.GetComponent<TextMeshProUGUI>().text;
        wordList.Add(wordText);
        wordbtn.transform.SetParent(thisContentTransform, false);
    }

    //Remove Word
    public void RemoveWord(GameObject wordbtn) 
    {
        var wordText = wordbtn.GetComponent<TextMeshProUGUI>().text;
        wordList.Remove(wordText);

        other.AddWord(wordbtn);
        CreatorManager.Instance.UpdateListLetter();
    }
}
